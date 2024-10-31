# Dogshouse Service API

A RESTful API for managing dogs data, built with Clean Architecture principles and modern .NET practices.

## Getting Started

### Prerequisites
- .NET 8
- MS SQL Server
- EF Core
- Docker (for running integration tests)

### Running the Project
1. Clone the repository
2. Add connection string with **YOUR** credentials to `appsettings.json`:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=DogsHouseDB;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True"
  }
```
3. Run the app using http or https profile (migrations applied and data seeded automatically)


## API Endpoints

### `GET /ping`

### `GET /dogs/{name}`

### `GET /dogs`

Optional parameters:
- `pageSize` - Items per page
- `pageNumber` - Page number
- `attribute` - Sort field (e.g., `tail_length`, `weight`)
- `order` - Sort order (`asc`/`desc`)

Notes:
- For sorting API does not allow providing 'order' without 'attribute'.
- Pagination metadata is included in response headers.
- By default sorted by 'name' in ascending order
- By default paginated to page 1, size 10

### `POST /dogs`

## Implementation Considerations

### Technical Task Interpretation
- Dog's 'name' serves as primary key (unique identifier) based on my understanding of requirements. ID would be better since it's a common situation where dogs have different names, but a specific table structure was provided so I tried to not violate the requirements.
- API follows snake_case for both JSON properties and query attribute values (`tail_length`, not `tailLength`) since such approach was shown in the task.
- Query parameters use camelCase (`pageSize`, not `page_size`) following URL conventions.
- API attempts to follow robustness principle: be conservative in what you do, be liberal in what you accept from others.

### Rate Limiting
Default limit: 10 requests/second (configurable in appsettings). You can test it with the following Bash command when API is running with http:
```bash
for i in {1..15}; do
   echo "Request $i:"
   curl -X GET http://localhost:5243/ping -w "\nHTTP Status: %{http_code}\n"
done
```
When running with https:
```
for i in {1..15}; do
   echo "Request $i:"
   curl -k -X GET https://localhost:7031/ping -w "\nHTTP Status: %{http_code}\n"
done
```

## Technical Stack & Architecture

- Clean Architecture with CQRS (MediatR)
- Unified Problem Details factory for standardized API responses
- Specification pattern for building database queries (pagination and sorting)
- Global exception handling
- FluentValidation across layers
- Result pattern for flow control

## Testing

- Unit tests for API, Application, and Domain layers
- Integration tests using containerized databases
