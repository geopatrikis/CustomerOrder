# Customer Order System API Documentation

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
