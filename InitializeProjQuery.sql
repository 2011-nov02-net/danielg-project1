CREATE SCHEMA Proj;
GO

--Create Customers table
-- DROP TABLE Proj.Customer;
CREATE TABLE Proj.Customer (
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) 
);
-- Get entire Customer table
SELECT * FROM Proj.Customer;

-- Create GenOrder Table 
-- DROP TABLE Proj.GenOrder;
CREATE TABLE Proj.GenOrder(
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerID INT NOT NULL,
    StoreID INT NOT NULL,
    Cost DECIMAL NOT NULL,
    Date DATETIME NOT NULL 
);
-- Get Entire GenOrder Table
SELECT * FROM Proj.GenOrder;
-- Add Foreign Key from Genorder to CustomerID
-- ALTER TABLE Proj.GenOrder DROP CONSTRAINT FK_CustomerID;
ALTER TABLE [Proj].[GenOrder]
ADD CONSTRAINT FK_CustomerID
FOREIGN KEY (CustomerID) REFERENCES Proj.Customer(ID);
-- Add Foreign Key from Genorder to StoreID
-- ALTER TABLE Proj.GenOrder DROP CONSTRAINT FK_StoreID;
ALTER TABLE Proj.GenOrder 
ADD CONSTRAINT FK_StoreID
FOREIGN KEY (StoreID) REFERENCES Proj.Store(ID);
-- Make Cost column a real decimal
ALTER TABLE [Proj].[Genorder]
ALTER COLUMN Cost DECIMAL(10,2);

-- Create Store table 
-- DROP TABLE Proj.Store;
CREATE TABLE Proj.Store (
    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Location NVARCHAR(80) NOT NULL
);
SELECT * FROM Proj.Store;

-- Create Product table
-- DROP TABLE Proj.Product;
CREATE TABLE Proj.Product (
    Name NVARCHAR(100) NOT NULL PRIMARY KEY,
    Price DECIMAL NOT NULL
);
-- Get entire Product Table
SELECT * FROM Proj.Product;
-- Add Check to ensure price is above 0
ALTER TABLE Proj.Product
ADD CONSTRAINT CHK_PriceNotNeg
CHECK (Price >= 0);
-- Alter the Price Column because I forgot to make it have a true decimal
ALTER TABLE [Proj].[Product]
ALTER COLUMN Price DECIMAL(10,2);

-- Create AggInventory Table
-- DROP TABLE Proj.AggInventory;
CREATE TABLE Proj.AggInventory (
    StoreID INT NOT NULL,
    Product NVARCHAR(100) NOT NULL,
    InStock INT NOT NULL
);
-- Get Entire Agg Inventory Table
SELECT * FROM Proj.AggInventory;
-- Add Composite Primary Key
ALTER TABLE [Proj].[AggInventory]
ADD CONSTRAINT PK_StoreIdProduct
PRIMARY KEY (StoreID, Product);
-- Add Foreign Key From AggInventory to StoreID
-- ALTER TABLE Proj.AggInventory DROP CONSTRAINT FK_InventoryStoreID;
ALTER TABLE Proj.AggInventory
ADD CONSTRAINT FK_InventoryStoreID 
FOREIGN KEY (StoreID) REFERENCES Proj.Store(ID);
-- Add Foriegn Key From AggInventory To ProductName
ALTER TABLE Proj.AggInventory 
ADD CONSTRAINT FK_InventoryProductName
FOREIGN KEY (Product) REFERENCES Proj.Product(Name);
-- Add Check to ensure Stock does not go below 0
ALTER TABLE Proj.AggInventory
ADD CONSTRAINT CHK_InventoryNotNeg
CHECK (InStock >= 0);

-- Create AggOrders Table 
-- DROP TABLE Proj.AggOrders;
CREATE TABLE Proj.AggOrders (
    OrderID INT NOT NULL,
    Product NVARCHAR(100) NOT NULL,
    Amount INT NOT NULL
);
-- Add Composite Primary Key
ALTER TABLE Proj.AggOrders
ADD CONSTRAINT PK_OrderIdProduct
PRIMARY KEY (OrderID, Product);
-- Get Entire AggOrders table
SELECT * FROM Proj.AggOrders;
-- Add Foreign Key From AggOrders To OrderID
-- ALTER TABLE Proj.AggOrders DROP CONSTRAINT FK_OrderID;
ALTER TABLE Proj.AggOrders
ADD CONSTRAINT FK_OrderID
FOREIGN KEY (OrderID) REFERENCES Proj.GenOrder(ID);
-- Add Foreign Key From AggOrders to Product Name 
ALTER TABLE Proj.AggOrders
ADD CONSTRAINT FK_ProductName
FOREIGN KEY (Product) REFERENCES Proj.Product(Name);
-- Add Check Constraint to check if a product is being bought too many times
ALTER TABLE Proj.AggOrders 
ADD CONSTRAINT CHK_ProductAmt
CHECK (Amount < 11 AND Amount >= 0);

