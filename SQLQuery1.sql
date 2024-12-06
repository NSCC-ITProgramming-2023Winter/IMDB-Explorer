RESTORE FILELISTONLY
FROM DISK = 'C:\!NSCC\Fall2024\PROG2500 Windows #C\Assignments\final\IMDB_Project.bak';

RESTORE DATABASE IMDB FROM DISK = 'C:\!NSCC\Fall2024\PROG2500 Windows #C\Assignments\final\IMDB_Project.bak'
WITH MOVE 'IMDB_Project' TO 'C:\Users\Adam\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\IMDB_Project.mdf',
MOVE 'IMDB_Project_Log' TO 'C:\Users\Adam\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\IMDB_Project_Log.ldf',
RECOVERY, REPLACE , STATS = 10;