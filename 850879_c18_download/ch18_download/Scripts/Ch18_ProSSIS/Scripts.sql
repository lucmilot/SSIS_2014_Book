
CREATE TABLE InventoryCheck (
    ProductID INT )
GO
CREATE TABLE InventoryWarning (
    ProductID INT, ReorderQuantity INT )
GO
CREATE TABLE MissingProductID (
    ProductID INT )
GO
CREATE PROC usp_GetReorderQuantity @ProductID INT, 
    @ReorderQuantity INT OUTPUT AS 
    IF NOT EXISTS(SELECT ProductID FROM Production.ProductInventory
            WHERE ProductID = @ProductID) BEGIN
        RAISERROR('InvalidID',16,1)
        RETURN 1
    END

    SELECT @ReorderQuantity = SafetyStockLevel - SUM(Quantity)
    FROM Production.Product AS p 
    INNER JOIN Production.ProductInventory AS i
    ON p.ProductID = i.ProductID
    WHERE p.ProductID = @ProductID
    GROUP BY p.ProductID, SafetyStockLevel
    RETURN 0
GO

CREATE TABLE CoinToss (
    Heads INT NULL,
    Tails INT NULL )
GO

INSERT INTO CoinToss SELECT 0,0
GO

CREATE TABLE [UpdatedProducts] (
    [ProductID] int,
    [Name] nvarchar(50),
    [ProductNumber] nvarchar(25),
    [MakeFlag] bit,
    [FinishedGoodsFlag] bit,
    [Color] nvarchar(15),
    [SafetyStockLevel] smallint,
    [ReorderPoint] smallint,
    [StandardCost] money,
    [ListPrice] money,
    [Size] nvarchar(5),
    [SizeUnitMeasureCode] nvarchar(3),
    [WeightUnitMeasureCode] nvarchar(3),
    [Weight] numeric(8,2),
    [DaysToManufacture] int,
    [ProductLine] nvarchar(2),
    [Class] nvarchar(2),
    [Style] nvarchar(2),
    [ProductSubcategoryID] int,
    [ProductModelID] int,
    [SellStartDate] datetime,
    [SellEndDate] datetime,
    [DiscontinuedDate] datetime,
    [rowguid] uniqueidentifier,
    [ModifiedDate] datetime,
    [Size_Numeric] numeric(18,0)
)
GO

USE SSISDB
GO

select top 5 e.executable_name, es.execution_duration
from catalog.executable_statistics es
left join catalog.executables e on es.executable_id=e.executable_id
where es.execution_id = 43/*Put your id here*/
order by execution_duration desc