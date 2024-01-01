# Park Attractions

### This application allows you to do CRUD over Park attractions as admin and buy tickets as a user.

## Setup
---
Navigate to the file **config.json** in `Lab06.MVC\Lab06.DAL` folder and enter your server name, and databse name to connection string:
```json
"DefaultConnection": "Server=name;Database=name;Trusted_Connection=True;MultipleActiveResultSets=true"
```

## Launch
---
Locally: <br> Navigate to the `Lab06.MVC\Lab06.MVC` folder and type:
```
dotnet run
```
Go to the browser to the localhost specified in the console.

Or just simply build and run using visual studio

Seeded users are:
```
Email: parkadmin2021@gmail.com    Password: Pass+w0rd   (Admin)
Email: romasavin2021@gmail.com    Password: Pass+w0rd   (Employee)
```

After registration, the new user is assigned the "user" role