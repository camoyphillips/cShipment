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

