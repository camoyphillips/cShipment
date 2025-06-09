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
├── Controllers/ # API + MVC Controllers
├── Interfaces/ # Service interfaces (IDriverService)
├── Models/ # Entity and DTO classes
├── Services/ # Business logic layer
├── Data/ # ApplicationDbContext
├── Views/ # Razor Views (Privacy, Index)
├── wwwroot/ # Static files
├── appsettings.json # Configurations
└── Program.cs # Main entry point
