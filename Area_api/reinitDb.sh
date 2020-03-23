rm -rf Migrations ;
dotnet ef database drop --force --context ApplicationDbContext;
dotnet ef migrations add Init --context ApplicationDbContext;
dotnet ef database update --context ApplicationDbContext;
