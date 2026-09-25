# Gift of the Givers Prototype — Section C

## Stack
- ASP.NET Core MVC / .NET 10
- ASP.NET Identity with Employee and Donor roles
- Entity Framework Core 10
- SQLite for a portable local prototype
- SQL Server/Azure SQL-ready configuration
- Bootstrap responsive UI

## Run locally
From the folder containing `GiftOfTheGivers.csproj`:

```powershell
dotnet restore
dotnet build
dotnet run
```

The application automatically creates `giftgivers.db` on first run.

## Employee demo account
Email: `employee@giftofthegivers.org`
Password: `Employee123!`

The account is seeded into the Employee role.

## Features
- Home, About, Relief Projects, Donate, Volunteer and Contact
- ASP.NET Identity login/register
- Employee and Donor roles
- One-time/recurring symbolic donations
- ZAR/USD/EUR selection
- Anonymous donations
- On-screen prototype tax certificate with Print / Save as PDF
- Volunteer registration
- Employee dashboard
- Employee relief project creation and project updates
- Database indexes for common operational queries

## Azure SQL
The local prototype defaults to SQLite so it runs without requiring SQL Server or LocalDB.

For Azure SQL deployment, set:

`DatabaseProvider=SqlServer`

and provide:

`ConnectionStrings__AzureSqlConnection=<Azure SQL connection string>`

Then deploy the published application to Azure App Service.
