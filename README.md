# AcmeCorpAPI

There's just a few things that need to be done to get the API up and running!


## Add local connection string and API Key

Open appsettings.development.json and add the DefaultConnection and ApiKey values.
> ApiKey can be any random value

## Migrations
Using a cli, cd into the solution root and run the following command:
> dotnet ef database update --project AcmeCorp.Infrastructure --startup-project AcmeCorp.Api
