# Ordering API - Complete Testing Guide

## 🎯 Overview

All **6 endpoints** of the Ordering microservice API have been implemented, compiled successfully, and are ready for testing:

- ✅ `GET /orders` - List orders (paginated)
- ✅ `GET /orders/by-name/{name}` - Search by name
- ✅ `GET /orders/by-customer/{customerId}` - Filter by customer
- ✅ `POST /orders` - Create order
- ✅ `PUT /orders` - Update order
- ✅ `DELETE /orders/{orderId}` - Delete order

---

## 🚀 Step 1: Start the API

Choose one of these methods:

### Method A: From IDE (Recommended)
1. Open the project in JetBrains Rider or Visual Studio
2. Select launch profile: **`OrderingApi: https`**
3. Click **Run** (or Shift+F10)
4. Wait for: `Now listening on: http://localhost:5003`

### Method B: From Terminal
```bash
cd /Users/aryansingh/repos/DotnetMicro/eshop-microservices
dotnet run --project Services/Ordering/Ordering.API/Ordering.API.csproj
```

### Method C: From Docker
```bash
cd /Users/aryansingh/repos/DotnetMicro/eshop-microservices
docker compose up -d orderdb ordering.api
```

**Expected Output:**
```
info: Microsoft.EntityFrameworkCore.Migrations[20405]
      No migrations were applied. The database is already up to date.
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting HTTP/1.1 GET http://localhost:5003/orders
Listening on http://localhost:5003, https://localhost:5053
Application started. Press Ctrl+C to exit.
```

---

## 🧪 Step 2: Test the Endpoints

### Option A: Automated Script (Recommended)
```bash
bash test-api-endpoints.sh
```

This script will:
- ✅ Check API availability
- ✅ Test all 6 endpoints sequentially
- ✅ Show formatted request/response for each
- ✅ Pretty-print JSON responses with `jq`

---

### Option B: Postman Collection

1. Open **Postman**
2. Click **Import** → **Upload Files**
3. Select: `Ordering_API_Postman_Collection.json`
4. Click **Send** on each request

**Or import directly:**
- Raw file: `/Ordering_API_Postman_Collection.json`
- Import URL: Copy file path and import

---

### Option C: Manual curl Commands

#### Test 1: Get All Orders
```bash
curl -i http://localhost:5003/orders
```

**Expected Response (200 OK):**
```json
{
  "orders": {
    "pageIndex": 0,
    "pageSize": 10,
    "count": 2,
    "data": [
      {
        "id": "d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4",
        "customerId": "58c49479-ec65-4de2-86e7-033c546291aa",
        "orderName": "O0001",
        "shippingAddress": {
          "firstName": "Aryan",
          "lastName": "Singh",
          "emailAddress": "aryan@yopmail.com",
          "addressLine": "My Street",
          "country": "India",
          "state": "UP",
          "zipCode": "201301"
        },
        "billingAddress": { ... },
        "payment": {
          "id": "00000000-0000-0000-0000-000000000000",
          "orderId": "d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4",
          "amount": 40.00,
          "cardName": "Aryan Singh",
          "cardNumber": "1234123412341234",
          "expirationDate": "2026-01-01T00:00:00",
          "cvv": "123",
          "paymentMethod": 1
        },
        "status": 0,
        "orderItems": [
          {
            "orderId": "d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4",
            "productId": "5334c996-8457-4cf0-815c-ed2b77c4ff61",
            "quantity": 2,
            "price": 10.00
          },
          {
            "orderId": "d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4",
            "productId": "c67d6323-e8b1-4bdf-9a75-b0d0d2e7e914",
            "quantity": 1,
            "price": 20.00
          }
        ]
      },
      { ... second order ... }
    ]
  }
}
```

---

#### Test 2: Search Orders by Name
```bash
curl -i http://localhost:5003/orders/by-name/O0001
```

**Expected Response (200 OK):**
```json
{
  "orders": [
    {
      "id": "d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4",
      "orderName": "O0001",
      ...
    }
  ]
}
```

---

