# TODO

**TODO** is a WebApplication deloped as a test assigment for Luna Edge

## How to Run

Install:
- [**.NET SDK 8.0**]("https://dotnet.microsoft.com/en-us/download")
- [**PostgreSQL**]("https://www.postgresql.org/download/")
- [**Docker**]("https://www.docker.com/")

Clone Repo
```bash
$git clone https://github.com/UkrainoPhill/TODO.git
```
Change config for PostgreSQL Server in appsettings.json and docker-compose.yml

Then run in console
```bash
dotnet tool install --global dotnet-ef
```

Go to folder, where you clone repo and run program
```bash
dotnet run
```
Run Docker
```bash
docker-compose up
```
After docker update databse writting
```bash
dotnet ef database update -s TODO.API -p TODO.Persistence
```
Here you are, now u can see endpoints using http://localhost:5057/swagger or https://localhost:7183/swagger

## Usage

Here are the endpoints for TODO application:

### User Endpoints

- **Register**: `POST /users/register`  - Description: Register by email, username, and password.
  - Restrictions: None. This endpoint is publicly accessible.  - Password Requirements: 
    - Length: 8 to 16 characters    - Must contain at least one uppercase letter
    - Must contain at least one lowercase letter    - Must contain at least one number
- **Login**: `POST /users/login`  - Description: Generate your JWT token and add it to the cookie.  - Restrictions: None. This endpoint is publicly accessible.

### Task Endpoints

- **Get Tasks**: `GET /tasks`  - Description: Get all user tasks. You can filter tasks by Status, DueDate, and Priority, and sort by DueDate and Priority. Pagination is supported.  - Restrictions: Requires authorization. Only accessible by the authenticated user.
- **Get Task by ID**: `GET /tasks/{id}`  - Description: Get task by ID.  - Restrictions: Requires authorization. Only accessible by the authenticated user who owns the task.
- **Create Task**: `POST /tasks`  - Description: Create a new task.  - Restrictions: Requires authorization. Only accessible by the authenticated user.
- **Update Task**: `PUT /tasks/{id}` - Description: Update task by ID.  - Restrictions: Requires authorization. Only accessible by the authenticated user who owns the task.
- **Delete Task**: `DELETE /tasks/{id}` - Description: Delete task by ID.  - Restrictions: Requires authorization. Only accessible by the authenticated user who owns the task.
