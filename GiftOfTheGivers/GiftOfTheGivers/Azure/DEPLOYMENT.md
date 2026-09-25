# Azure App Service deployment

1. Create an Azure SQL Database and App Service.
2. Add the Azure SQL connection string in App Service > Environment variables:
   - `DatabaseProvider` = `SqlServer`
   - `ConnectionStrings__AzureSqlConnection` = your SQL connection string
3. Publish:
   `dotnet publish -c Release -o ./publish`
4. Deploy the published output to Azure App Service.
5. On first start, the application uses `EnsureCreated` for the prototype database.
6. For the full production release, replace `EnsureCreated` with EF Core migrations and run:
   `dotnet ef migrations add InitialCreate`
   `dotnet ef database update`

Never commit real Azure SQL credentials to source control.
