USE [AdventureWorksDW2016]
GO

/****** Object:  Table [dbo].[Staging]    Script Date: 1/18/2020 6:36:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Staging](
	[OrderDateAlternateKey] [datetime] NULL,
	[DueDateAlternateKey] [datetime] NULL,
	[ShipDateAlternateKey] [datetime] NULL,
	[CustomerID] [int] NULL,
	[SalesTerritoryAlternateKey] [int] NULL,
	[SalesAmount] [money] NULL,
	[TaxAmt] [money] NULL,
	[Freight] [money] NULL,
	[PromotionAlternateKey] [int] NULL,
	[OrderQuantity] [smallint] NULL,
	[UnitPrice] [money] NULL,
	[UnitPriceDiscount] [money] NULL,
	[CarrierTrackingNumber] [nchar](25) NULL,
	[OnlineOrderFlag] [bit] NULL,
	[SalesOrderID] [int] NULL,
	[CustomerPONumber] [nchar](25) NULL,
	[SalesOrderLineNumber] [smallint] NULL,
	[SalesOrderNumber] [nchar](20) NULL,
	[SalesPersonID] [int] NULL,
	[ProductAlternateKey] [nchar](25) NULL,
	[ProductStandardCost] [money] NULL,
	[CurrencyAlternateKey] [nchar](3) NULL,
	[CurrencyRateID] [int] NULL,
	[ProductID] [int] NULL,
	[Fact_SalesOrderID] [int] NULL,
	[EmployeeNationalIDAlternateKey] [nchar](15) NULL,
	[ResellerCustomerAlternateKey] [nchar](15) NULL
) ON [PRIMARY]
GO


