# ✅ Ordering API - Complete Testing Checklist

## Pre-Testing Checklist

### Environment Setup
- [ ] .NET 8.0 SDK installed (`dotnet --version`)
- [ ] SQL Server running on localhost:1433
- [ ] Port 5003 is available (not in use)
- [ ] Repository cloned/opened in IDE
- [ ] All files downloaded/visible

### Code Verification
- [ ] `Services/Ordering/Ordering.API/Orders/OrderEndpoints.cs` exists
- [ ] `Services/Ordering/Ordering.API/Program.cs` updated
- [ ] `Services/Ordering/Ordering.API/DependencyInjection.cs` updated
- [ ] `Services/Ordering/Ordering.API/appsettings.json` has correct connection string
- [ ] `global.json` shows SDK 8.0.125
- [ ] Launch settings have "OrderingApi: https" profile

### Documentation Available
- [ ] QUICK_START_TESTING.md (✅ provided)
- [ ] TESTING_GUIDE.md (✅ provided)
- [ ] ENDPOINT_TESTING_REPORT.md (✅ provided)
- [ ] ORDERING_API_IMPLEMENTATION.md (✅ provided)
- [ ] test-api-endpoints.sh (✅ provided)
- [ ] Ordering_API_Postman_Collection.json (✅ provided)

---

## API Startup Checklist

### Starting the API
- [ ] Navigate to project: `cd /Users/aryansingh/repos/DotnetMicro/eshop-microservices`
- [ ] Run: `dotnet run --project Services/Ordering/Ordering.API/Ordering.API.csproj`
- [ ] OR use IDE launch profile: "OrderingApi: https"
- [ ] Wait for startup message
- [ ] See: "Listening on http://localhost:5003, https://localhost:5053"
- [ ] See: "Application started. Press Ctrl+C to exit."
- [ ] No error messages appear
- [ ] No exception stack traces

---

## Database Verification

After API starts:
- [ ] Database migrations applied (see in startup logs)
- [ ] No connection errors
- [ ] Seeded data queries execute (see SELECT queries in logs)
- [ ] Tables created: Customers, Products, Orders, OrderItems
- [ ] Sample data loaded (2 customers, 4 products, 2 orders)

---

## Endpoint Testing Checklist

### Test 1: GET /orders (List All Orders)
```bash
curl http://localhost:5003/orders
```
- [ ] HTTP Status: 200 OK
- [ ] Response contains `"orders"` field
- [ ] Contains `"pageIndex"`, `"pageSize"`, `"count"`, `"data"`
- [ ] `"count"` is 2 or more (seeded data)
- [ ] Each order has: id, customerId, orderName, etc.
- [ ] OrderItems array contains items
- [ ] Response is valid JSON

### Test 2: GET /orders/by-name/O0001
```bash
curl http://localhost:5003/orders/by-name/O0001
```
- [ ] HTTP Status: 200 OK
- [ ] Response contains filtered orders
- [ ] At least 1 order returned
- [ ] Returned order name contains "O0001"
- [ ] Response is valid JSON

### Test 3: GET /orders/by-customer/{customerId}
```bash
curl http://localhost:5003/orders/by-customer/58c49479-ec65-4de2-86e7-033c546291aa
```
- [ ] HTTP Status: 200 OK
- [ ] Response contains array of orders
- [ ] All orders have matching customerId
- [ ] At least 1 order returned (seeded data)
- [ ] Each order has orderItems
- [ ] Response is valid JSON

### Test 4: POST /orders (Create Order)
```bash
curl -X POST http://localhost:5003/orders -H "Content-Type: application/json" -d '{...payload...}'
```
- [ ] HTTP Status: 201 Created
- [ ] Response contains new `"id"` (GUID)
- [ ] Location header present: `/orders/{newId}`
- [ ] ID is valid GUID format
- [ ] Response is valid JSON

### Test 5: PUT /orders (Update Order)
```bash
curl -X PUT http://localhost:5003/orders -H "Content-Type: application/json" -d '{...updated payload...}'
```
- [ ] HTTP Status: 200 OK
- [ ] Response contains `"isSuccess": true`
- [ ] No error messages
- [ ] Response is valid JSON

### Test 6: DELETE /orders/{orderId}
```bash
curl -X DELETE http://localhost:5003/orders/d7fb44cf-1abc-4277-bfdd-eb6d4b2e8fb4
```
- [ ] HTTP Status: 204 No Content
- [ ] No response body (204 has no content)
- [ ] Order deleted from database
- [ ] Subsequent GET for same ID returns 404

---

## Error Handling Verification

### Test Not Found (404)
```bash
curl http://localhost:5003/orders/non-existent-id
```
- [ ] HTTP Status: 404 Not Found
- [ ] Response contains error details
- [ ] Error message is descriptive
- [ ] Response is valid JSON

### Test Validation Error (400)
```bash
curl -X POST http://localhost:5003/orders -H "Content-Type: application/json" -d '{"Order": {}}'
```
- [ ] HTTP Status: 400 Bad Request
- [ ] Response contains validation errors
- [ ] Error details explain what's wrong
- [ ] Response is valid JSON

