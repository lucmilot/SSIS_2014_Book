--BAD programming practice (returns 11 columns, 121,317 rows)
SELECT * FROM Sales.SalesOrderDetail;

--BETTER programming practice (returns 6 columns, 121,317 rows)
SELECT SalesOrderID, SalesOrderDetailID, OrderQty,
ProductID, UnitPrice, UnitPriceDiscount
FROM Sales.SalesOrderDetail;

--BEST programming practice (returns 6 columns, 79 rows)
SELECT SalesOrderID, SalesOrderDetailID, OrderQty,
ProductID, UnitPrice, UnitPriceDiscount
FROM Sales.SalesOrderDetail
WHERE ModifiedDate = '2008-07-01';

--Old Query
SELECT [BusinessEntityID]
      ,[FirstName]
      ,[EmailPromotion]
FROM [AdventureWorks].[Person].[Person]

--New Query
SELECT
  --Cast the ID to an Int and use a friendly name
  cast([BusinessEntityID] as int) as BusinessID
  --Trim whitespaces, convert empty strings to Null
  ,NULLIF(LTRIM(RTRIM(FirstName)), ‘’) AS FirstName
  --Cast the Email Promotion to a bit
  ,cast((Case EmailPromotion When 0 Then 0 Else 1 End) as bit) as EmailPromoFlag
FROM [AdventureWorks].[Person].[Person]
--Only load the dates you need
Where [ModifiedDate] > ‘2008-12-31’

--Extraction query using UNION ALL
SELECT --Get data from Sales Q1
SalesOrderID,
SubTotal
FROM Sales.SalesQ1
UNION ALL --Combine Sales Q1 and Sales Q2
SELECT --Get data from Sales Q2
SalesOrderID,
SubTotal
FROM Sales.SalesQ2

--Extraction query using a JOIN
SELECT
p.ProductID,
p.[Name] AS ProductName,
p.Color AS ProductColor,
sc.ProductSubcategoryID,
sc.[Name] AS SubcategoryName
FROM Production.Product AS p
INNER JOIN --Join two tables together
Production.ProductSubcategory AS sc
ON p.ProductSubcategoryID = sc.ProductSubcategoryID;

--Extraction query using a JOIN and an ORDER BY
SELECT
p.ProductID,
p.[Name] AS ProductName,
p.Color AS ProductColor,
sc.ProductSubcategoryID,
sc.[Name] AS SubcategoryName
FROM
Production.Product AS p
INNER JOIN --Join two tables together
Production.ProductSubcategory AS sc
ON p.ProductSubcategoryID = sc.ProductSubcategoryID
ORDER BY --Sorting clause
p.ProductID,
sc.ProductSubcategoryID;

USE SourceSystemDatabase;
GO
CREATE PROCEDURE dbo.up_DimCustomerExtract(@date DATETIME)
-- Test harness (also the query statement you’d use in the SSIS source component):
-- Sample execution: EXEC dbo.up_DimCustomerExtract ‘2004-12-20’;
AS BEGIN
  SET NOCOUNT ON;
  SELECT
    --Convert to INT and alias using a friendlier name
    Cast(CUSTOMER_ID as int) AS CustomerID
    --Trim whitespace, convert empty strings to NULL and alias
    ,NULLIF(LTRIM(RTRIM(CUSTOMER_NAME)), ‘’) AS CustomerName
    --Convert to BIT and use friendly alias
    ,Cast(ACTIVE_IND as bit) AS IsActive
    ,CASE
      --Convert magic dates to NULL
      WHEN LOAD_DATE = ‘9999-12-31’ THEN NULL
      --Convert date to smart surrogate number of form YYYYMMDD
      ELSE CONVERT(INT, (CONVERT(NVARCHAR(8), LOAD_DATE, 112)))
      --Alias using friendly name
    END AS LoadDateID
  FROM dbo.Customers
  --Filter rows using input parameter
  WHERE LOAD_DATE = @date;
  SET NOCOUNT OFF;
END;
GO

EXEC dbo.up_DimCustomerExtract ‘2013-12-20’;

