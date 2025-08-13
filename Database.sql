-- Create Database
CREATE DATABASE IBSRAStore;
GO

USE IBSRAStore;
GO

-- Products Table
CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    OriginalPrice DECIMAL(10,2) NOT NULL,
    ImageUrl NVARCHAR(500) NOT NULL,
    Rating DECIMAL(3,2) NOT NULL,
    ReviewCount INT NOT NULL,
    Features NVARCHAR(MAX) NOT NULL, -- JSON array of features
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);

-- Orders Table
CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId NVARCHAR(50) NOT NULL UNIQUE,
    ProductId INT NOT NULL,
    CustomerEmail NVARCHAR(255) NOT NULL,
    CustomerName NVARCHAR(255) NOT NULL,
    TotalAmount DECIMAL(10,2) NOT NULL,
    TaxAmount DECIMAL(10,2) NOT NULL,
    SubtotalAmount DECIMAL(10,2) NOT NULL,
    PaymentStatus NVARCHAR(50) DEFAULT 'Pending',
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- Payment Details Table
CREATE TABLE PaymentDetails (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    CardNumberLast4 NVARCHAR(4) NOT NULL,
    CardHolderName NVARCHAR(255) NOT NULL,
    ExpiryDate NVARCHAR(5) NOT NULL,
    PaymentMethod NVARCHAR(50) DEFAULT 'Credit Card',
    TransactionId NVARCHAR(100),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);

-- Billing Address Table
CREATE TABLE BillingAddresses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    Address NVARCHAR(500) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    ZipCode NVARCHAR(20) NOT NULL,
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);

-- Insert Sample Products
INSERT INTO Products (Name, Price, OriginalPrice, ImageUrl, Rating, ReviewCount, Features) VALUES
('Premium Wireless Headphones', 299.99, 399.99, 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400&h=300&fit=crop', 4.8, 1247, '["Active Noise Cancelling", "30h Battery Life", "Premium Sound Quality", "Wireless Charging"]'),
('Smart Fitness Watch', 199.99, 249.99, 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400&h=300&fit=crop', 4.6, 892, '["Heart Rate Monitor", "GPS Tracking", "Water Resistant", "7 Day Battery"]'),
('Ultra-Book Laptop', 1299.99, 1499.99, 'https://images.unsplash.com/photo-1496181133206-80ce9b88a853?w=400&h=300&fit=crop', 4.9, 2156, '["Intel i7 Processor", "16GB RAM", "512GB SSD", "4K Display"]');

-- Stored Procedures

-- Get All Products
CREATE PROCEDURE GetAllProducts
AS
BEGIN
    SELECT 
        Id,
        Name,
        Price,
        OriginalPrice,
        ImageUrl,
        Rating,
        ReviewCount,
        Features,
        CreatedAt,
        UpdatedAt
    FROM Products
    ORDER BY CreatedAt DESC;
END
GO

-- Get Product By Id
CREATE PROCEDURE GetProductById
    @ProductId INT
AS
BEGIN
    SELECT 
        Id,
        Name,
        Price,
        OriginalPrice,
        ImageUrl,
        Rating,
        ReviewCount,
        Features,
        CreatedAt,
        UpdatedAt
    FROM Products
    WHERE Id = @ProductId;
END
GO

-- Create Order
CREATE PROCEDURE CreateOrder
    @OrderId NVARCHAR(50),
    @ProductId INT,
    @CustomerEmail NVARCHAR(255),
    @CustomerName NVARCHAR(255),
    @TotalAmount DECIMAL(10,2),
    @TaxAmount DECIMAL(10,2),
    @SubtotalAmount DECIMAL(10,2),
    @CardNumberLast4 NVARCHAR(4),
    @ExpiryDate NVARCHAR(5),
    @Address NVARCHAR(500),
    @City NVARCHAR(100),
    @ZipCode NVARCHAR(20)
AS
BEGIN
    BEGIN TRANSACTION;
    
    BEGIN TRY
        -- Insert Order
        INSERT INTO Orders (OrderId, ProductId, CustomerEmail, CustomerName, TotalAmount, TaxAmount, SubtotalAmount, PaymentStatus)
        VALUES (@OrderId, @ProductId, @CustomerEmail, @CustomerName, @TotalAmount, @TaxAmount, @SubtotalAmount, 'Completed');
        
        DECLARE @NewOrderId INT = SCOPE_IDENTITY();
        
        -- Insert Payment Details
        INSERT INTO PaymentDetails (OrderId, CardNumberLast4, CardHolderName, ExpiryDate, TransactionId)
        VALUES (@NewOrderId, @CardNumberLast4, @CustomerName, @ExpiryDate, NEWID());
        
        -- Insert Billing Address
        INSERT INTO BillingAddresses (OrderId, Address, City, ZipCode)
        VALUES (@NewOrderId, @Address, @City, @ZipCode);
        
        COMMIT TRANSACTION;
        
        SELECT @NewOrderId as OrderId;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Get Order Details
CREATE PROCEDURE GetOrderDetails
    @OrderId NVARCHAR(50)
AS
BEGIN
    SELECT 
        o.Id,
        o.OrderId,
        o.CustomerEmail,
        o.CustomerName,
        o.TotalAmount,
        o.TaxAmount,
        o.SubtotalAmount,
        o.PaymentStatus,
        o.CreatedAt,
        p.Name as ProductName,
        p.Price as ProductPrice,
        p.ImageUrl as ProductImage,
        pd.CardNumberLast4,
        pd.TransactionId,
        ba.Address,
        ba.City,
        ba.ZipCode
    FROM Orders o
    INNER JOIN Products p ON o.ProductId = p.Id
    INNER JOIN PaymentDetails pd ON o.Id = pd.OrderId
    INNER JOIN BillingAddresses ba ON o.Id = ba.OrderId
    WHERE o.OrderId = @OrderId;
END
GO
