# Security Deep Analysis

## Security Posture

This repository may process Ecuadorian fiscal documents, browser automation sessions, AI classification requests, and local persistence. Treat it as sensitive by default.

Primary risks:

- Credential exposure.
- PII and fiscal data leakage.
- Unsafe XML parsing.
- Path traversal.
- Injection through SQL, shell commands, prompts, or browser automation inputs.
- Excessive logging.
- Unapproved dependency or infrastructure changes.

## Sensitive Assets

Never print, commit, or log:

- API keys, GitHub tokens, OpenAI keys, SRI credentials, cookies, session tokens.
- Cedula, RUC, names, addresses, emails, phone numbers, access keys, invoice XML payloads.
- Browser screenshots, SRI evidence, downloaded invoices, upload contents.
- Full local paths when they reveal user or machine details in user-facing errors.

If a secret-like value is discovered, report only the file path and key name or context. Do not quote the value.

## Threat Model Checklist

For every non-trivial change, answer:

- Entry points: What inputs can a user, file, browser page, AI response, or external service control?
- Trust boundary: Where does untrusted data enter application code?
- Validation: Are null, empty, size, format, range, and allowed values checked?
- Output: Could sensitive data be returned to UI, logs, exceptions, or files?
- Persistence: Could untrusted data affect queries, filenames, or stored records?
- External calls: Could data sent to OpenAI, SRI, or Playwright contain unnecessary PII?
- Failure mode: On exception, does the user see a safe message and does the log avoid payload leakage?

## XML Security

Required defaults:

```csharp
var settings = new XmlReaderSettings
{
    DtdProcessing = DtdProcessing.Prohibit,
    XmlResolver = null
};
```

Also validate:

- Maximum XML size.
- Expected root element.
- Required fields.
- Encoding assumptions.
- Safe failure on malformed XML.

Do not use `XDocument.Parse` on untrusted XML unless the calling path has already disabled DTD and external resolution or S4 explicitly accepts the risk.

## File Security

For file operations:

- Use a fixed base directory.
- Normalize with `Path.GetFullPath`.
- Compare against the normalized allowed base directory.
- Use `Path.GetFileName` when only filenames are allowed.
- Reject absolute paths from user input.
- Reject `..` traversal.
- Do not write outside approved project paths.

Do not read user uploads or SRI evidence unless the task explicitly requires it.

## Logging Review

Every new log line must pass:

- Does it include only IDs, counts, statuses, durations, or safe enums?
- Does it avoid raw payloads?
- Does it avoid secret-bearing config values?
- Does it avoid PII?
- Does it avoid user-controlled strings that could forge logs?

Use placeholders:

```csharp
_logger.LogInformation("Processed invoice {InvoiceId} with status {Status}", invoiceId, status);
```

Avoid:

```csharp
_logger.LogInformation("Processed XML {Xml}", xml);
```

## AI and Prompt Safety

When adding AI integration:

- Send the minimum data needed.
- Prefer normalized, redacted, or derived fields over raw invoice XML.
- Treat model output as untrusted.
- Validate model output before use.
- Do not let model output select file paths, commands, URLs, or database operations without validation.
- Log model request IDs or statuses, not full prompts or responses containing PII.

## Browser Automation Safety

When using Playwright:

- Do not persist real credentials in code or config files.
- Do not log cookies, local storage, session storage, or screenshots.
- Store evidence only in approved locations.
- Treat page content as untrusted.
- Avoid executing arbitrary script built from user input.

## Dependency Review

Before adding or changing a package, S4 must approve:

- Package name.
- Version.
- Reason.
- License.
- Maintenance status.
- Security impact.

Do not add dependency changes as part of a local fix unless the task explicitly includes approval.

## Pre-PR Security Gate

- No secrets in files changed.
- No raw XML logged.
- No PII logged.
- No unsafe XML parser configuration.
- No string-built SQL.
- No shell command built from user input.
- No path traversal risk.
- No user-visible stack traces.
- No unapproved packages.
- Tests cover security-relevant validation paths touched by the change.
