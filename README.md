# Rick and Morty

Rick and Morty is a solution for loading the characters and exposing them through an API

## Database setup

1. Setup a MySql database
2. Configure the ConnectionString property in RickAndMorty.DataAccess.CharacterContextFactory to your MySql database
3. Use the package manager console, select RickAndMorty.DataAccess (if it does not work set RickAndMorty.DataAccess as startup project)

```package manager console
Update-Database
```

## Usage of ConsoleApp

1. Update the default connectionString in appsettings.json to the MySql database. 
2. Set RickAndMorty.ConsoleApp as the startup project.
3. Run the project.

## Usage of WebApi

1. Update the default connectionString in appsettings.json to the MySql database. 
2. Set RickAndMorty.WebApi as the startup project.
3. Run the project.

## Usage of UnitTests
1. Run the project.