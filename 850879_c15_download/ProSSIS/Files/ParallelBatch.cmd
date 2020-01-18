@Echo off
sqlcmd -E -S localhost -d AdventureWorksDW -Q "update dbo.ctlTaskQueue set StartDate = NULL, CompleteDate = NULL"
FOR /L %%i IN (1, 1, %1) DO (
ECHO Spawning thread %%i
START "Worker%%i" /Min "C:\Program Files\Microsoft SQL Server\120\DTS\Binn\DTEXEC.exe" /FILE "C:\ProSSIS\Code\Ch15\09_ParallelDemo.dtsx" /CHECKPOINTING OFF /REPORTING E
)