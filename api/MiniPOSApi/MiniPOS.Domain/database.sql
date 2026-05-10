CREATE TABLE Users
(
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Role NVARCHAR(50),
    PasswordHash NVARCHAR(MAX)
)

CREATE TABLE Customers
(
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(30),

    QuickBooksCustomerId NVARCHAR(50),

    CreatedAt DATETIME DEFAULT GETDATE()
)

CREATE TABLE Products
(
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Price DECIMAL(18,2),

    QuickBooksItemId NVARCHAR(50)
)

CREATE TABLE Sales
(
    Id INT PRIMARY KEY IDENTITY,

    CustomerId INT,
    SaleDate DATETIME,

    TotalAmount DECIMAL(18,2),

    QuickBooksInvoiceId NVARCHAR(50),

    FOREIGN KEY (CustomerId)
        REFERENCES Customers(Id)
)

CREATE TABLE SaleItems
(
    Id INT PRIMARY KEY IDENTITY,

    SaleId INT,
    ProductId INT,

    Quantity INT,
    UnitPrice DECIMAL(18,2),

    FOREIGN KEY (SaleId)
        REFERENCES Sales(Id),

    FOREIGN KEY (ProductId)
        REFERENCES Products(Id)
)

CREATE TABLE QuickBooksTokens
(
    Id INT PRIMARY KEY IDENTITY,

    AccessToken NVARCHAR(MAX),
    RefreshToken NVARCHAR(MAX),

    RealmId NVARCHAR(100),

    AccessTokenExpiresAt DATETIME,

    UpdatedAt DATETIME DEFAULT GETDATE()
)