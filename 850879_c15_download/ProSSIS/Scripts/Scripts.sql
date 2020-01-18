/* Single Package, Single Transaction */
CREATE TABLE dbo.T1(col1 int)
INSERT dbo.T1(col1) VALUES(1)
INSERT dbo.T1(col1) VALUES("A")
TRUNCATE TABLE dbo.T1

/* Scaling Out with Parallel Loading */
USE [AdventureWorksDW]
GO
CREATE TABLE [dbo].[ctlTaskQueue](
     [TaskQueueID] [smallint] IDENTITY(1,1) NOT NULL,
     [PartitionWhere] [varchar](20) NOT NULL,
     [Priority] [tinyint] NOT NULL,
     [StartDate] [datetime] NULL,
     [CompleteDate] [datetime] NULL,
 CONSTRAINT [PK_ctlTaskQueue] PRIMARY KEY CLUSTERED
(
     [TaskQueueID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])


USE [AdventureWorksDW]
GO
INSERT INTO [dbo].[ctlTaskQueue]
           ([PartitionWhere]
           ,[Priority]
           ,[StartDate]
           ,[CompleteDate])
     VALUES
      (2001,1, null, null),
      (2002,2, null, null),
      (2003,3, null, null),
      (2004,4, null, null),
      (2005,5, null, null),
      (2006,6, null, null),
      (2007,7, null, null),
      (2008,8, null, null),
      (2009,9, null, null),
      (2010,10, null, null),
      (2011,11, null, null)
GO


USE [AdventureWorksDW]
GO
CREATE PROCEDURE [dbo].[ctl_UseTask]
  @TaskQueueID int OUTPUT,
  @PartitionWhere varchar(50) OUTPUT
AS
SELECT TOP 1
@TaskQueueID = TaskQueueID,
@PartitionWhere = PartitionWhere
from ctlTaskQueue
WHERE StartDate is NULL
AND CompleteDate is NULL
ORDER BY Priority asc
IF @TaskQueueID IS NULL
BEGIN
  SET @TaskQueueID = 0
END
ELSE
BEGIN
  UPDATE ctlTaskQueue
  SET StartDate = GETDATE()
  WHERE TaskQueueID = @TaskQueueID
END
GO


DECLARE	@return_value int,
		@TaskQueueID int,
		@PartitionWhere varchar(50)

EXEC	@return_value = [dbo].[ctl_UseTask]
		@TaskQueueID = @TaskQueueID OUTPUT,
		@PartitionWhere = @PartitionWhere OUTPUT

SELECT	convert(int,@TaskQueueID) as N'@TaskQueueID',
		@PartitionWhere as N'@PartitionWhere'

GO


“SELECT [SalesOrderID]
      ,[RevisionNumber]
      ,[OrderDate]
      ,[DueDate]
      ,[ShipDate]
      ,[Status]
      ,[OnlineOrderFlag]
      ,[SalesOrderNumber]
      ,[PurchaseOrderNumber]
      ,[AccountNumber]
      ,[CustomerID]
      ,[SalesPersonID]
      ,[TerritoryID]
      ,[BillToAddressID]
      ,[ShipToAddressID]
      ,[ShipMethodID]
      ,[CreditCardID]
      ,[CreditCardApprovalCode]
      ,[CurrencyRateID]
      ,[SubTotal]
      ,[TaxAmt]
      ,[Freight]
      ,[TotalDue]
      ,[Comment]
      ,[rowguid]
      ,[ModifiedDate]
  FROM [AdventureWorks].[Sales].[SalesOrderHeader]
WHERE [ShipDate] > ‘”+ @[User::strWherePartition]  + “-01-01’ and
[ShipDate] <= ‘” + @[User::strWherePartition]  + “-12-31’ ORDER BY [ShipDate] “


UPDATE ctlTaskQueue
SET CompleteDate = GETDATE()
WHERE TaskQueueID = ?


@Echo off
sqlcmd -E -S localhost -d AdventureWorksDW -Q "update dbo.ctlTaskQueue set StartDate = NULL, CompleteDate = NULL"
FOR /L %%i IN (1, 1, %1) DO (
ECHO Spawning thread %%i
START "Worker%%i" /Min "C:\Program Files\Microsoft SQL Server\120\DTS\Binn\DTEXEC.exe" /FILE "C:\ProSSIS\Code\Ch15\09_ParallelDemo.dtsx" /CHECKPOINTING OFF /REPORTING E
)
