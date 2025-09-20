# Instructions for building and running the application

## 1. Restore dependencies

```bash
dotnet restore
```

## 2. Create migrations

```bash
dotnet ef migrations add InitialCreate --project DefaultCQRS
```

## 3. Apply migrations

```bash
dotnet ef database update --project DefaultCQRS
```

## 4. Run the application

```bash
dotnet run --project DefaultCQRS
```

After running the application, you can access the Swagger UI at `https://localhost:5001/swagger/index.html` to test the API.
