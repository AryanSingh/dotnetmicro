#!/bin/bash

# Ordering API Endpoint Testing Script
# This script tests all 6 endpoints after the service starts

BASE_URL="${1:-http://localhost:5003}"
echo "Testing Ordering API at: $BASE_URL"
echo "=========================================="

# Color codes
GREEN='\033[0;32m'
BLUE='\033[0;34m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Test 1: Get All Orders (Paginated)
echo -e "\n${BLUE}1. GET /orders (paginated list)${NC}"
curl -s -X GET "$BASE_URL/orders?PageIndex=0&PageSize=10" \
  -H "Content-Type: application/json" | jq '.' || echo "Failed to parse response"

# Test 2: Get Orders by Name
echo -e "\n${BLUE}2. GET /orders/by-name/O0001${NC}"
curl -s -X GET "$BASE_URL/orders/by-name/O0001" \
  -H "Content-Type: application/json" | jq '.' || echo "Failed to parse response"

# Test 3: Get Orders by Customer
echo -e "\n${BLUE}3. GET /orders/by-customer/58c49479-ec65-4de2-86e7-033c546291aa${NC}"
curl -s -X GET "$BASE_URL/orders/by-customer/58c49479-ec65-4de2-86e7-033c546291aa" \
  -H "Content-Type: application/json" | jq '.' || echo "Failed to parse response"

echo -e "\n${YELLOW}=========================================="
echo "Read-only endpoint tests complete!"
echo "Note: Write endpoints (POST/PUT/DELETE) require complete OrderDto payloads"
echo "=========================================="
echo -e "\nTo test write endpoints, refer to ORDERING_API_IMPLEMENTATION.md${NC}"

