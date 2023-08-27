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


# Setting up the Database 

In the solution an SQL Project can be found. There 2 files exist for the creation of the tables and the indexes of the database. The database chosen for this implementation is SQL Server. After creating a new database and creating the tables and the indexes the connection string need to be altered
"ConnectionStrings": {
  "DefaultConnection": "Server=GIVE SERVER NAME;Database=GIVE DATABASE NAME;Trusted_Connection=True;TrustServerCertificate=True;" 
}
It might be needed to give username and password in the Connectionstring if this is how the authentication is configured.



