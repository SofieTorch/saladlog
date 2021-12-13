# Saladlog
---
A blog for people who like (or need) healthy food :)  
  
### Pre requisites
* ASP .NET Core 5.0
* Sql Server  
  
### How to run
To use and run this project follow the nexts steps:
1. Clone this repo
2. Inside the repo directory run the commands:
	'''
	$ cd MySaladlog
	$ mkdir AppData
	$ mkdir AppData/Articles
	$ mkdir wwwroot/images
	'''
3. Restore database(Sql Server) with the dbSaladlog.sql file
4. Go to `MySaladlog/appsetting.json` and inside "ConnectionStrings" replace the "DefaultConnection" with your
own connection string for the database restored on the last step
4. Run the project! You can open it on Visual Studio and click the "run" button or use the command line:
	'''
	$ cd MySaladlog
	$ dotnet run
	'''
  
  
**Authors: ** Andy Guzmán, Carlos Severich, Sofia Toro