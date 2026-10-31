# ScrimHub

Practice hub for Valorant teams: roster, scrims, match notes, and a live map veto. This repository is the Phase 0 foundation. Accounts, teams, and the veto are not built yet.

ScrimHub is not endorsed by Riot Games.

## Prerequisites

- .NET SDK 10.0.403 or a later 10.0 feature band (`global.json`)
- Node.js 26.13.0 or later (Active LTS)
- Docker, for local SQL Server

## Run locally

From the repository root:

```powershell
copy .env.example .env
docker compose up -d
dotnet tool restore
dotnet ef database update --project src/api/ScrimHub.Infrastructure --startup-project src/api/ScrimHub.Api
dotnet run --project src/api/ScrimHub.Api --launch-profile http
```

In a second terminal:

```powershell
npm start --prefix src/web
```

In a third terminal:

```powershell
npm run dev --prefix src/realtime
```

Open `http://localhost:4200`. The page calls `GET /health` through the Angular dev proxy. The API listens on `http://localhost:5080`. The realtime service listens on `http://localhost:3001`.

`GET /health` does not use the database. `GET /health/ready` checks SQL Server. The local connection string is `SQL_CONNECTION` in `.env`. `TrustServerCertificate=True` is for the container certificate only. Do not use that flag against Azure SQL.

## Tests

```powershell
dotnet test ScrimHub.slnx
npm test --prefix src/web
npm test --prefix src/realtime
```

## Layout

- `src/api` — Domain, Application, Infrastructure, Api
- `src/web` — Angular app
- `src/realtime` — Node service
- `tests` — API unit and integration tests
- `infra` — Bicep. `main.bicep` creates the resource group. `resources.bicep` creates the resources inside it. Not deployed from this phase.
- `pipelines/azure-pipelines.yml` — Azure DevOps build and a gated deploy stage
- `docs/adr` — architecture decisions

The solution file is `ScrimHub.slnx` (the .NET 10 solution format).

## Azure

Nothing in this phase is deployed. Before a deploy, confirm the region (default `westeurope`), the SKUs, and current prices in the Azure pricing calculator.

Intended resources, created by `infra/main.bicep` when you later run a deployment:

- One resource group
- One Linux App Service plan, SKU `B1`, shared by the API and the realtime app. `B1` is the smallest tier that supports Always On and WebSockets.
- Two Web Apps. The API uses `DOTNETCORE|10.0`. The realtime app uses `NODE|22-lts` until the portal lists a Node 26 stack. The code itself targets Node 26.
- Azure Static Web Apps, Free tier, for the Angular app
- Azure SQL, General Purpose serverless, 1 vCore max, minimum 0.5 vCore, auto-pause after 60 minutes. Switch the parameters to the free database offer if that offer is still available in the subscription.
- Key Vault, standard SKU, RBAC
- Log Analytics workspace and Application Insights

The SQL admin password is a deploy-time parameter. It is not stored in git. Create the Key Vault secret for the API connection string before `/health/ready` can succeed in Azure. `/health` does not need it.

Create the Azure DevOps pipeline from `pipelines/azure-pipelines.yml` against the GitHub repo. Set variable group secrets `sqlAdminPassword` and `swaDeploymentToken`, and set `azureServiceConnection` to the Azure service connection name. The deploy stage targets an environment named `production` and waits for manual approval. Branch policy on `main`: pull request required, and this pipeline must pass.

New Azure DevOps organizations often have no free Microsoft-hosted parallelism until you request the grant.

## License

No license is chosen yet.
