# Hotel Booking Management System Technical Challenge

**Lead Developer:** Ryan Smith  
**Date:** 14/04/2026  

---

## How to Test

The API is hosted on Azure (App Service) and can be accessed via:

> **TBC**

Once loaded, Swagger UI will be available for interacting with all endpoints.

---

## Quick Start

1. Open Swagger UI  
2. Reset the database:
   - `POST /api/Test/db/reset`  
3. Seed test data:
   - `POST /api/Test/db/seed`  
4. Retrieve a hotel:
   - `GET /api/Hotels/search`  
5. Use the returned `HotelID` to:
   - Search available rooms  
   - Create a booking  

---

## Test Endpoints

> These endpoints are only available in **Development mode**

### POST `/api/Test/db/reset`

Clears all data from:
- Hotels  
- Rooms  
- Bookings  

> This would not be exposed in a production environment but is included here to simplify testing.

---

### POST `/api/Test/db/seed`

Recreates a base dataset for testing:
- Adds 3 sample hotels  
- Adds 6 rooms to each of the hotels  

---

## Hotels Controller

### GET `/api/Hotels/search`

Returns all hotels in the system.

**Why this exists:**
- Allows discovery of `HotelID` values (GUIDs) for use in other endpoints  

**Design note:**
- GUIDs are used to avoid predictable IDs and reduce the risk of enumeration in future secured implementations.

---

### GET `/api/Hotels/search/name`

Search for hotels by name.

**Query Parameters:**

| Parameter | Type | Description |
|----------|------|-------------|
| `name` | string | Required. Case-insensitive search |

**Behaviour:**
- Trims whitespace  
- Converts input to lowercase for comparison  
- Can return multiple results  

---

### GET `/api/Hotels/bookings/{bookingRef}`

Retrieve booking details by reference.

**Path Parameters:**

| Parameter | Type | Description |
|----------|------|-------------|
| `bookingRef` | GUID | Unique booking reference |

**Returns:**
- Booking details  
- Associated room  
- Associated hotel  

---

### GET `/api/Hotels/bookings/available`

Returns all available rooms based on search criteria.

**Query Parameters:**

| Parameter | Type | Description | Notes |
|----------|------|-------------|---------|
| `HotelID` | GUID | Retrieved via `/api/Hotels/search` |
| `StartDate` | Date | Format: `yyyy-MM-dd` (e.g. 2026-04-14) | Must be at least todays date
| `EndDate` | Date | Format: `yyyy-MM-dd` (e.g. 2026-04-21) | Must be after `StartDate` |
| `NumberOfGuestsOnBooking` | int | Range: 1–6 |

**Logic:**
- Filters rooms by capacity  
- Excludes rooms with overlapping bookings  
- Ensures no double booking occurs 

---

### POST `/api/Hotels/bookings/create`

Create a new booking.

**Request Body:**
Uses the same model as the availability search.

**Behaviour:**
- Finds a suitable available room  
- Selects the closest match based on capacity  
- Creates a booking with a unique GUID reference  

**Concurrency Handling:**
- Re-checks availability immediately before saving  
- Prevents race conditions where two users attempt to book the same room  

> In a production system, this would be handled using database transactions or constraints.

---

## Design Decisions

### Technology

- Built using **.NET 8 (LTS)**  
- Swagger included for API testing and documentation and to meet use case requirement

---

### Architecture

Implemented using the **Repository Pattern** to ensure separation of concerns:

- Data Layer  
- Repository Layer  
- Service Layer  
- API Layer  

**Benefits:**
- Improved maintainability  
- Easier testing  
- Clear separation of responsibilities  

---

### Database

- **SQLite** used for simplicity and fast setup  
- Database is created automatically at runtime in startup file

> In a production environment, SQL Server and EF Core migrations would be used.

---

## Additional Considerations

- Authentication not implemented  
  - Would typically use **OAuth2** in production  

- Error handling kept simple  
  - Would normally be centralised via middleware , perhaps Elmah

- API designed to be easily testable via Swagger  

---

## Automated Testing

Basic automated tests have been implemented using **NUnit** for the repository layer.

The focus of these tests was to validate:

- Core booking logic  
- Date overlap scenarios  
- Edge cases and boundary conditions  
- Invalid or unexpected inputs  

These tests helped identify edge cases during development, some of which were initially handled at controller level and later moved into the repository layer for better separation of concerns.

> Due to time constraints, testing has been limited to the repository layer, but this could be expanded to include service and integration testing in a production environment.

---

## Use of AI

AI was used to assist with generating unit tests for the repository layer.

**Prompt used:**

> "Create a small sample of NUnit3 compatible tests for the repository method shown below, ensuring coverage of edge cases, outliers, and potential vulnerabilities."

**Outcome:**
- 12 tests generated  
- 10 passed immediately  
- 2 highlighted edge cases  

These edge cases were initially handled at controller level, but were subsequently addressed within the repository layer to improve robustness at speed.

---

## Future Improvements

If this were to be extended further:

- Introduce authentication and authorisation (OAuth2)  
- Implement database transactions for stronger concurrency control of bookings 
- Expand test coverage beyond repository layer to the other repositories
- Introduce global error handling middleware  
- Add logging and monitoring  

---
