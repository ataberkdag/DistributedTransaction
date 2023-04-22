# Distributed Transaction Sample

.Net 6 Distributed Transaction Sample with MassTransit, RabbitMQ, OutboxPattern, Identity, Service Discovery/Registry, Gateway and more.

## Includes

- Onion Architecture
- CQRS
- Mediator (MediatR)
- EF Core (PostgreSQL)
- Dapper
- MongoDB
- MassTransit, RabbitMQ
- Outbox Pattern
- Identity
- Authentication
- Role based Authorization
- Distributed Cache (Redis)
- Distributed Lock (Redis - Redlock)
- Service Discovery/Registry (Consul)
- Gateway (Ocelot)
- Serilog (File, Console, ElasticSearch)
- Api Versioning
- Http Client
- FluentValidation
- AutoMapper
- Repository, Unit of Work Pattern
- Polly

## Run

### Docker

- To run RabbitMQ, Consul, Postgres, Redis, PgAdmin, ElasticSearch, Kibana as a container. Follow this command on Console

```bash
docker-compose up -d
```

### Migration

- To apply migrations follow this command on Package Manager Console for Order-Stock-Payment-User Microservices. (Set starting project to API and set default project to Infrastructure on Package Manager Console)

```bash
update-database
```

### RabbitMQ

- Microservices are not using default RabbitMQ user. Create a RabbitMQ user with following credentials. **"admin"** and **"sa1234"** . 

> If you want to create a different user then make sure to change appsettings too.

### Get a Token

- Get a Token from User API (over Gateway) for authn, authz.
	- Default Gateway Endpoint to Get Token: http://localhost:5291/v1/Authentication/CreateToken
	- Default Endpoint to get token: http://localhost:5099/api/Authentication/CreateToken

> Admin user credentials are **"admin@test.com"** and **"admin_12345."**.

### What is Next?

- Place an order.
	- Default Gateway Endpoint to Place Order: http://localhost:5291/v1/Orders/PlaceOrder
	- Default Endpoint to Place Order: http://localhost:5091/api/v1/Orders/PlaceOrder

> Dont forget to add Authorization key to Headers.

## TODO
- [ ] Rule Engine (.Net Rules Engine)
- [ ] Distributed Tracing (OpenTelemetry, Jaeger)
- [ ] Healthcheck
- [ ] Gateway Logging
- [ ] Dockerize
- [ ] Unit Tests
- [ ] Bugfix (Enabling DbContextHandler)
