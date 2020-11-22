**Migration**

0- cd Services.Identity

1- *dotnet ef migrations add "initial" -o ./Data/Migrations*

2- *dotnet ef database update*