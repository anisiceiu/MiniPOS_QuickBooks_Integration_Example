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


CREATE TABLE [dbo].[Sales](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[SaleDate] [datetime] NULL,
	[TotalAmount] [decimal](18, 2) NULL,
	[QuickBooksInvoiceId] [nvarchar](50) NULL,
	[InvoiceNumber] [nvarchar](50) NULL,
	[DueDate] [datetime] NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[TaxAmount] [decimal](18, 2) NULL,
	[ShippingAmount] [decimal](18, 2) NULL,
	[SubTotal] [decimal](18, 2) NULL,
	[Currency] [nvarchar](10) NULL,
	[Notes] [nvarchar](500) NULL,
	[PaymentStatus] [nvarchar](20) NULL,
	[SyncStatus] [nvarchar](20) NULL,
	[LastSyncedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Sales] ADD  CONSTRAINT [DF_Sales_TotalAmount]  DEFAULT ((0)) FOR [TotalAmount]
GO

ALTER TABLE [dbo].[Sales] ADD  DEFAULT ((0)) FOR [DiscountAmount]
GO

ALTER TABLE [dbo].[Sales] ADD  DEFAULT ((0)) FOR [TaxAmount]
GO

ALTER TABLE [dbo].[Sales] ADD  DEFAULT ((0)) FOR [ShippingAmount]
GO

ALTER TABLE [dbo].[Sales] ADD  CONSTRAINT [DF_Sales_SubTotal]  DEFAULT ((0)) FOR [SubTotal]
GO

ALTER TABLE [dbo].[Sales] ADD  DEFAULT ('BDT') FOR [Currency]
GO

ALTER TABLE [dbo].[Sales] ADD  DEFAULT ('Unpaid') FOR [PaymentStatus]
GO

ALTER TABLE [dbo].[Sales] ADD  DEFAULT ('Pending') FOR [SyncStatus]
GO

ALTER TABLE [dbo].[Sales]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([Id])
GO




CREATE TABLE [dbo].[SaleItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SaleId] [int] NULL,
	[ProductId] [int] NULL,
	[Quantity] [int] NULL,
	[UnitPrice] [decimal](18, 2) NULL,
	[Description] [nvarchar](255) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[TaxAmount] [decimal](18, 2) NULL,
	[LineTotal] [decimal](18, 2) NULL,
	[QuickBooksLineId] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SaleItems] ADD  DEFAULT ((0)) FOR [DiscountAmount]
GO

ALTER TABLE [dbo].[SaleItems] ADD  DEFAULT ((0)) FOR [TaxAmount]
GO

ALTER TABLE [dbo].[SaleItems]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO

ALTER TABLE [dbo].[SaleItems]  WITH CHECK ADD FOREIGN KEY([SaleId])
REFERENCES [dbo].[Sales] ([Id])
GO



CREATE TABLE QuickBooksTokens
(
    Id INT PRIMARY KEY IDENTITY,

    AccessToken NVARCHAR(MAX),
    RefreshToken NVARCHAR(MAX),

    RealmId NVARCHAR(100),

    AccessTokenExpiresAt DATETIME,

    UpdatedAt DATETIME DEFAULT GETDATE()
)


