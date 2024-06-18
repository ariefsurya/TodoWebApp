# TodoWebApp

**Nuget Packages:
*TodoWebApp:
	Newtonsoft.Json (latest)
	CurrieTechnologies.Razor.SweetAlert2 (5.6.0)
	Blazor.Bootstrap (latest)
*TodosApi
	Newtonsoft.Json (latest)
	Microsoft.EntityFrameworkCore (7.0.20)
	Microsoft.EntityFrameworkCore.Design (7.0.20)
	Microsoft.EntityFrameworkCore.Sqlite (7.0.20)
	Microsoft.EntityFrameworkCore.Tools (7.0.20)



**database migrations TodosApi:
PM> dotnet ef migrations add Initial
PM> dotnet ef database update

PM> Add-Migration Initial-2
PM> Update-Database

*tools for database migrations TodosApi if needed:
dotnet tool install --global dotnet-ef
dotnet new tool-manifest # if you don't have a manifest file yet
dotnet tool install dotnet-ef


**TodosApiTest:
$ dotnet add package Moq
$ dotnet add package xUnit
$ dotnet add package xUnit.runner.visualstudio



**Deployment:
docker-compose build --no-cache
docker-compose down
docker-compose up -d


go to login page:
https://localhost:9005/login


API Documentation:
https://localhost:9000/swagger/index.html


