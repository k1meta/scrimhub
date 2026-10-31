# Backlog

Import into Azure Boards. Epics are phases. Stories are requirement ids from the handoff. Phase 0 has no FR ids. It is the scaffold in this repository.

## Epic: Phase 0 — Foundations

Done when a pull request runs the build, the three test suites pass, and the Angular shell shows API health. Deploy stays manual.

## Epic: Phase 1 — Accounts and auth

- FR-A1 Register with email, password, and display name. ASP.NET Core Identity. Minimum password length 10.
- FR-A2 Login and logout. Access token plus rotating, revocable refresh token.
- FR-A3 Profile: display name, optional unverified Riot ID, preferred positions, IANA time zone.

## Epic: Phase 2 — Teams, roles, reference data

- FR-T1 Create a team. Creator becomes Owner.
- FR-T2 Join by invite code. Owner and Admins manage the code.
- FR-T3 Roster, roles, positions. Last Owner cannot be removed or demoted.
- FR-T4 Team settings: map pool, default veto format. Discord webhook waits for Phase 6.
- FR-T5 A user can belong to several teams and switch among them.
- Reference data client and cache for agents, maps, weapons, and tiers.

## Epic: Phase 3 — Scrims and RSVP

- FR-S1 Create, edit, and cancel scrims. Start time stored in UTC.
- FR-S2 Calendar or list, filterable by status.
- FR-S3 RSVP Yes, No, or Maybe, plus a five-stack summary.

## Epic: Phase 4 — Match log, notes, VODs, dashboard

- FR-M1 Log a match, including the five players and agents.
- FR-M2 Match notes with category, author, and timestamp.
- FR-M3 External VOD links and timestamped notes. No upload. No embed.
- FR-M4 Dashboard stats computed on the server.
- FR-S4 Attach the veto result and match results to a scrim.

## Epic: Phase 5 — Live map veto

- FR-V1 Admin creates a veto session.
- FR-V2 Two seats. Seat B can be a guest with a signed seat token. Others spectate.
- FR-V3 Live bans, picks, and side choices.
- FR-V4 Server rejects illegal actions. Reconnect restores state.
- FR-V5 Completed result is saved on the scrim.

## Epic: Phase 6 — Notifications and hardening

- FR-N1 Optional Discord webhook: reminder about one hour before a scrim, plus created and cancelled messages.
- FR-A5 Email verification and password reset.
- Security review, rate limits, security headers, accessibility pass, Application Insights alerts, privacy policy, verified account deletion, backup notes.

## Epic: Phase 7 — Launch and portfolio polish

- README diagram, screenshots, cost notes, ADR index, seeded demo account, load sanity check.
- Human: Riot Developer Portal registration, disclaimer wording, custom domain, first teams.

## Stretch

Only after Phase 7: playbook notes, avatar uploads, audit log, Riot Sign On, Playwright, staging slots, a SignalR comparison branch.
