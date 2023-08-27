# Customer Order System API Documentation

The port that is demonstrated in the examples might need to be altered.

## Retrieve List of All Customers

- **Description:** Retrieve a list of all customers in the system.
- **Method:** GET
- **Endpoint:** `/api/customers`
- **Curl Example:** curl -X GET http://localhost:5090/api/customers


## Search Customers by Email

- **Description:** Search for customers by (partial) email address.
- **Method:** GET
- **Endpoint:** `/api/customers/search`
- **Parameters:** `email` (query parameter)
- **Curl Example:** curl -X GET "http://localhost:5090/api/customers/search?email=example"


## Create a Customer

- **Description:** Create a new customer.
- **Method:** POST
- **Endpoint:** `/api/customers`
- **Data:** JSON object representing the customer
- **Curl Example:** curl -X POST -H "Content-Type: application/json" -d '{"firstName": "John", "lastName": "Doe", "email": "johndoe@example.com"}' http://localhost:5090/api/customers


## Update a Customer

- **Description:** Update an existing customer's information.
- **Method:** PUT
- **Endpoint:** `/api/customers/{id}`
- **Parameters:** `id` (path parameter)
- **Data:** JSON object representing the updated customer
- **Curl Example:** curl -X PUT -H "Content-Type: application/json" -d '{"firstName": "Updated", "lastName": "Doe", "email": "updated@example.com"}' http://localhost:5090/api/customers/1

 ## Create Order for Customer

- **Description:** Create an order for a specific customer.
- **Method:** POST
- **Endpoint:** `/api/orders/customer/{customerId}`
- **Parameters:** `customerId` (path parameter)
- **Data:** JSON object representing the order
- **Curl Example:** curl -X POST -H "Content-Type: application/json" -d '{"description": "New Order", "price": 100.00, "creationDate": "2023-08-25T12:00:00", "cancelled": false}' http://localhost:5090/api/orders/customer/1

  
## Get Customer Orders

- **Description:** Retrieve a list of orders for a specific customer.
- **Method:** GET
- **Endpoint:** `/api/orders/customer/{customerId}`
- **Parameters:** `customerId` (path parameter)
- **Curl Example:** curl -X GET http://localhost:5090/api/orders/customer/1
  
## Search Orders by Time Window

- **Description:** Search for orders within a specified time window.
- **Method:** GET
- **Endpoint:** `/api/orders/searchbytime`
- **Parameters:** `startTime` (query parameter), `endTime` (query parameter)
- **Curl Example:** curl -X GET "http://localhost:5090/api/orders/searchbytime?startTime=2023-01-01T00:00:00&endTime=2023-12-31T23:59:59"
  
## Get Cancelled Customer Orders

- **Description:** Retrieve a list of cancelled orders for a specific customer.
- **Method:** GET
- **Endpoint:** `/api/orders/customer/{customerId}/Cancelled`
- **Parameters:** `customerId` (path parameter)
- **Curl Example:** curl -X GET http://localhost:5090/api/orders/customer/1/Cancelled

  
## Cancel an Order

- **Description:** Cancel an order by marking it as cancelled.
- **Method:** PUT
- **Endpoint:** `/api/orders/{orderId}/cancel`
- **Parameters:** `orderId` (path parameter)
- **Curl Example:** curl -X PUT http://localhost:5090/api/orders/1/cancel


#Setting Up the Database:

    1.In the solution, an SQL Project contains scripts to create tables and indexes.
    2.Create a new database in SQL Server.
    3.Execute the provided SQL scripts to create tables and indexes in the new database.

#Altering Connection String:

    1.After creating tables and indexes, update the application's connection string.
    2.Open appsettings.json in your project.
    3.Locate the "ConnectionStrings" section.
    4.Modify "DefaultConnection" with your SQL Server details:
    5.Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;Trusted_Connection=True;TrustServerCertificate=True;
    6.If needed, include username and password instead of usin trusted_connection and certificate.



