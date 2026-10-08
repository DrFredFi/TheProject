# TheProject

[![ci](https://github.com/DrFredFi/TheProject/actions/workflows/ci.yml/badge.svg)](https://github.com/DrFredFi/TheProject/actions/workflows/ci.yml)

TheProject is a production-style e-commerce backend built with .NET 10 microservices: Products, Customers, Orders and Shipping, each a vertical-slice service that owns its own PostgreSQL database. It is a hands-on study of the distributed-systems toolbox applied end to end, with each technique implemented by hand and covered by tests rather than hidden behind a framework. This covers synchronous calls over gRPC with resilience pipelines, asynchronous integration events through a hand-rolled outbox and inbox (RabbitMQ or Kafka), an orchestrated saga with compensation, event sourcing in Shipping, a YARP gateway secured with Keycloak, and a Blazor front end. The system will run identically under .NET Aspire, Docker Compose and a local Kubernetes cluster. It is being built incrementally, and every change lands through a pull request gated by CI.

## Build and test

Requires the .NET SDK version pinned in `global.json`.

```shell
dotnet build TheProject.slnx
dotnet test --solution TheProject.slnx
dotnet run build/ci.cs   # the full CI gate, exactly as GitHub Actions runs it
```