### Test Invalid GUID (400/404)
```bash
curl http://localhost:5003/orders/invalid-guid-format
```
- [ ] HTTP Status: 400 or 404
- [ ] Appropriate error message
- [ ] Response is valid JSON

---

## Automated Testing Checklist

### Run Script
```bash
bash test-api-endpoints.sh
```
- [ ] Script finds API at localhost:5003
- [ ] Script outputs: "✓ API is reachable"
- [ ] Test 1: GET /orders passes
- [ ] Test 2: GET /orders/by-name passes
- [ ] Test 3: GET /orders/by-customer passes
- [ ] Test 4: POST /orders passes
- [ ] Test 5: PUT /orders passes
- [ ] Test 6: DELETE /orders passes
- [ ] Script completes without errors
- [ ] All output is properly formatted

---

## Postman Testing Checklist

### Import Collection
- [ ] Postman is open
- [ ] File > Import selected
- [ ] Ordering_API_Postman_Collection.json uploaded
- [ ] 6 requests appear in workspace
- [ ] Request names are visible:
  - [ ] "1. GET - List All Orders"
  - [ ] "2. GET - Search Orders by Name"
  - [ ] "3. GET - Orders by Customer"
  - [ ] "4. POST - Create New Order"
  - [ ] "5. PUT - Update Existing Order"
  - [ ] "6. DELETE - Delete Order"

### Test Each Request
- [ ] Click Send on each request
- [ ] GET requests return 200
- [ ] POST returns 201
- [ ] PUT returns 200
- [ ] DELETE returns 204
- [ ] All responses are formatted JSON
- [ ] No error messages

---

## Data Persistence Verification

### Create and Verify
- [ ] POST /orders with unique OrderName
- [ ] GET /orders returns newly created order
- [ ] GET /orders/by-name/NewOrderName finds it
- [ ] PUT /orders updates it successfully
- [ ] Changes visible in subsequent GET
- [ ] DELETE /orders/{id} removes it
- [ ] GET returns 404 for deleted order

---

## Documentation Verification

- [ ] All 6 markdown files are readable
- [ ] All 6 files contain relevant information
- [ ] test-api-endpoints.sh is executable
- [ ] Postman collection is valid JSON
- [ ] No broken links in documentation
- [ ] Code examples are correct
- [ ] Sample data GUIDs match seeded data

---

## Final Verification Checklist

### API Running
- [ ] No errors on startup
- [ ] No warnings (except expected ones)
- [ ] Listening on http://localhost:5003
- [ ] Database connected
- [ ] Ready to accept requests

### All Endpoints Working
- [ ] ✅ GET /orders → 200
- [ ] ✅ GET /orders/by-name/{name} → 200
- [ ] ✅ GET /orders/by-customer/{id} → 200
- [ ] ✅ POST /orders → 201
- [ ] ✅ PUT /orders → 200
- [ ] ✅ DELETE /orders/{id} → 204

### Data Integrity
- [ ] Seeded data is accessible
- [ ] Created data persists
- [ ] Updated data reflects changes
- [ ] Deleted data is removed
- [ ] Database consistency maintained

### Error Handling
- [ ] Invalid requests return 400
- [ ] Not found returns 404
- [ ] Server errors return 5xx
- [ ] Error messages are descriptive
- [ ] No unhandled exceptions

### Testing Tools
- [ ] Automated script runs successfully
- [ ] Postman collection imports correctly
- [ ] All curl examples work
- [ ] Documentation is accurate

---

## Success Criteria: ALL CHECKS PASSED ✅

If all checkboxes above are checked, your Ordering API is:

✅ **Fully implemented**  
✅ **Properly configured**  
✅ **Database connected**  
✅ **All endpoints working**  
✅ **Error handling working**  
✅ **Data persisting**  
✅ **Ready for production**  

---

## Troubleshooting Guide

| Issue | Checkbox | Solution |
|-------|----------|----------|
| Port in use | [ ] | `lsof -ti:5003 \| xargs kill -9` |
| SDK not found | [ ] | Check `global.json`, ensure .NET 8.0 installed |
| DB connection fail | [ ] | Ensure SQL Server running on 1433 |
| No seeded data | [ ] | Check logs, run database initialization |
| 404 on endpoints | [ ] | Verify port is 5003, API is running |
| JSON parse error | [ ] | Use `jq` filter or check response format |
| Script won't run | [ ] | `chmod +x test-api-endpoints.sh` |
| Postman import fails | [ ] | Ensure JSON is not corrupted |

---

## Sign-Off

When all checks are complete:

```
Date: [Your Date]
Tester: [Your Name]
Status: ✅ ALL TESTS PASSED
API Version: 1.0
Build: Clean, Zero Errors
Endpoints: 6/6 Working
Database: Connected & Seeded
Documentation: Complete
Ready for: Production Testing ✅
```

---

## Next Steps

1. ✅ Verify all checkboxes above
2. ✅ Run `bash test-api-endpoints.sh` for automation
3. ✅ Review documentation as needed
4. ✅ Document any issues found
5. ✅ API is ready for deployment!

**Congratulations! Your Ordering API is fully tested and ready!** 🎉

