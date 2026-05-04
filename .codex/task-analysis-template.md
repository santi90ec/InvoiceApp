# Task Analysis Template

Use this template before non-trivial changes. Keep the written output short unless S4 asks for a full analysis.

## 1. Request

What exactly did S4 ask to change?

## 2. Allowed Scope

Files or modules likely involved:

- `src/...`
- `test/...`
- `docs/...`

Forbidden or approval-required areas:

- Public contracts.
- DTOs and records.
- NuGet packages.
- Database schema and migrations.
- Infrastructure and CI/CD.
- Secrets and credentials.

## 3. Current Behavior

What does the code currently do?

What tests already cover it?

## 4. Proposed Local Change

Smallest implementation that satisfies the request:

- Internal method changes.
- Local helper extraction.
- Focused tests.

## 5. Security Analysis

Inputs:

Outputs:

Sensitive data touched:

Logging impact:

File/path impact:

XML impact:

Dependency impact:

## 6. Validation

Commands to run:

```powershell
dotnet build
dotnet test
```

Expected result:

- Build passes.
- Tests pass.
- No new warnings.

## 7. Report

Final response should include:

- Files changed.
- Tests run.
- Security notes.
- Blockers or S4 decisions required.