sp_configure ‘show advanced options’, 1; --Show advanced configuration options
GO
RECONFIGURE;
GO
sp_configure ‘Ad Hoc Distributed Queries’, 1; --Switch on OPENROWSET functionality
GO
RECONFIGURE;
GO
sp_configure ‘show advanced options’, 0; --Remember to hide advanced options
GO
RECONFIGURE;
GO

--Create temporary table to define the flat file schema
USE AdventureWorks
GO
CREATE TABLE BulkImport(ID INT, CustomerName NVARCHAR(50));

bcp AdventureWorks..BulkImport format nul -c -t , -x -f BulkImport.fmt -T

--Select data from a text file as if it was a table
SELECT
T.* --SELECT * used for illustration purposes only
FROM OPENROWSET(--This is the magic function
BULK ‘c:\ProSSIS\Data\Ch13\BulkImport.txt’, --Path to data file
FORMATFILE = ‘c:\ProSSIS\Data\Ch13\BulkImport.fmt’ --Path to format file
) AS T; --Command requires a table alias

--Selecting from a text file and sorting the results
SELECT
T.OrgID, --Not using SELECT * anymore
T.OrgName
FROM OPENROWSET(
BULK ‘c:\ProSSIS\Data\Ch13\BulkImport.txt’,
FORMATFILE = ‘c:\ProSSIS\Data\Ch13\BulkImport.fmt’
) AS T(OrgID, OrgName) --For fun, give the columns different aliases
ORDER BY T.OrgName DESC; --Sort the results in descending order

SET STATISTICS PROFILE ON; --Show query plan
SELECT
T.OrgID,
T.OrgName
FROM OPENROWSET(
BULK ‘c:\ProSSIS\Data\Ch13\BulkImport.txt’,
FORMATFILE = ‘c:\ProSSIS\Data\Ch13\BulkImport.fmt’
) AS T(OrgID, OrgName)
ORDER BY T.OrgID ASC; --Sort the results by OrgID
SET STATISTICS PROFILE OFF; --Hide query plan

SET STATISTICS PROFILE ON; --Show query plan
SELECT
T.OrgID,
T.OrgName
FROM OPENROWSET(
BULK ‘c:\ProSSIS\Data\Ch13\BulkImport.txt’,
FORMATFILE = ‘c:\ProSSIS\Data\Ch13\BulkImport.fmt’,
ORDER (OrgID ASC) --Declare the text file is already sorted by OrgID
)AS T(OrgID, OrgName)
ORDER BY T.OrgID ASC; --Sort the results by OrgID
SET STATISTICS PROFILE OFF; --Hide query plan

WITH SourceRows AS ( --CTE containing all source rows
SELECT TOP 1000 AccountNumber
FROM AdventureWorks.Sales.Customer
ORDER BY AccountNumber
),
DestinationRows(AccountNumber) AS ( --CTE containing all destination rows
SELECT CustomerAlternateKey
FROM AdventureWorksDW.dbo.DimCustomer
),
RowsInSourceOnly AS ( --CTE: rows where AccountNumber is in source only
SELECT AccountNumber FROM SourceRows --select from previous CTE
EXCEPT --EXCEPT means ‘subtract’
SELECT AccountNumber FROM DestinationRows --select from previous CTE
),
RowsInSourceAndDestination AS( --CTE: AccountNumber in both source & destination
SELECT AccountNumber FROM SourceRows
INTERSECT --INTERSECT means ‘find the overlap’
SELECT AccountNumber FROM DestinationRows
),
RowsInDestinationOnly AS ( --CTE: AccountNumber in destination only
SELECT AccountNumber FROM DestinationRows
EXCEPT --Simply doing the EXCEPT the other way around
SELECT AccountNumber FROM SourceRows 
),
RowLocation(AccountNumber, Location) AS ( --Final CTE
SELECT AccountNumber, 'Source Only' FROM RowsInSourceOnly
UNION ALL --UNION means ‘add’
SELECT AccountNumber, 'Both' FROM RowsInSourceAndDestination
UNION ALL
SELECT AccountNumber, 'Destination Only' FROM RowsInDestinationOnly
)
SELECT * FROM RowLocation --Generate final result
ORDER BY AccountNumber;

