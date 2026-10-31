# ADR-001: Three deployable units, Node for realtime

## Context

The app has to show Angular, TypeScript, C#, ASP.NET Core, Node.js, SQL Server, and Azure on a resume, and it has to be a product teams can use. The live map veto needs a bidirectional session. SignalR is the usual .NET choice for that.

The veto is a small state machine with two seats and spectators. It does not need to scale past one process for the first teams. A second realtime stack keeps socket code out of the HTTP API.

## Decision

Ship three units:

- Angular SPA on Azure Static Web Apps. Standalone components and signals. Angular Material dark theme. No NgRx.
- ASP.NET Core API on .NET 10, EF Core, SQL Server. Layers are Domain, Application, Infrastructure, and Api. No MediatR and no CQRS.
- Node.js 26 TypeScript service for veto sessions and, later, Discord notifications.

The realtime service will use Socket.IO in Phase 5. Phase 0 only serves `GET /health`, so the Socket.IO package is not installed yet. The API will sign access tokens with an asymmetric key. The Node process will verify them with the public key only. It will persist the finished veto by calling the API with a service credential.

Veto state lives in memory on a single instance. The API stores the finished result. A second instance would not see the in-memory session. Do not turn on autoscale for the realtime app until that design changes.

## Consequences

The interview explanation is the tradeoff: SignalR would be one less runtime, and Node is here so the realtime concern and the second ecosystem stay visible.

Operations cost is two Web Apps on one plan, plus the database and the static app. A process restart abandons in-memory veto sessions. Clients must be able to see an abandoned status. That behavior is Phase 5 work.

Azure App Service may not list a Node 26 runtime stack on the day of the first deploy. The Bicep parameter defaults to `NODE|22-lts` until the portal lists Node 26. Local development and CI still use Node 26.
