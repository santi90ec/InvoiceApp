# Development Best Practices

## Role Boundary

Codex implements approved work as Pepe. Codex does not redesign the system.

If a change requires a contract, dependency, data model, architecture, security policy, or infrastructure decision, report it to S4 instead of deciding locally.

## .NET Implementation

- Prefer existing project patterns over new abstractions.
- Keep methods small enough to read without jumping across many files.
- Validate public method inputs close to the boundary.
- Use `ArgumentNullException.ThrowIfNull` for required reference arguments.
- Do not catch broad exceptions unless the method can add useful context or return a safe domain result.
- Avoid hidden side effects in LINQ queries.
- Avoid blocking calls such as `.Result`, `.Wait()`, and `GetAwaiter().GetResult()`.
- Pass `CancellationToken` only when an existing contract already supports it or S4 approves a contract change.

## Async

- Async methods must end in `Async`.
- Await tasks directly.
- Do not create fire-and-forget work unless the existing codebase already owns that lifecycle.
- Preserve exception behavior unless the task explicitly requires safer error handling.

## Dependency Injection

- Use constructor injection.
- Do not resolve services manually from `IServiceProvider` unless the existing pattern requires it.
- Do not introduce service locator behavior.
- Register implementations in the existing composition root only when required by the task.

## Entity Framework Core

- Use LINQ queries or EF Core APIs, not string-built SQL.
- Keep queries explicit and readable.
- Do not change schema, migrations, indexes, relationships, or database provider without S4 approval.
- Avoid loading more data than needed.
- Use `AsNoTracking` for read-only queries when it matches existing patterns.

## XML Processing

- Treat XML invoice content as untrusted input.
- Disable DTD processing and external resolvers.
- Validate size before parsing.
- Catch `XmlException` and return or log safe messages.
- Never log raw XML, issuer names, tax IDs, addresses, or access keys unless S4 authorizes sanitized diagnostics.

## Razor Pages

- Keep page handlers thin.
- Validate model state before executing use cases.
- Return user-safe errors.
- Do not expose internal exception messages in UI.
- Keep UI changes consistent with the current Razor Pages and Bootstrap style.

## Logging

- Use structured logging.
- Prefer IDs, counts, statuses, and durations.
- Do not log secrets, full tax IDs, full emails, addresses, raw XML, cookies, tokens, browser storage, or stack traces in user-visible responses.
- Error logs can include exceptions, but messages must avoid sensitive payload values.

## Testing

- Use xUnit, Moq, and FluentAssertions when available.
- Follow Arrange-Act-Assert.
- Cover happy path, validation failures, edge cases, and exception behavior affected by the change.
- Do not add broad snapshot tests for small behavior.
- Do not skip tests unless S4 approves and the reason is documented.

## Validation Commands

Preferred validation sequence:

```powershell
dotnet restore
dotnet build
dotnet test
```

If restore requires network and fails because of restricted access, report it and request permission before using network access.

## Git Hygiene

- Never commit directly to `main`.
- Do not rewrite history.
- Do not stage unrelated user changes.
- Before reporting completion, inspect `git status --short`.
- Conventional commit examples:
  - `feat: add invoice parsing validation`
  - `fix: prevent unsafe XML resolver usage`
  - `test: cover invoice classifier failures`
