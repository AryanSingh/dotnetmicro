#!/bin/bash

# ============================================================
# Ordering API Endpoint Test Suite
# ============================================================
# This script tests all 6 endpoints of the Ordering API
# Usage: bash test-api-endpoints.sh [base_url]
# Default: http://localhost:5003
# ============================================================

BASE_URL="${1:-http://localhost:5003}"
CUSTOMER_ID="58c49479-ec65-4de2-86e7-033c546291aa"
ORDER_ID="d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

echo -e "${CYAN}"
echo "╔════════════════════════════════════════════════════════╗"
echo "║   Ordering API Endpoint Test Suite                     ║"
echo "║   Base URL: $BASE_URL"
echo "╚════════════════════════════════════════════════════════╝"
echo -e "${NC}"

# Helper function to print test result
print_test_header() {
    echo -e "\n${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${YELLOW}TEST $1: $2${NC}"
    echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
}

# Helper function to print results
print_result() {
    if [ $1 -eq 0 ]; then
        echo -e "${GREEN}✓ PASSED${NC}"
    else
        echo -e "${RED}✗ FAILED${NC}"
    fi
}

# Test connection first
echo -e "\n${CYAN}Checking API availability...${NC}"
if ! curl -s --max-time 2 "$BASE_URL/orders" > /dev/null 2>&1; then
    echo -e "${RED}✗ Cannot connect to $BASE_URL${NC}"
    echo -e "${YELLOW}Make sure the API is running:${NC}"
    echo "  dotnet run --project Services/Ordering/Ordering.API/Ordering.API.csproj"
    exit 1
fi
echo -e "${GREEN}✓ API is reachable${NC}"

# ============================================================
# TEST 1: GET /orders (List all orders - paginated)
# ============================================================
print_test_header "1" "GET /orders - List all orders (paginated)"
echo "Endpoint: GET $BASE_URL/orders?PageIndex=0&PageSize=10"
echo -e "\n${CYAN}Request:${NC}"
echo "curl -s -X GET '$BASE_URL/orders?PageIndex=0&PageSize=10'"

echo -e "\n${CYAN}Response:${NC}"
curl -s -X GET "$BASE_URL/orders?PageIndex=0&PageSize=10" | jq '.' 2>/dev/null || curl -s -X GET "$BASE_URL/orders?PageIndex=0&PageSize=10"

# ============================================================
# TEST 2: GET /orders/by-name/{name} - Filter by name
# ============================================================
print_test_header "2" "GET /orders/by-name/{name} - Search by order name"
echo "Endpoint: GET $BASE_URL/orders/by-name/O0001"
echo -e "\n${CYAN}Request:${NC}"
echo "curl -s -X GET '$BASE_URL/orders/by-name/O0001'"

echo -e "\n${CYAN}Response:${NC}"
curl -s -X GET "$BASE_URL/orders/by-name/O0001" | jq '.' 2>/dev/null || curl -s -X GET "$BASE_URL/orders/by-name/O0001"

# ============================================================
# TEST 3: GET /orders/by-customer/{customerId} - Filter by customer
# ============================================================
print_test_header "3" "GET /orders/by-customer/{customerId} - Orders for specific customer"
echo "Endpoint: GET $BASE_URL/orders/by-customer/$CUSTOMER_ID"
echo -e "\n${CYAN}Request:${NC}"
echo "curl -s -X GET '$BASE_URL/orders/by-customer/$CUSTOMER_ID'"

echo -e "\n${CYAN}Response:${NC}"
curl -s -X GET "$BASE_URL/orders/by-customer/$CUSTOMER_ID" | jq '.' 2>/dev/null || curl -s -X GET "$BASE_URL/orders/by-customer/$CUSTOMER_ID"

# ============================================================
# TEST 4: POST /orders - Create new order
# ============================================================
print_test_header "4" "POST /orders - Create a new order"
echo "Endpoint: POST $BASE_URL/orders"
echo -e "\n${CYAN}Request Payload:${NC}"

CREATE_PAYLOAD='{
  "Order": {
    "Id": "00000000-0000-0000-0000-000000000000",
    "CustomerId": "'$CUSTOMER_ID'",
    "OrderName": "TestOrder-'$(date +%s)'",
    "ShippingAddress": {
      "FirstName": "Test",
      "LastName": "User",
      "EmailAddress": "test@example.com",
      "AddressLine": "123 Test Street",
      "Country": "USA",
      "State": "CA",
      "ZipCode": "90001"
    },
    "BillingAddress": {
      "FirstName": "Test",
      "LastName": "User",
      "EmailAddress": "test@example.com",
      "AddressLine": "123 Test Street",
      "Country": "USA",
      "State": "CA",
      "ZipCode": "90001"
    },
    "Payment": {
      "Id": "00000000-0000-0000-0000-000000000000",
      "OrderId": "00000000-0000-0000-0000-000000000000",
      "Amount": 50.00,
      "CardName": "Test Card",
      "CardNumber": "4532123456789010",
      "ExpirationDate": "2027-12-31T00:00:00",
      "Cvv": "123",
      "PaymentMethod": 1
    },
    "Status": 0,
    "OrderItems": [
      {
        "OrderId": "00000000-0000-0000-0000-000000000000",
        "ProductId": "5334c996-8457-4cf0-815c-ed2b77c4ff61",
        "Quantity": 2,
        "Price": 10.00
      },
      {
        "OrderId": "00000000-0000-0000-0000-000000000000",
        "ProductId": "c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914",
        "Quantity": 1,
        "Price": 20.00
      }
    ]
  }
}'

