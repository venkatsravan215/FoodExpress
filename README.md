# FoodExpress – Full-Stack Food Ordering Application

FoodExpress is a full-stack food ordering application built using ASP.NET Core Web API, React, and SQL Server. It provides RESTful APIs for managing users, restaurants, menu categories, menu items, and orders.

## 🚀 Technologies Used

- **Backend:** C#, ASP.NET Core Web API, .NET 8
- **Frontend:** React.js, JavaScript, Vite
- **Database:** Microsoft SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** JWT-based authentication
- **API Testing:** Swagger / OpenAPI
- **Development Tools:** Visual Studio, Visual Studio Code, Git, GitHub

## ✨ Key Features

- User registration and authentication
- Restaurant management
- Menu category and menu item management
- Food ordering and order item management
- RESTful API development
- SQL Server database integration
- React-based user interface
- Shopping cart with quantity persistence
- API documentation and testing through Swagger

## 🏗️ Project Structure

```text
FoodExpress/
├── FoodExpress.API/
├── FoodExpress.Client/
├── FoodExpress.Core/
├── FoodExpress.Infrastructure/
├── FoodExpress.Tests/
└── Food Express.sln
```

## ⚙️ Getting Started

### Prerequisites

- .NET 8 SDK
- Node.js and npm
- Microsoft SQL Server or SQL Server LocalDB
- Visual Studio or Visual Studio Code

### 1. Clone the repository

```bash
git clone https://github.com/venkatsravan215/FoodExpress.git
cd FoodExpress
```

### 2. Configure the backend

Configure your local database connection string and JWT settings using .NET User Secrets or local configuration files. Do not commit credentials or secret keys to GitHub.

Restore and build the API:

```bash
dotnet restore Food Express.sln
dotnet build Food Express.sln
```

### 3. Run the backend

Open the solution in Visual Studio, set `FoodExpress.API` as the startup project, and run the application.

Open the Swagger URL shown in the terminal or browser to explore the API endpoints.

### 4. Run the frontend

Open a terminal in the client directory:

```bash
cd FoodExpress.Client
npm install
npm run dev
```

Open the local URL displayed by Vite in your browser.

## 🎯 Project Objective

The objective of FoodExpress is to demonstrate full-stack application development using ASP.NET Core Web API, React, and SQL Server, including REST API design, database integration, authentication, and frontend-backend communication.

## 👨‍💻 Developer

**Venkat Sravan Payyavala**

GitHub: [venkatsravan215](https://github.com/venkatsravan215)

Project Repository: [FoodExpress](https://github.com/venkatsravan215/FoodExpress)
