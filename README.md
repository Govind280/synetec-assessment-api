# API for calculating Company wide bonus

Requirement is to build a REST API for calculating company wide bonus for each employee.

I have created a REST service created for calculating bonus for employees in an organisation using .net Core, Entity Framework Core, Microsoft SQL as database. Added Swagger for consumers to easily integrate with their application.

## How to run
### Endpoint and test details
- There is swagger implementation for the API to know the Request and Response with status code in details. Available in Json and can also access after running the application.
- The local url for swagger - https://localhost:44362/swagger/index.html
There are 2 endpoints as mentioned below,
- `POST` /api/BonusPool
- `GET` /api/Employee
#### Request
`https://localhost:44362/api/Employee`

**Using CURL**

`curl -X GET "https://localhost:44362/api/Employee" -H  "accept: */*"`
#### Response
`[
  {
    "fullname": "John Smith",
    "jobTitle": "Accountant (Senior)",
    "salary": 60000,
    "department": {
      "title": "Finance",
      "description": "The finance department for the company"
    }
  },
  {
    "fullname": "Janet Jones",
    "jobTitle": "HR Director",
    "salary": 90000,
    "department": {
      "title": "Human Resources",
      "description": "The Human Resources department for the company"
    }
  }
 ]`
- `GET` /api/Payments/{paymentReference}
#### Request
`https://localhost:44362/api/api/BonusPool`
`{
  "totalBonusPoolAmount": 100000,
  "selectedEmployeeId": 1
}`

**Using CURL**
`curl -X POST "https://localhost:44362/api/BonusPool" -H  "accept: */*" -H  "Content-Type: application/json" -d "{\"totalBonusPoolAmount\":100000,\"selectedEmployeeId\":1}"
`

#### Response
`{
  "amount": 9163,
  "employee": {
    "fullname": "John Smith",
    "jobTitle": "Accountant (Senior)",
    "salary": 60000,
    "department": {
      "title": "Finance",
      "description": "The finance department for the company"
    }
  }
}`

## Unit Test
Used `MS Unite test` and `Moq` for writing unit test.

## Improvements/Extra miles
Below improvements can be implemented if I get more time,
 - I can build token based authentication.
 - If provided with more time I could write more Unit tests for more code coverage
 - Can build validation using Fluent validation

## Cloud Technologies to Consider
We can implement Payment gateway using below cloud technologies,
 - API Gateway / App for hosting service in cloud
 - Serverless functions for processing transactions and triggers
 - Use cloud data store
