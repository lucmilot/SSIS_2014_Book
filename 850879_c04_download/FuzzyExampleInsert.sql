CREATE TABLE [Occupation](
[OccupationID] [smallint] IDENTITY(1,1) NOT NULL,
[OccupationLabel] [varchar] (50) NOT NULL
CONSTRAINT [PK_Occupation_OccupationID] PRIMARY KEY CLUSTERED
(
[OccupationID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT INTO [Occupation] Select 'EXEC VICE PRES'
INSERT INTO [Occupation] Select 'FIELDS OPS MGR'
INSERT INTO [Occupation] Select 'BUS OFFICE MGR'
INSERT INTO [Occupation] Select 'X-RAY TECH'
