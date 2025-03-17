# Sales Management Application

This repository contains an MVC-based Sales Management Application built with C#. The application is developed using Onion Architecture for a robust and maintainable code structure.

## 📌 Project Features

- Sales Management
- Customer Management
- Product Management
- Reporting and Analysis

## 🛠 Technologies Used

| Technology | Description |
|------------|-------------|
| **.NET 7.0** | Main development platform |
| **ASP.NET Core MVC** | Web application framework |
| **Entity Framework Core** | ORM layer |
| **Identity Framework** | User management |
| **Bootstrap 5** | Frontend design |
| **Toastr.js** | Notification management |
| **MSSQL** | Database management |

## 📁 Project Folder Structure

```plaintext
SalesManagementApp
│── SalesManagementApp.Application
│   ├── DTOs
│   ├── Extensions
│   ├── Services
│── SalesManagementApp.Domain
│   ├── Core
│   ├── Entities
│   ├── Enums
│   ├── Utilities
│── SalesManagementApp.Infrastructure
│   ├── AppContext
│   ├── Configurations
│   ├── DataAccess
│   ├── Migrations
│   ├── Repositories
│── SalesManagementApp.UI
│   ├── Controllers
│   ├── Models
│   ├── Views
│   ├── wwwroot
│   ├── Areas
│── README.md
```

## 📊 Database Schema

This project is a management system that includes **sales management**, **customer management**, **product management**, and **reporting**. Below is the **database schema and relationships** detailed.

## 📌 **Table Structures**

### 🔹 **User and Authorization**
| **Table** | **Description** |
|-----------|-----------------|
| `AspNetUsers` | Stores user information (Admin, User) |
| `AspNetRoles` | Contains user roles (Admin, User) |
| `AspNetUserRoles` | Manages the **many-to-many** relationship between users and roles |
| `AspNetUserClaims` | Stores special permission claims for users |
| `AspNetUserTokens` | Stores user login tokens |

### 📝 **Sales Management**
| **Table** | **Description** |
|-----------|-----------------|
| `Sales` | Contains sales information (Product, Quantity, Date, Customer) |
| `Customers` | Contains customer information (First Name, Last Name, Email) |
| `Products` | Contains product information (Name, Price, Stock) |
| `SalesProducts` | Manages the **many-to-many** relationship between sales and products |

### 🏷 **Product Management**
| **Table** | **Description** |
|-----------|-----------------|
| `Products` | Contains product information (Name, Price, Stock) |
| `Categories` | Contains product categories |
| `ProductCategories` | Manages the **many-to-many** relationship between products and categories |

## 🔗 **Database Relationships**
- **`AspNetUsers` → `Sales`** → (1-N) **One user can make multiple sales.**
- **`Sales` → `Customers`** → (N-1) **One sale can have only one customer.**
- **`Sales` → `Products`** → (N-N) **One sale can have multiple products.**
- **`Products` → `Categories`** → (N-N) **One product can belong to multiple categories.**

## Requirements

- **.NET 7.0** or newer
- **SQL Server** or **SQL Server Express**
- **Visual Studio** or **Visual Studio Code** (optional)

## Getting Started

### 1. Clone the Project

First, clone the project from GitHub to your computer:

```bash
git clone https://github.com/mykece/SalesManagementApp.git
```

### 2. Edit the Connection String

Edit the contents of the `appsettings.json` file as follows:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=SalesManagementApp;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Apply Database Migrations

Before running the project for the first time, you need to apply database migrations to create the database structure. Follow these steps:

a. Open the Package Manager Console (Visual Studio)

Open it from Tools > NuGet Package Manager > Package Manager Console.

b. Apply the Migrations

Run the following commands to apply the migrations:

```bash
Add-Migration InitialCreate
Update-Database
```

### 4. User Roles

In the project, roles like `Admin` and `User` are added to the `AspNetRoles` table. You need to add these roles manually. Use a tool like SQL Server Management Studio (SSMS) to run the following SQL commands:

```sql
INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES (NEWID(), 'Admin', 'ADMIN');
INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES (NEWID(), 'User', 'USER');
```

## Configuring the Mail Service

The project uses a mail service to send emails through the `MailService` class. You need a **Gmail account** to send emails via SMTP.

### 1. Create a Gmail Account

To use the mail service, you need a Gmail account. The project will send emails using this account. If you don't have a Gmail account, create one [here](https://accounts.google.com/signup).

### 2. Create an App Password in Gmail

For secure email sending, create an app password in your Gmail account. Sign in to your Gmail account and follow these steps:

1. [Sign In to Your Google Account](https://myaccount.google.com/).
2. Go to the "Security" tab.
3. Click on the "App passwords" section.
4. If 2-step verification (2FA) is enabled, create a new password here.
5. Create an "App password" and use it for SMTP authentication in the `MailService` class.

### 3. Configure the `MailService`

The `MailService` `ConnectionStrings` class in the project works as follows. Ensure you fill in the parameters correctly to send emails:

```csharp
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Recaptcha": {
    "SiteKey": "6Ldr0RgqAAAAAAomGEztuRDZE2CnWvElG0j1jodazr",
    "SecretKey": "6Ldr0RgqAAAAANpjrvGRx7GNfIt3-42Qv9IXrrzgzr"
  },
  "ConnectionStrings": {
    "ProjectConnectionString": "Server=DESKTOP-15U9NLA\\SQLEXPRESS;Database=ExampApp;Trusted_Connection=True;TrustServerCertificate=True;",
    "HangfireConnectionString": "Server=DESKTOP-15U9NLA\\SQLEXPRESS;Database=ExampApp;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "EmailConfiguration": {
    "From": "marka.musayw@gmail.com",
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "marka.musayw@gmail.com",
    "Password": "awdawdawdawdg"
  },

  "AllowedHosts": "*"
}

```

### 4. Start the Application

After configuring everything related to the project, start the application by running the following command:

```bash
dotnet run
```

## Contributing

If you want to contribute, please start by opening an issue to discuss what you would like to work on. Pull requests are welcome.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Contact

For any questions or feedback, please contact [mykece](https://github.com/mykece).
