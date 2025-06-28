#  cShipment: Transport Logistics Management System

**cShipment** is a robust and scalable web application built with **ASP.NET Core MVC** and **Entity Framework Core**, designed to streamline logistics operations. It provides comprehensive **CRUD functionality** for managing **shipments, trucks, and drivers**, offering intelligent relational views and assignment workflows to optimize transport efficiency.

Developed as a passion project for HTTP5226 by **Camoy Phillips**, cShipment emphasizes best practices, a scalable service architecture, and a clean user interface.

---

## Core Features

cShipment provides a complete suite of tools for managing your logistics operations, from basic record keeping to complex assignments.

### Shipment & Resource Management (CRUD)

- **Shipments**: Create, view, update, and delete shipment records, including details like origin, destination, distance, and current status.
- **Trucks**: Track truck model, mileage, and last maintenance date. Supports **image uploads** for visual identification.
- **Drivers**: Manage driver profiles, license information, and truck assignment.

### Intelligent Relational Workflows

- **Driver Assignments**: Assign multiple drivers to a single shipment, each with a role (e.g., "Lead", "Backup").
- **Integrated Views**: View assigned truck and driver details directly inside shipment and truck details pages.
- **Cost Planning**: Structure in place to calculate shipment costs based on distance and fuel logic.

### Robust & User-Friendly Design

- **Validation & Feedback**: Clean Razor forms with full validation via `ModelState`. Success/error messages are shown using `TempData`.
- **Relational ViewModels**: Custom ViewModels (`ShipmentDetails`, `TruckDetails`) combine DTOs with assigned entities for better frontend rendering.
- **Graceful Fallbacks**: Invalid or missing IDs are caught and redirected to safe views with error feedback.

---

## Tech Stack

- **Frontend**: Razor Pages (ASP.NET Core MVC)
- **Backend**: ASP.NET Core Web API
- **ORM**: Entity Framework Core (Code-First Migrations)
- **Database**: SQL Server
- **Languages**: C#, HTML, Bootstrap, LINQ
- **Hosting**: Kestrel / IIS Express
- **Tools**: Visual Studio 2022, SSMS

---

## RESTful API Endpoints

| Method  | Route                                | Description                              |
|---------|--------------------------------------|------------------------------------------|
| GET     | `/api/drivers`                       | List all drivers                         |
| GET     | `/api/trucks`                        | List all trucks                          |
| GET     | `/api/shipments`                     | List all shipments                       |
| GET     | `/api/shipments/driver/{id}`         | List shipments for a specific driver     |
| POST    | `/api/drivers`                       | Create a new driver                      |
| POST    | `/api/trucks`                        | Create a new truck                       |
| POST    | `/api/shipments`                     | Create a new shipment                    |
| POST    | `/api/assignments`                   | Assign a driver to a shipment            |
| PUT     | `/api/drivers/{id}`                  | Update driver                            |
| PUT     | `/api/trucks/{id}`                   | Update truck                             |
| PUT     | `/api/shipments/{id}`                | Update shipment                          |
| DELETE  | `/api/drivers/{id}`                  | Delete driver                            |
| DELETE  | `/api/trucks/{id}`                   | Delete truck                             |
| DELETE  | `/api/shipments/{id}`                | Delete shipment                          |

---

## Architecture & Structure


cShipment/
├── Controllers/ # MVC Page Controllers + API Controllers
├── Interfaces/ # Service contracts (e.g., IShipmentService)
├── Models/
│ ├── Entities/ # Domain models: Shipment.cs, Truck.cs, Driver.cs
│ ├── Dtos/ # Flat data models for API/View binding
│ └── ViewModels/ # Complex models for relational views (e.g., ShipmentDetails)
├── Services/ # Business logic (e.g., ShipmentService, TruckService)
├── Data/ # ApplicationDbContext + EF Migrations
├── Views/ # Razor views for CRUD + details pages
├── wwwroot/ # Static assets (CSS, JS, uploads/)
├── appsettings.json # Configuration and connection string
└── Program.cs # Entry point and DI configuration


---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or newer

### Setup Instructions

```bash
# 1. Clone the project
git clone https://github.com/camoyphillips/cShipment.git
cd cShipment

# 2. Set up your DB in appsettings.json, then run:
dotnet ef database update

# 3. Run the application
dotnet run


Visit the site at:
https://localhost:PORT/ShipmentPage/List
________________________________________
What I Learned
•	How to build async, testable service layers with dependency injection
•	How to use EF Core navigation properties and projections
•	How to manage many-to-many relationships and build relational Razor views

________________________________________
Future
•	Role-based access control (Admin, Driver, Dispatcher)
•	Mobile app (React Native or .NET MAUI)
•	Google Maps integration for shipment routes
•	PDF export for shipment summaries
•	 Proof of delivery image uploads
•	Cost calculator with fuel API integration
________________________________________
Project Status
MVP complete — all CRUD, assignment, image uploads, and view models in place.
Ready for extensions like mobile UI, delivery proof, and real-time updates.
________________________________________

