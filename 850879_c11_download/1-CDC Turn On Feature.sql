
EXEC sp_changedbowner 'sa'
GO

--Enable CDC on the database
EXEC sys.sp_cdc_enable_db;
GO
--Check CDC is enabled on the database
SELECT name, is_cdc_enabled
FROM sys.databases WHERE database_id = DB_ID();
GO


--Enable CDC on a specific table
EXECUTE sys.sp_cdc_enable_table
@source_schema = N'HumanResources'
,@source_name = N'Employee'
,@role_name = N'cdc_Admin'
,@capture_instance = N'HumanResources_Employee'
,@supports_net_changes = 1;

GO

--Check CDC is enabled on the table
SELECT [name], is_tracked_by_cdc FROM sys.tables
WHERE [object_id] = OBJECT_ID(N'HumanResources.Employee');
--Alternatively, use the built-in CDC help procedure
EXECUTE sys.sp_cdc_help_change_data_capture
@source_schema = N'HumanResources',
@source_name = N'Employee';
GO