--Use a snapshot to make it simple to rollback the DML
USE master;
GO
--To create a snapshot you need to close all other connections on the DB
ALTER DATABASE [AdventureWorksDW] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
ALTER DATABASE [AdventureWorksDW] SET MULTI_USER;
--Check if there is already a snapshot on this DB
IF EXISTS (SELECT [name] FROM sys.databases
WHERE [name] = N’AdventureWorksDW_Snapshot’) BEGIN
--If so RESTORE the database from the snapshot
RESTORE DATABASE AdventureWorksDW
FROM DATABASE_SNAPSHOT = N’AdventureWorksDW_Snapshot’;
--If there were no errors, drop the snapshot
IF @@error = 0 DROP DATABASE [AdventureWorksDW_Snapshot];
END; --if
--OK, let’s create a new snapshot on the DB
CREATE DATABASE [AdventureWorksDW_Snapshot] ON (
NAME = N’AdventureWorksDW_Data’,
--Make sure you specify a valid location for the snapshot file here
FILENAME = N’c:\ProSSIS\Data\Ch13\AdventureWorksDW_Data.ss’)
AS SNAPSHOT OF [AdventureWorksDW];
GO

--List database snapshots
SELECT
d.[name] AS DatabaseName,
s.[name] AS SnapshotName
FROM sys.databases AS s
INNER JOIN sys.databases AS d
ON (s.source_database_id = d.database_id);

--Create a new table and add some rows
USE AdventureWorksDW;
GO
CREATE TABLE dbo.TableToTestSnapshot(ID INT);
GO
INSERT INTO dbo.TableToTestSnapshot(ID) SELECT 1 UNION SELECT 2 UNION SELECT 3;

--Confirm the table exists and has rows
SELECT * FROM dbo.TableToTestSnapshot;

USE AdventureWorksDW;
GO
--Add a column to the destination table to help us track what happened
--You would not do this in a real solution, this just helps the example
ALTER TABLE dbo.DimCustomer ADD Operation NVARCHAR(10);
GO

USE AdventureWorksDW;
GO
--Merge rows from source into the destination
MERGE
--Define the destination table
INTO AdventureWorksDW.dbo.DimCustomer AS [Dest] --Friendly alias
--Define the source query
USING (
SELECT AccountNumber AS CustomerAlternateKey,
--Keep example simple by using just a few data columns
p.FirstName,
p.LastName
FROM AdventureWorks.Sales.Customer c
INNER JOIN AdventureWorks.Person.Person p on c.PersonID=p.BusinessEntityID
) AS [Source] --Friendly alias
--Define the join criteria (how SQL matches source/destination rows)
ON [Dest].CustomerAlternateKey = [Source].CustomerAlternateKey

--If the same key is found in both the source & destination
WHEN MATCHED
--For *illustration* purposes, only update every second row
--AND CustomerAlternateKey % 2 = 0
--Then update data values in the destination
THEN UPDATE SET
[Dest].FirstName  = [Source].FirstName ,
[Dest].LastName = [Source].LastName,
[Dest].Operation = N'Updated'
--Note: <WHERE CustomerAlternateKey =…> clause is implicit

--If a key is in the source but not in the destination
WHEN NOT MATCHED BY TARGET
--Then insert row into the destination
THEN INSERT
(
GeographyKey, CustomerAlternateKey, FirstName,
LastName, DateFirstPurchase, Operation
)
VALUES
(
1, [Source].CustomerAlternateKey, [Source].FirstName,
[Source].LastName, GETDATE(), N'Inserted'
)

--If a key is in the destination but not in the source…
WHEN NOT MATCHED BY SOURCE
--Then do something relevant, say, flagging a status field
THEN UPDATE SET
[Dest].Operation = N'Deleted';
--Note: <WHERE ContactID =…> clause is implicit
--Alternatively you could have deleted the destination row
--but in AdventureWorksDW that would fail due to FK constraints
--WHEN NOT MATCHED BY SOURCE THEN DELETE;
GO

--Have a look at the results
SELECT CustomerAlternateKey, DateFirstPurchase, Operation
FROM AdventureWorksDW.dbo.DimCustomer;
