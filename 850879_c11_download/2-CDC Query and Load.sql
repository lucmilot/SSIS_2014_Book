--Make an update to the source table


UPDATE HumanResources.Employee
SET HireDate = DATEADD(day, 1, HireDate)
WHERE [BusinessEntityID] IN (1, 2, 3);


GO


--Let's check for all changes since the same time yesterday
DECLARE @begin_time AS DATETIME = GETDATE() - 1;
--Let's check for changes up to right now
DECLARE @end_time AS DATETIME = GETDATE();
--Map the time intervals to a CDC query range (using LSNs)
DECLARE @from_lsn AS BINARY(10)
= sys.fn_cdc_map_time_to_lsn('smallest greater than or equal', @begin_time);
DECLARE @to_lsn AS BINARY(10)
= sys.fn_cdc_map_time_to_lsn('largest less than or equal', @end_time);
--Validate @from_lsn using the minimum LSN available in the capture instance
DECLARE @min_lsn AS BINARY(10)
= sys.fn_cdc_get_min_lsn('HumanResources_Employee');
IF @from_lsn < @min_lsn SET @from_lsn = @min_lsn;
--Return the NET changes that occurred within the specified time
SELECT * FROM
cdc.fn_cdc_get_net_changes_HumanResources_Employee(@from_lsn, @to_lsn,
N'all with mask');
GO


--First update another column besides the HireDate so you can
--test the difference in behavior
UPDATE HumanResources.Employee
SET VacationHours = VacationHours + 1
WHERE BusinessEntityID IN (3, 4, 5);
WAITFOR DELAY '00:00:05'; --Wait 10s to let the log reader catch up
--Map times to LSNs as you did previously
DECLARE @begin_time AS DATETIME = GETDATE() - 1;
DECLARE @end_time AS DATETIME = GETDATE();
DECLARE @from_lsn AS BINARY(10)
= sys.fn_cdc_map_time_to_lsn('smallest greater than or equal', @begin_time);
DECLARE @to_lsn AS BINARY(10)
= sys.fn_cdc_map_time_to_lsn('largest less than or equal', @end_time);
DECLARE @min_lsn AS BINARY(10)
= sys.fn_cdc_get_min_lsn('HumanResources_Employee');
IF @from_lsn <@min_lsn SET @from_lsn = @min_lsn;
--Get the ordinal position(s) of the column(s) you want to track
DECLARE @hiredate_ord INT
= sys.fn_cdc_get_column_ordinal(N'HumanResources_Employee', N'HireDate');
DECLARE @vac_hr_ord INT
= sys.fn_cdc_get_column_ordinal(N'HumanResources_Employee', N'VacationHours');
--Return ALL the changes and a flag to tell us if the HireDate changed
SELECT
BusinessEntityID,
--Boolean value to indicate whether hire date was changed
sys.fn_cdc_is_bit_set(@hiredate_ord, __$update_mask) AS [HireDateChg],
--Boolean value to indicate whether vacation hours was changed in the source
sys.fn_cdc_is_bit_set(@vac_hr_ord, __$update_mask) AS [VacHoursChg]
FROM cdc.fn_cdc_get_all_changes_HumanResources_Employee(@from_lsn, @to_lsn, 
N'all');


