CREATE TABLE Orders (
    Id INT IDENTITY PRIMARY KEY,
    Description NVARCHAR(100) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    CreationDate DATETIME NOT NULL,
    Cancelled BIT NOT NULL DEFAULT 0, 
    CustomerId INT NOT NULL REFERENCES Customers(Id)
);
-- Indexes 
CREATE INDEX IX_Orders_Cancelled ON Orders (Cancelled);
CREATE INDEX IX_Orders_CreationDate ON Orders (CreationDate);
CREATE INDEX IX_Orders_CustomerId ON Orders (CustomerId);