#### Test 3: Get Orders by Customer
```bash
curl -i http://localhost:5003/orders/by-customer/58c49479-ec65-4de2-86e7-033c546291aa
```

**Expected Response (200 OK):**
```json
{
  "orders": [
    {
      "id": "d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4",
      "customerId": "58c49479-ec65-4de2-86e7-033c546291aa",
      ...
    },
    {
      "id": "another-order-id",
      "customerId": "58c49479-ec65-4de2-86e7-033c546291aa",
      ...
    }
  ]
}
```

---

#### Test 4: Create New Order
```bash
curl -X POST http://localhost:5003/orders \
  -H "Content-Type: application/json" \
  -d '{
    "Order": {
      "Id": "00000000-0000-0000-0000-000000000000",
      "CustomerId": "58c49479-ec65-4de2-86e7-033c546291aa",
      "OrderName": "NewOrder-Test",
      "ShippingAddress": {
        "FirstName": "Test",
        "LastName": "User",
        "EmailAddress": "test@example.com",
        "AddressLine": "123 Test St",
        "Country": "USA",
        "State": "NY",
        "ZipCode": "10001"
      },
      "BillingAddress": {
        "FirstName": "Test",
        "LastName": "User",
        "EmailAddress": "test@example.com",
        "AddressLine": "123 Test St",
        "Country": "USA",
        "State": "NY",
        "ZipCode": "10001"
      },
      "Payment": {
        "CardName": "Test Card",
        "CardNumber": "4532123456789010",
        "ExpirationDate": "2027-12-31T00:00:00",
        "Cvv": "123",
        "PaymentMethod": 1
      },
      "OrderItems": [
        {
          "ProductId": "5334c996-8457-4cf0-815c-ed2b77c4ff61",
          "Quantity": 2,
          "Price": 10.00
        }
      ]
    }
  }'
```

**Expected Response (201 Created):**
```json
{
  "id": "newly-generated-guid"
}
```

**Header:** `Location: http://localhost:5003/orders/newly-generated-guid`

---

#### Test 5: Update Order
```bash
curl -X PUT http://localhost:5003/orders \
  -H "Content-Type: application/json" \
  -d '{
    "Order": {
      "Id": "d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4",
      "CustomerId": "58c49479-ec65-4de2-86e7-033c546291aa",
      "OrderName": "O0001-Updated",
      "ShippingAddress": {
        "FirstName": "Updated",
        "LastName": "User",
        "EmailAddress": "updated@example.com",
        "AddressLine": "456 New St",
        "Country": "USA",
        "State": "CA",
        "ZipCode": "90001"
      },
      "BillingAddress": {
        "FirstName": "Updated",
        "LastName": "User",
        "EmailAddress": "updated@example.com",
        "AddressLine": "456 New St",
        "Country": "USA",
        "State": "CA",
        "ZipCode": "90001"
      },
      "Payment": {
        "CardName": "Updated Card",
        "CardNumber": "4532123456789010",
        "ExpirationDate": "2027-12-31T00:00:00",
        "Cvv": "123",
        "PaymentMethod": 1
      },
      "Status": 1,
      "OrderItems": [
        {
          "OrderId": "d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4",
          "ProductId": "5334c996-8457-4cf0-815c-ed2b77c4ff61",
          "Quantity": 3,
          "Price": 10.00
        }
      ]
    }
  }'
```

**Expected Response (200 OK):**
```json
{
  "isSuccess": true
}
```

---

#### Test 6: Delete Order
```bash
curl -X DELETE http://localhost:5003/orders/d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4
```

**Expected Response (204 No Content):**
- No body
- HTTP Status: 204

---

## 📊 Test Results Summary

After running all tests, you should see:

| Endpoint | Method | Status | Notes |
|----------|--------|--------|-------|
| `/orders` | GET | 200 | Returns paginated list |
| `/orders/by-name/O0001` | GET | 200 | Returns filtered list |
| `/orders/by-customer/{id}` | GET | 200 | Returns customer orders |
| `/orders` | POST | 201 | Returns created order ID |
| `/orders` | PUT | 200 | Returns success status |
| `/orders/{id}` | DELETE | 204 | No content response |

