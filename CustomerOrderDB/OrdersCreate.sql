﻿CREATE TABLE Orders (
    Id INT IDENTITY PRIMARY KEY,
    Description NVARCHAR(100) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    CreationDate DATETIME NOT NULL,
    Canceled BIT NOT NULL DEFAULT 0, 
    CustomerId INT NOT NULL REFERENCES Customers(Id)
);