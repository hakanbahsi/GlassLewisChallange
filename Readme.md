# GlassLewis Full Stack .NET Code Challenge

## 📄 Project Description

This repository contains a full stack web application built for the Glass Lewis Code Challenge. It features a secure and testable Web API for managing companies and a simple user interface for interaction. The system is built using .NET 8, follows best practices (CQRS, Clean Architecture), and is fully containerized.

## ⚙️ Technologies Used

* .NET 8
* ASP.NET Core Web API
* Razor Pages (UI)
* Entity Framework Core (Code-First)
* MediatR (CQRS)
* FluentValidation
* AutoMapper
* SQL Server (Dockerized)
* xUnit, FluentAssertions (Unit Testing)
* Docker & Docker Compose

## 🚀 Getting Started

### Prerequisites

* Docker & Docker Compose
* (Optional) .NET 8 SDK for local development

### Clone the Repository

```bash
git clone https://github.com/<your-username>/GlassLewisChallange.git
cd GlassLewisChallange
```

### Run with Docker

```bash
docker-compose up --build
```

## 🌐 Application URLs

| Layer    | URL                                                                                  |
| -------- | ------------------------------------------------------------------------------------ |
| API      | [http://localhost:5139/swagger/index.html](http://localhost:5139/swagger/index.html) |
| UI       | [http://localhost:8080](http://localhost:8080)                                       |
| Database | localhost:1433 (sa / HakanBahsi\_123)                                                |

## 🔐 Authentication

All API endpoints require an API key:

```
X-API-KEY: secret-key-123
```

This key must be sent in request headers.

## 🔄 Available Features

| Endpoint                       | Description                      |
| ------------------------------ | -------------------------------- |
| `POST /api/company`            | Create a new company             |
| `GET /api/company/{id}`        | Retrieve company by encrypted ID |
| `GET /api/company/isin/{isin}` | Retrieve company by ISIN         |
| `GET /api/company`             | Retrieve all companies           |
| `PUT /api/company`             | Update a company                 |

## 📊 Data Validation

* ISIN must start with two letters
* URL must be a valid URI
* All required fields are enforced using FluentValidation

## 🚪 Security

* API Key Authorization Middleware
* Encrypted and URL-safe company IDs (Base64Url)

## 📅 Database

* SQL Server is run as a container
* EF Core Migrations applied automatically at startup
* Alternatively, use the `init.sql` file

## 🔬 Running Tests

```bash
cd GlassLewisChallange.Tests
dotnet test
```

## ✨ Highlights

* Robust architecture (API / Application / Domain / Persistance / Infrastructure)
* Centralized exception handling
* DTO and entity separation
* AutoMapper + MediatR integration
* Validation, error handling, logging
* Docker-based local environment

## 👀 Preview

| View                | Example URL                                                    |
| ------------------- | -------------------------------------------------------------- |
| Swagger UI          | [http://localhost:5139/swagger](http://localhost:5139/swagger) |
| Web UI (Company UI) | [http://localhost:8080](http://localhost:8080)                 |

## 🙌 Author

Developed by Hakan Bahsi for Glass Lewis Code Challenge.
