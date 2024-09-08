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

- **Register**: `POST /users/register` (register by email, username and password)
- **Login**: `POST /users/login` (generate your JWT token and adding it to cookie)

### Task Endpoints

- **Get Tasks**: `GET /tasks` (get all user task, require authorization, you can filter tasks by Status, DueDate, and Priority, can sort by DueDate and Priority, can take pages
)
- **Get Task by ID**: `GET /tasks/{id}` (get task by id, require authorization)
- **Create Task**: `POST /tasks` (create task, require authorization)
- **Update Task**: `PUT /tasks/{id}`(update task by id, require authorization)
- **Delete Task**: `DELETE /tasks/{id}` (delete task by id, require authorization)