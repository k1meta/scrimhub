# ADR-002: Guid ids and token storage

## Context

Every table needs one id type. ASP.NET Core Identity, added in Phase 1, uses a `Guid` user key when configured with `Guid` keys. A `bigint` key on teams and scrims would force a second key strategy on every foreign key to a user.

The browser and the API will sit on different hosts in Azure: Static Web Apps and App Service. A cookie set by the API is cross-site from the Angular origin unless a custom domain or a linked backend makes them same-site. `SameSite=Lax` cookies are not sent on those cross-site calls.

## Decision

All entity ids are `Guid`. `CreatedAt` and `UpdatedAt` are UTC. The base type is `ScrimHub.Domain.Entity`. Phase 0 maps no tables. The first migration is the empty model snapshot so later phases add columns in new migrations.

Phase 1 will store the access token in memory and the refresh token in an `httpOnly`, `Secure` cookie. The access token is not in `localStorage`. The refresh token is stored hashed in SQL and rotated on use. Phase 0 does not issue tokens.

Before the first authenticated deploy, pick one cookie boundary:

- Same-site: custom domain or Static Web Apps linked backend, `SameSite=Lax` or `Strict`.
- Cross-site: `SameSite=None` and `Secure`, with credentialed CORS limited to the real web origin.

## Consequences

Guids are larger indexes than bigints and are unordered. That is acceptable at this scale. Identity user ids and team foreign keys match.

The cookie choice stays open until the public hostnames exist. Building login against `localhost` plus the dev proxy can use a same-site cookie, because the browser only talks to port 4200. Do not assume that local setup is the production cookie setup.