ALTER TABLE Proj.AggOrders 
DROP CONSTRAINT CHK_ProductAmt
-- ^ DDL
-- v DML

-- Insert A few Customers. Only need to do name 
INSERT INTO [Proj].[Customer] VALUES ('Daniel Grant');
INSERT INTO [Proj].[Customer] VALUES ('Larry Fine');
INSERT INTO [Proj].[Customer] VALUES ('Curly Howard');
INSERT INTO [Proj].[Customer] VALUES ('Moe Howard');
-- Get all values from customer
SELECT * FROM [Proj].[Customer];

-- Insert Stores into Store Table
INSERT INTO [Proj].[Store] VALUES ('Norwich');
INSERT INTO [Proj].[Store] VALUES ('Boston');
INSERT INTO [Proj].[Store] VALUES ('New York');
INSERT INTO [Proj].[Store] VALUES ('Hartford');
-- Get all values from Store
SELECT * FROM [Proj].[Store];

-- Create A Few Products
INSERT INTO [Proj].[Product] VALUES ('Dumb Big Mac', 5.50);
INSERT INTO [Proj].[Product] VALUES ('Dumb Cheese Burger', 1.06);
INSERT INTO [Proj].[Product] VALUES ('Dumb French Fries', 0.50);
INSERT INTO [Proj].[Product] VALUES ('Dumb McFlurry', 2.25);
-- Deleted after changing data type of Price Column
DELETE FROM [Proj].[Product] WHERE (Name = 'Dumb Big Mac');
-- Get all data from Product table
SELECT * FROM [Proj].[Product];

-- Stock up the Norwich(1) Inventory
INSERT INTO [Proj].[AggInventory] VALUES (1, 'Dumb Big Mac', 15);
INSERT INTO [Proj].[AggInventory] VALUES (1, 'Dumb Cheese Burger', 23);
INSERT INTO [Proj].[AggInventory] VALUES (1, 'Dumb McFlurry', 10);
INSERT INTO [Proj].[AggInventory] VALUES (1, 'Dumb French Fries', 29);
-- Get Norwich(1) Inventory
SELECT * FROM [Proj].[AggInventory] WHERE StoreID = 1;
-- Stock up the Boston(2) Inventory
INSERT INTO [Proj].[AggInventory] VALUES (2, 'Dumb Big Mac', 25);
INSERT INTO [Proj].[AggInventory] VALUES (2, 'Dumb Cheese Burger', 31);
INSERT INTO [Proj].[AggInventory] VALUES (2, 'Dumb McFlurry', 20);
INSERT INTO [Proj].[AggInventory] VALUES (2, 'Dumb French Fries', 37);
-- Get Boston(2) Inventory
SELECT * FROM [Proj].[AggInventory] WHERE StoreID = 2;
-- Stock up the New York(3) Inventory
INSERT INTO [Proj].[AggInventory] VALUES (3, 'Dumb Big Mac', 33);
INSERT INTO [Proj].[AggInventory] VALUES (3, 'Dumb Cheese Burger', 39);
INSERT INTO [Proj].[AggInventory] VALUES (3, 'Dumb McFlurry', 26);
INSERT INTO [Proj].[AggInventory] VALUES (3, 'Dumb French Fries', 50);
-- Get New York(3) Inventory
SELECT * FROM [Proj].[AggInventory] WHERE StoreID = 3;
-- Stock up the Hartford(4) Inventory
INSERT INTO [Proj].[AggInventory] VALUES (4, 'Dumb Big Mac', 17);
INSERT INTO [Proj].[AggInventory] VALUES (4, 'Dumb Cheese Burger', 26);
INSERT INTO [Proj].[AggInventory] VALUES (4, 'Dumb McFlurry', 14);
INSERT INTO [Proj].[AggInventory] VALUES (4, 'Dumb French Fries', 28);
-- Get Hartford(4) Inventory
SELECT * FROM [Proj].[AggInventory] WHERE StoreID = 4;
-- Get all inventories of all stores
SELECT * FROM [Proj].[AggInventory];