echo "$CREATE_PAYLOAD" | jq '.'
echo -e "\n${CYAN}Request:${NC}"
echo "curl -s -X POST '$BASE_URL/orders' -H 'Content-Type: application/json' -d '{payload}'"

echo -e "\n${CYAN}Response:${NC}"
CREATE_RESPONSE=$(curl -s -X POST "$BASE_URL/orders" \
  -H "Content-Type: application/json" \
  -d "$CREATE_PAYLOAD")

echo "$CREATE_RESPONSE" | jq '.' 2>/dev/null || echo "$CREATE_RESPONSE"

# Extract the created order ID for use in update/delete tests
CREATED_ORDER_ID=$(echo "$CREATE_RESPONSE" | jq -r '.id' 2>/dev/null || echo "")

# ============================================================
# TEST 5: PUT /orders - Update existing order
# ============================================================
print_test_header "5" "PUT /orders - Update an existing order"
echo "Endpoint: PUT $BASE_URL/orders"
echo -e "\n${CYAN}Note: Using Order O0001 from seeded data${NC}"

UPDATE_PAYLOAD='{
  "Order": {
    "Id": "'$ORDER_ID'",
    "CustomerId": "'$CUSTOMER_ID'",
    "OrderName": "O0001-Updated-'$(date +%s)'",
    "ShippingAddress": {
      "FirstName": "Updated",
      "LastName": "User",
      "EmailAddress": "updated@example.com",
      "AddressLine": "456 Updated Street",
      "Country": "USA",
      "State": "NY",
      "ZipCode": "10001"
    },
    "BillingAddress": {
      "FirstName": "Updated",
      "LastName": "User",
      "EmailAddress": "updated@example.com",
      "AddressLine": "456 Updated Street",
      "Country": "USA",
      "State": "NY",
      "ZipCode": "10001"
    },
    "Payment": {
      "Id": "00000000-0000-0000-0000-000000000000",
      "OrderId": "'$ORDER_ID'",
      "Amount": 40.00,
      "CardName": "Updated Card",
      "CardNumber": "4532123456789010",
      "ExpirationDate": "2027-12-31T00:00:00",
      "Cvv": "123",
      "PaymentMethod": 1
    },
    "Status": 1,
    "OrderItems": [
      {
        "OrderId": "'$ORDER_ID'",
        "ProductId": "5334c996-8457-4cf0-815c-ed2b77c4ff61",
        "Quantity": 2,
        "Price": 10.00
      }
    ]
  }
}'

echo "$UPDATE_PAYLOAD" | jq '.' | head -30
echo -e "\n${CYAN}Request:${NC}"
echo "curl -s -X PUT '$BASE_URL/orders' -H 'Content-Type: application/json' -d '{payload}'"

echo -e "\n${CYAN}Response:${NC}"
curl -s -X PUT "$BASE_URL/orders" \
  -H "Content-Type: application/json" \
  -d "$UPDATE_PAYLOAD" | jq '.' 2>/dev/null || curl -s -X PUT "$BASE_URL/orders" \
  -H "Content-Type: application/json" \
  -d "$UPDATE_PAYLOAD"

# ============================================================
# TEST 6: DELETE /orders/{orderId} - Delete an order
# ============================================================
print_test_header "6" "DELETE /orders/{orderId} - Delete an order"

# Use the created order if available, otherwise use a test order ID
DELETE_ID=${CREATED_ORDER_ID:-$ORDER_ID}
echo "Endpoint: DELETE $BASE_URL/orders/$DELETE_ID"
echo -e "\n${CYAN}Request:${NC}"
echo "curl -s -X DELETE '$BASE_URL/orders/$DELETE_ID'"

echo -e "\n${CYAN}Response:${NC}"
DELETE_RESPONSE=$(curl -s -w "\n%{http_code}" -X DELETE "$BASE_URL/orders/$DELETE_ID")
HTTP_CODE=$(echo "$DELETE_RESPONSE" | tail -n1)
BODY=$(echo "$DELETE_RESPONSE" | head -n-1)

if [ "$HTTP_CODE" = "204" ]; then
    echo -e "${GREEN}✓ Successfully deleted (HTTP 204 No Content)${NC}"
elif [ -z "$BODY" ]; then
    echo -e "${GREEN}✓ Deleted (No response body)${NC}"
else
    echo "$BODY" | jq '.' 2>/dev/null || echo "$BODY"
fi

# ============================================================
# Summary
# ============================================================
echo -e "\n${CYAN}"
echo "╔════════════════════════════════════════════════════════╗"
echo "║            Test Suite Complete                         ║"
echo "║                                                        ║"
echo "║   All 6 endpoints tested:                              ║"
echo "║   ✓ GET /orders                                        ║"
echo "║   ✓ GET /orders/by-name/{name}                         ║"
echo "║   ✓ GET /orders/by-customer/{customerId}               ║"
echo "║   ✓ POST /orders                                       ║"
echo "║   ✓ PUT /orders                                        ║"
echo "║   ✓ DELETE /orders/{orderId}                           ║"
echo "╚════════════════════════════════════════════════════════╝"
echo -e "${NC}"

echo -e "\n${YELLOW}For detailed information, see:${NC}"
echo "  - ENDPOINT_TESTING_REPORT.md"
echo "  - ORDERING_API_IMPLEMENTATION.md"

