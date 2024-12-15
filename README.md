# TaskManager

criar migration
cd TaskManager.Infra
dotnet ef migrations add --startup-project ../TaskManager.API Init --context Context
dotnet ef database update --startup-project ../TaskManager.API Init --context Context
