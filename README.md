# 🧠 Personal Data Pipeline

A personal platform to automate ingestion, processing, storage, and visualization of structured personal data — starting with expense data exported from the "Money Manager" Android app.

This project serves as a hands-on practice with modern .NET technologies, Azure Functions, Docker, and cloud-native design patterns.

---

## 📦 Solution Structure

| Project                    | Description                                         |
|---------------------------|-----------------------------------------------------|
| `App.WebApp`              | ASP.NET Core MVC for dashboard and visualizations |
| `App.WebApi`              | Minimal API to expose REST endpoints |
| `App.Functions`           | Azure Functions for triggering, background jobs, processing data, etc |
| `App.Domain`              | Core domain models, interfaces, and business logic                   |
| `App.Infrastructure`      | Implementation of data access and integration (SQL, Storage, API, etc)    |
| `App.Tests`               | Unit and integration tests using xUnit             |
| `App.Shared`              | Common utilities and value objects      |
| `App.WebApp.IntegrationTests`  |   WebApp IntegrationTests    |
| `App.WebApi.IntegrationTests`  |   WebApi IntegrationTests    |
| `App.Functions.IntegrationTests`  |   Az Functions IntegrationTests    |
| `App.Core.UnitTests`  |   Core UnitTests    |
| `App.Infrastructure.UnitTests`  |   Domain UnitTests    |

> _Note: `Money Manager` is implemented as one of the features in this platform._

---

## ⚙️ Planned Architecture

1. **Excel Export** from Money Manager app
2. **Blob-trigger Azure Function** picks up Excel
3. Store **raw transactions** in Azure Table Storage
4. Queue a message for async processing
5. **Queue-trigger Azure Function** validates & transforms data
6. Save cleaned data to **SQL Server**
7. Sync results to **Google Sheets**
8. Visualize in **Google Looker Studio**

---

## 🧪 Testing Strategy

- **Unit Tests**: xUnit + Shouldly
- **Integration Tests**: [TestContainers](https://github.com/testcontainers/testcontainers-dotnet)
- **Coverage Reporting**: Coverlet + ReportGenerator + SonarQube

---

## 📊 Quality Gate (via SonarQube)

- Code coverage target: `80%+`
- Technical debt ratio and code smells tracked
- Static analysis integrated in CI (planned)

---

## 🚀 Long-Term Goals

- ASP.NET Core MVC dashboard for visualization
- Docker support for local dev and deployment
- CI/CD with GitHub Actions
- Hosting on Azure (Functions + App Service + SQL + Storage)

---

## 🗂 Roadmap

- [ ] Epic: Core Data Pipeline Foundation
- [ ] Epic: Feature - Money Manager Data Integration
- [ ] Epic: Set Up Codebase, Tests, SonarQube
- [ ] Epic: ASP.NET Core MVC Dashboard (Future)

---

