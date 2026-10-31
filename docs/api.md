# API

Base URL locally: `http://localhost:5080`.

OpenAPI, in Development: `GET /openapi/v1.json`.

Errors from unhandled exceptions use RFC 7807 problem details. These two routes return a small JSON body instead.

## GET /health

Liveness. Does not open a database connection.

Response `200`:

```json
{ "status": "ok", "service": "api" }
```

## GET /health/ready

Readiness. Opens a SQL connection with `CanConnectAsync`.

Response `200` when SQL accepts the connection:

```json
{ "status": "ok", "service": "api" }
```

Response `503` when SQL is down, the database does not exist, or the connection string is missing:

```json
{ "status": "unavailable", "service": "api" }
```

The log line records the exception type only. It does not record the connection string.

## Realtime

`GET http://localhost:3001/health` returns the same JSON shape with `"service": "realtime"`. There is no ready route on that process.
