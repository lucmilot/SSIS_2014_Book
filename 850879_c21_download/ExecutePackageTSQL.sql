USE [SSISDB]
GO

DECLARE @RC int
DECLARE @folder_name nvarchar(128)
DECLARE @project_name nvarchar(128)
DECLARE @package_name nvarchar(260)
DECLARE @reference_id bigint
DECLARE @use32bitruntime bit
DECLARE @execution_id bigint

-- TODO: Set parameter values here.
set @folder_name = 'SSISInfoPath'
set @project_name = 'SSISInfoPath'
set @package_name = 'Package1.dtsx'
set @use32bitruntime = 0

EXECUTE @RC = [catalog].[create_execution] 
   @folder_name
  ,@project_name
  ,@package_name
  ,@reference_id
  ,@use32bitruntime
  ,@execution_id OUTPUT

 
EXECUTE @RC = [catalog].[set_execution_parameter_value] 
   @execution_id
  ,30 --
  ,N'MyParameter'
  ,N'Test1' -- verbose logging, 0 (none), 1 (basic), 2 (performance), 3, verbose

EXECUTE @RC = [catalog].[set_execution_parameter_value] 
   @execution_id
  ,50 --
  ,N'LOGGING_LEVEL'
  ,3 -- verbose logging, 0 (none), 1 (basic), 2 (performance), 3, verbose

EXECUTE @RC = [catalog].[set_execution_parameter_value] 
   @execution_id
  ,50 --
  ,N'SYNCHRONIZED'
  ,1
 
 
 
 

EXECUTE @RC = [catalog].[start_execution] 
   @execution_id

print @execution_id

-- add the delay show the logs can be written
--waitfor delay '00:00:05'

select om.* from catalog.executions e inner join
catalog.operations o on e.execution_id = o.operation_id
inner join catalog.operation_messages om on o.operation_id = om.operation_id
where om.operation_id = @execution_id





