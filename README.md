#  cShipment – Logistics Management System

**cShipment** is a transport logistics management web application built with **ASP.NET Core MVC** and **Entity Framework Core**. It helps logistics companies manage **shipments, trucks, and drivers** with full **CRUD functionality**, assignment workflows, and smart relational views.

---

## Tech Stack

- **Frontend**: Razor Pages (ASP.NET Core MVC)
- **Backend**: ASP.NET Core Web App
- **Database**: SQL Server (Entity Framework Core)
- **ORM**: EF Core (Code-First Migrations)
- **Languages**: C#, HTML, Bootstrap, LINQ
- **Hosting**: Local IIS Express or Kestrel
- **Tools**: Visual Studio 2022, SSMS

---

## Features

### Core CRUD Functionality

- **Shipments**: Create, View, Update, and Delete shipment records with distance, origin, and status fields.
- **Trucks**: Track truck model, mileage, last maintenance date, and optionally upload an image.
- **Drivers**: Manage driver records, license numbers, and truck assignments.

### Relational Features

- Assign **drivers to shipments** with role tags (e.g. “Lead”, “Backup”)
- Display **assigned truck and driver details** inside shipment and truck views.
- Use view models like `ShipmentDetails` and `TruckDetails` to display joined data.

### File Upload

- Admins can upload truck images during creation or update.
- Uploaded files are saved to `/wwwroot/uploads/trucks/`.

### Error Handling & Validation

- Full validation with ModelState in forms.
- Uses `TempData` to flash success/error messages.
- View fallbacks and graceful redirects for missing IDs.


### Prerequisites

- [.NET 8 SDK]
- [SQL Server or SQL Express]
- Visual Studio 2022 or newer

###  Running the Project

1. **Clone the repo**:
    ```bash
    git clone https://github.com/camoyphillips/cShipment.git
    cd cShipment
    ```

2. **Set up the database**:
    - In `appsettings.json`, set your SQL Server connection string.
    - Run migrations and seed data:
      ```bash
      dotnet ef database update
      ```

3. **Run the app**:
    ```bash
    dotnet run
    ```

4. Open in browser at:  
   `https://localhost:xxxx/ShipmentPage/List`

# cShipment

**Transport Truck Logistics Management System**

---

## Project Overview

**cShipment** is a Minimum Viable Product (MVP) for a logistics company management system designed to handle core operations including managing trucks, drivers, and shipments. The system empowers administrators to efficiently assign trucks and drivers and calculate shipment costs.

Built for HTTP5226 as a Passion Project by **Camoy Phillips**, this solution follows best practices, scalable service architecture, and clean user interface wireframes.

---

## Core Features (MVP)

### Admin Functions
- Assign drivers and trucks to shipments
- View and update shipment, truck, and driver details
- Calculate shipment costs based on distance and fuel logic

### Entities & Relationships
- **Drivers ↔ Shipments** (Many-to-Many)
- **Trucks ↔ Shipments** (One-to-Many)
- **Drivers ↔ Trucks** (One-to-One)

---

## RESTful API Endpoints

| Method | Route                          | Description                        |
|--------|--------------------------------|------------------------------------|
| GET    | `/api/drivers`                | List all drivers                   |
| GET    | `/api/trucks`                 | List all trucks                    |
| GET    | `/api/shipments`              | List all shipments                 |
| GET    | `/api/shipments/driver/{id}`  | Get shipments assigned to driver   |
| POST   | `/api/drivers`                | Add a new driver                   |
| POST   | `/api/trucks`                 | Add a new truck                    |
| POST   | `/api/shipments`              | Add a new shipment                 |
| POST   | `/api/assignments`            | Assign a driver to a shipment      |
| PUT    | `/api/drivers/{id}`           | Update driver details              |
| PUT    | `/api/trucks/{id}`            | Update truck details               |
| PUT    | `/api/shipments/{id}`         | Update shipment details            |
| DELETE | `/api/drivers/{id}`           | Delete a driver                    |
| DELETE | `/api/trucks/{id}`            | Delete a truck                     |
| DELETE | `/api/shipments/{id}`         | Delete a shipment                  |

---

## Architecture

- **Backend:** ASP.NET Core Web API  
- **Database:** SQL Server with Entity Framework Core  
- **Authentication:** ASP.NET Identity  
- **Frontend (MVP Wireframes):** Razor Views

### Service Layer
- `DriverService`, `TruckService`, `ShipmentService`, `CustomerService`, `DriverShipmentService`

Each service handles logic independently for separation of concerns and maintainability.

---

## UI Wireframes (From My Passion Project)

- **Update Shipment Form:** Assign truck & driver  
- **Update Truck Form:** View driver, and shipments  
- **Update Driver Form:** Assign to truck and view shipment history  

> Wireframes prioritize usability and reflect all core relationships from the ERD.

---

## Future Enhancements (Post-MVP)

-  Driver mobile app  
-  Google Maps integration for route tracking  
-  PDF export for logs  
-  Role-based access (admin vs driver)  
-  Fuel-price-aware shipment cost estimator  
-  Proof of delivery upload  

---

## Folder Structure 

cShipment/
│
├── Controllers/
│ ├── ShipmentPageController.cs
│ └── TruckPageController.cs
│
├── Models/
│ ├── Shipment.cs, Truck.cs, Driver.cs
│ └── Dtos/ (flattened models)
│ └── ViewModels/ (for relational views)
│
├── Views/
│ └── ShipmentPage/
│ ├── List.cshtml, Details.cshtml, Edit.cshtml, ConfirmDelete.cshtml
│
├── Services/
│ └── ShipmentService.cs, TruckService.cs, 
│
├── Data/
│ └── ApplicationDbContext.cs

---

## What I Learned

- EF Core navigation & projection
- Async data access and separation of concerns with services
- ViewModel composition for clean Razor rendering
- Debugging model mismatches and null-safe LINQ

---

## Project Status

 MVP completed  
 Future additions:
- Role-based login for dispatchers vs admins  
- Mobile UI using .NET MAUI or React Native  

---

├── Controllers/ # API + MVC Controllers
├── Interfaces/ # Service interfaces (IDriverService)
├── Models/ # Entity and DTO classes
├── Services/ # Business logic layer
├── Data/ # ApplicationDbContext
├── Views/ # Razor Views (Privacy, Index)
├── wwwroot/ # Static files
├── appsettings.json # Configurations
└── Program.cs # Main entry point
