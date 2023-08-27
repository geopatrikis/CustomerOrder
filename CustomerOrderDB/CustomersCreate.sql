CREATE TABLE Customers (
    Id INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    CONSTRAINT UQ_Customers_Email UNIQUE (Email)
);
-- Indexes
CREATE INDEX IX_Customers_Email ON Customers (Email);