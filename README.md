# Project Name
Trainee Management API
 
# Technology Used
 
- ASP.NET Core Web API
- C#
- Entity Framework Core InMemory Database
- Swagger
- Postman
 
# How to Run
 
1. Clone the repository.
2. Open the project in Visual Studio or VS Code.
3. Restore dependencies:
dotnet restore
 
4. Run the application:
dotnet run
 
5. Open Swagger using the URL shown in the terminal.
 
---
 
# API List
 
Health Check
- GET "/api/health"
 
Get All Trainees
- GET "/api/trainees"
 
Get Trainee By Id
- GET "/api/trainees/{id}"
 
Create Trainee
- POST "/api/trainees"
 
Update Trainee
- PUT "/api/trainees/{id}"
 
Delete Trainee
- DELETE "/api/trainees/{id}"
 
Search Trainees
- GET "/api/trainees?search={value}"
 
---
 
# Sample Request JSON
 
{
  "firstName": "Amit",
  "lastName": "Sharma",
  "email": "amit@gmail.com",
  "techStack": "ASP.NET Core",
  "status": "Active"
}
 
---
 
# Sample Response JSON
 
{
  "id": 1,
  "firstName": "Amit",
  "lastName": "Sharma",
  "email": "amit@gmail.com",
  "techStack": "ASP.NET Core",
  "status": "Active"
}
 
---

# Challenges Faced
- Understanding the syntax and code format
- finding other resources to understand logic flow and notations
- referring multiple site to write structured code

---
 
# Known Limitations
 
- Uses EF Core InMemory Database.
- Data is lost when the application stops.
- No authentication or authorization implemented.
- Search supports basic text matching only.
- Database persistence is not available in the current phase.