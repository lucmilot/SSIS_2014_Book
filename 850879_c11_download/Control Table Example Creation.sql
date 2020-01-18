
CREATE TABLE dbo.SourceTable
	(
	MyID int NOT NULL IDENTITY (1, 1),
	ColumnValue varchar(50) NULL,
	CreatedDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.SourceTable ADD CONSTRAINT
	DF_SourceTable_CreatedDate DEFAULT getdate() FOR CreatedDate
GO
ALTER TABLE dbo.SourceTable ADD CONSTRAINT
	PK_SourceTable PRIMARY KEY CLUSTERED 
	(
	MyID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
CREATE TABLE dbo.DestTable
	(
	MyID int NOT NULL,
	ColumnValue varchar(50) NULL,
	CreatedDate datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.DestTable ADD CONSTRAINT
	DF_DestTable_CreatedDate DEFAULT getdate() FOR CreatedDate
GO
ALTER TABLE dbo.DestTable ADD CONSTRAINT
	PK_DestTable PRIMARY KEY CLUSTERED 
	(
	MyID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO


CREATE TABLE dbo.ControlTable
	(
	SourceTable varchar(50) NOT NULL,
	LastLoadID int NOT NULL,
	LastLoadDate datetime NOT NULL,
	RowsInserted int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ControlTable ADD CONSTRAINT
	DF_ControlTable_LastLoadDate DEFAULT 1/1/1900 FOR LastLoadDate
GO
ALTER TABLE dbo.ControlTable ADD CONSTRAINT
	DF_ControlTable_RowsInserted DEFAULT 0 FOR RowsInserted
GO
ALTER TABLE dbo.ControlTable ADD CONSTRAINT
	PK_ControlTable PRIMARY KEY CLUSTERED 
	(
	SourceTable
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

INSERT INTO ControlTable
(SourceTable, LastLoadID)
Values('SourceTable', -1)
GO
INSERT INTO SourceTable (ColumnValue)
VALUES 
('A'), ('B'), ('C')








