# ✅ ToDo API

REST API for task management (**ToDo**), built with **ASP.NET Core**, following **Clean Architecture** principles, with a strong focus on **best practices**, **security**, and **maintainability**.

This project is **educational and portfolio-oriented**, simulating a real-world production application.

---

## 🎯 Project Goals

- Consolidate knowledge in **ASP.NET Core**
- Build a strong portfolio project
- Apply **Clean Architecture**
- Implement **authentication and authorization** using JWT
- Use **MediatR** for decoupling
- Practice **REST API best practices**
- Prepare a solid foundation for automated testing and scalability

---

## 🏗️ Architecture

The project follows **Clean Architecture**, separating responsibilities into well-defined layers:

```
src/
 ├── ToDo.Api              → Controllers, Middlewares, Auth
 ├── ToDo.Application      → Use Cases, DTOs, Commands, Queries
 ├── ToDo.Domain           → Entities, Enums, Business Rules
 └── ToDo.Infrastructure  → EF Core, Repositories, Persistence
```

### Core concepts applied

- Separation of concerns
- Dependency direction pointing to the domain
- Low coupling
- High testability

---

## 🔐 Authentication & Authorization

- Authentication via **JWT (JSON Web Token)**
- Role-based access control
    - `Admin`
    - `User`
- Endpoint protection using `[Authorize]`
- Access restrictions by role
- Properly mapped claims (`NameIdentifier`, `Role`)

---

## 🔗 Main Endpoints

### 👤 Authentication

- `POST /api/auth/login`
- `POST /api/auth/register`

### 📋 Users

- `GET /api/users` → Admin
- `PUT /api/users/{id}` → Admin
- `DELETE /api/users/{id}` → Admin
- `PUT /api/users` → Authenticated user
- `DELETE /api/users` → Authenticated user

### ✅ Tasks (ToDo)

- `POST /api/todos`
- `GET /api/todos`
- `PUT /api/todos/{id}`
- `DELETE /api/todos/{id}`

*(Endpoints may vary depending on the implementation)*

---

## 🛠️ Technologies Used

- ASP.NET Core Web API
- C#
- Entity Framework Core
- MediatR
- JWT Bearer Authentication
- Swagger (OpenAPI)
- Clean Architecture

---

## 🧪 Testing via Swagger

The API is integrated with **Swagger**, allowing:

- JWT authentication
- Testing protected endpoints
- Clear visualization of API contracts

---

## ▶️ How to Run the Project

### Prerequisites

- .NET SDK 10+
- PostgreSQL
- Visual Studio / VS Code

### Steps

```
git clone https://github.com/your-username/todo-api.git
cd todo-api
dotnet restore
dotnet run
```

---

## 🐳 Running with Docker Compose

The project can also be run using **Docker Compose**, simplifying environment setup and eliminating the need to install dependencies locally (such as the database).

### 🔧 Prerequisites

- Docker
- Docker Compose

---

### ▶️ How to Run

1. From the project root, run:

```
docker-compose up --build
```

1. Wait for the containers to start
2. Access the API at:

```
http://localhost:8080/swagger
```

---

### 🧩 What Docker Compose Runs

- ASP.NET Core API
- Database (PostgreSQL)
- Internal network for service communication

---

## ⚙️ `.env` File Configuration

The project uses a **`.env`** file to store **environment variables**, preventing sensitive data from being committed to source control.

This file is consumed by **Docker Compose** during container startup.

---

### 📄 `.env` Example

```
# Database
DB_HOST=db
DB_PORT=5432
DB_NAME=ToDo
DB_USER=postgres
DB_PASSWORD=postgres

# JWT
JWT_KEY=dev_jwt_key_example_1234567890
```

---

### 🔐 JWT Settings

- `JWT__KEY` → Key used to sign the JWT

These variable are automatically read by ASP.NET Core via `IConfiguration`.

---

### 🗄️ Database

Database variables are used in `docker-compose.yml` to:

- Configure the database container
- Build the application **connection string**

Generated connection string example:

```
DatabaseConnection=Host=${DB_HOST};Port=${DB_PORT};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD}
```

---

### 🧠 Naming Convention Used

The `JWT__KEY` pattern (with **double underscore**) is an ASP.NET Core convention for mapping hierarchical configuration, equivalent to:

```
{
  "JWT": {
    "KEY": "super-secret-key-change-me"
  }
}
```

---

### ▶️ Execution Flow with `.env`

1. Docker Compose loads the `.env` file
2. Environment variables are injected into the containers
3. ASP.NET Core automatically reads the variables
4. The application starts fully configured

---

## 📌 Best Practices Adopted

- DTOs for external communication
- Validations in the Application layer
- Thin controllers
- Business rules isolated in the Domain layer
- Role-based security
- Clean, organized, and readable code

---

## 👨‍💻 Author

**Gustavo Soares**

Junior .NET Developer focused on backend development, REST APIs, and software architecture best practices.

### 🌐 Portfolio

https://sirsoares33.github.io/gustavosoares.github.io/index.html
