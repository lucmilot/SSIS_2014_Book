CREATE TABLE [dbo].[SSIS_SETTING] (
[PACKAGE_ID] [uniqueidentifier] NOT NULL,
[SETTING] [nvarchar](2080) NOT NULL,
[VALUE] [nvarchar](2080) NOT NULL
) ON [PRIMARY]
GO

INSERT INTO SSIS_SETTING
SELECT '{INSERT YOUR PACKAGE ID HERE}', 'LOGFILEPATH', 'c:\myLogFile.txt'

CREATE TABLE [dbo].[Contacts](
[ContactID] [int] IDENTITY(1,1) NOT NULL,
[FirstName] [varchar](50) NOT NULL,
[LastName] [varchar](50) NOT NULL,
[City] [varchar](25) NOT NULL,
[State] [varchar](15) NOT NULL,
[Zip] [char](11) NULL
) ON [PRIMARY]

CREATE TABLE dbo.ContactsErrorQueue
(
ContactErrorID int NOT NULL IDENTITY (1, 1),
FirstName varchar(50) NULL,
LastName varchar(50) NULL,
City varchar(50) NULL,
State varchar(50) NULL,
Zip varchar(50) NULL,
RejectReason varchar(50) NULL
) ON [PRIMARY]