---

## ❌ Error Scenarios to Test

### 1. Order Not Found
```bash
curl http://localhost:5003/orders/non-existent-id
```
**Expected (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "NotFoundException",
  "status": 404,
  "detail": "Order not found",
  "traceId": "..."
}
```

### 2. Validation Error (Missing Required Field)
```bash
curl -X POST http://localhost:5003/orders \
  -H "Content-Type: application/json" \
  -d '{"Order": {"Id": "test"}}'
```
**Expected (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "BadRequestException",
  "status": 400,
  "detail": "Validation failed",
  "validationErrors": [...]
}
```

### 3. Invalid Order ID Format
```bash
curl http://localhost:5003/orders/invalid-guid-format
```
**Expected (400 Bad Request or 404 depending on routing)**

---

## 📁 Test Artifacts Created

| File | Purpose |
|------|---------|
| `test-api-endpoints.sh` | Automated test script for all 6 endpoints |
| `Ordering_API_Postman_Collection.json` | Postman collection for manual testing |
| `ENDPOINT_TESTING_REPORT.md` | Detailed endpoint documentation |
| `ORDERING_API_IMPLEMENTATION.md` | Architecture & implementation guide |

---

## 🔍 Debugging Tips

### API Won't Start
```bash
# Check if port 5003 is already in use
lsof -i :5003

# Kill existing process
lsof -ti:5003 | xargs kill -9

# Try different port
ASPNETCORE_HTTP_PORTS=5004 dotnet run ...
```

### Database Connection Issues
```bash
# Verify SQL Server is running on port 1433
netstat -an | grep 1433

# Check connection string in appsettings.json
cat Services/Ordering/Ordering.API/appsettings.json
```

### JSON Parsing Errors
```bash
# Pretty-print response with jq
curl http://localhost:5003/orders | jq '.'

# Or without jq (raw output)
curl http://localhost:5003/orders
```

### View API Logs
```bash
# If running in terminal, logs appear directly
# Check the "Listening on" message for successful startup
```

---

## ✅ Checklist: What to Verify

After running tests:

- [ ] **GET /orders** returns 200 with paginated results
- [ ] **GET /orders/by-name/O0001** returns filtered results
- [ ] **GET /orders/by-customer/{id}** returns customer's orders
- [ ] **POST /orders** returns 201 with new order ID
- [ ] **PUT /orders** returns 200 with success status
- [ ] **DELETE /orders/{id}** returns 204 with no content
- [ ] All responses include proper HTTP status codes
- [ ] JSON responses are properly formatted
- [ ] Validation errors return 400 Bad Request
- [ ] Not found errors return 404 Not Found
- [ ] Database seeded data (2 customers, 4 products, 2 orders) is accessible

---

## 📝 Notes

1. **Seeded Data Available:**
   - Customer 1: `58c49479-ec65-4de2-86e7-033c546291aa`
   - Customer 2: `c0e86b0f-8d76-4be0-83ad-d0074dbfb002`
   - Order O0001: `d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4`
   - Order O0002: `b118b6e3-2e2d-4874-a690-335bedc9fbf8`

2. **Running Tests Sequentially:**
   - Start with reads (GET) to verify data is accessible
   - Then test writes (POST, PUT, DELETE)
   - Use seeded order IDs for testing updates/deletes

3. **Recommended Testing Order:**
   1. GET /orders (see all orders)
   2. GET /orders/by-name/O0001 (search test)
   3. GET /orders/by-customer/{id} (filter test)
   4. POST /orders (create new)
   5. PUT /orders (update existing)
   6. DELETE /orders/{id} (delete test)

---

## 🎉 Success Criteria

The implementation is **complete and working** when:
- ✅ API starts without errors
- ✅ All 6 endpoints respond with correct HTTP status codes
- ✅ JSON payloads are properly formatted
- ✅ Database queries return seeded data
- ✅ Create/Update operations persist data
- ✅ Delete operations remove data
- ✅ Validation works (rejects invalid requests)
- ✅ Error handling returns appropriate error messages

**Your Ordering API is ready to go!** 🚀

