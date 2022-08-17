In Package Manager Console:

Scaffold-DbContext -Connection "Server=.;User Id=reaccess;Password=reaccess123;Database=REACCESS_DEV" -Provider "Microsoft.EntityFrameworkCore.SqlServer" -OutputDir "Models" -f -Context "DatabaseContext" -Project "REAccess.Mobile.Database"

Scaffold-DbContext -Connection "Server=.;User Id=reaccess;Password=reaccess123;Database=REACCESS_LOG" -Provider "Microsoft.EntityFrameworkCore.SqlServer" -OutputDir "LogModels" -f -Context "LogDatabaseContext" -Project "REAccess.Mobile.Database"

