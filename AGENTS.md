# ScrimHub agent guide

ScrimHub is a web app where Valorant teams organize practice: roster, scrim calendar, match log, and a live map veto. The product name does not include Riot trademarks. Phase 0 is the foundation. Do not build a later phase unless the owner asks for that phase.

## Stack

- Web: Angular 22, TypeScript, standalone components, signals, lazy routes, reactive forms when forms exist. Angular Material dark theme. No NgRx.
- API: .NET 10 LTS, ASP.NET Core, EF Core, SQL Server. Projects: Domain, Application, Infrastructure, Api. No MediatR. No CQRS.
- Realtime: Node.js 26 LTS, TypeScript. Phase 0 is a health server. Socket.IO arrives with the veto in Phase 5.
- Ids are `Guid`. `CreatedAt` and `UpdatedAt` are UTC. See `docs/adr/002-ids-and-tokens.md`.
- Pin the SDK with `global.json`. Pin Node with `engines` in each Node project.

## Layers

- Domain owns entities and rules that do not need a database or HTTP.
- Application owns use cases. It references Domain only.
- Infrastructure owns EF Core, SQL Server, and the local `.env` reader.
- Api owns HTTP, OpenAPI, problem details, logging, and composition.
- The Angular app calls this API. It does not call valorant-api.com.
- The realtime service does not call the API in Phase 0.

## Secrets and config

Never commit secrets. Local SQL credentials live in `.env`, copied from `.env.example`. Production secrets belong in Key Vault. Do not log connection strings, tokens, or webhook URLs. Do not collect Riot credentials.

## Work rules

- One phase at a time. A phase is done when its acceptance criteria pass.
- Every feature ships with tests. Authorization changes need a negative test. That starts in Phase 1.
- Conventional Commits. When a board item exists, put `AB#123` in the commit message.
- Record non-obvious decisions in `docs/adr/`. One page: context, decision, consequences.
- Prefer clear code the owner can explain in an interview.
- EF migrations are applied by hand locally and later by the pipeline. The API does not migrate on startup.
- Do not deploy to Azure until the owner creates the subscription, DevOps project, and service connections.

## Local ports

- Web: `http://localhost:4200`
- API: `http://localhost:5080`
- Realtime: `http://localhost:3001`
- SQL Server: `localhost,1433`

## Verify

```powershell
copy .env.example .env
docker compose up -d
dotnet test ScrimHub.slnx
dotnet run --project src/api/ScrimHub.Api --launch-profile http
npm test --prefix src/web
npm start --prefix src/web
npm test --prefix src/realtime
npm run dev --prefix src/realtime
```
