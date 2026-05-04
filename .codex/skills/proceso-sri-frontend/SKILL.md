---
name: proceso-sri-frontend
description: Frontend design and UX contract for Proceso SRI based on the Lovable mock at https://roceso-facil.lovable.app/. Use when modifying Razor Pages, layouts, CSS, navigation, upload/results/history screens, or any user-facing frontend flow for the SRI IVA refund app.
---

# Proceso SRI Frontend

## Source Of Truth

Canonical mock:

`https://roceso-facil.lovable.app/`

Observed routes:

- `/`: home and start CTA.
- `/paso-1`: first step for selecting invoice folder.
- `/resultados`: final results summary.
- `/historial`: previous executions list.

Use the mock as the visual contract. Do not invent a different app style unless S4 approves.

## Product UX

Target user: non-technical Ecuadorian taxpayer or family helper.

Tone:

- Spanish.
- Plain, friendly, concrete.
- Avoid technical labels such as DTO, handler, confidence, raw XML, pipeline, parser, or classifier.
- Prefer "Te ayudamos...", "paso a paso", "Tus datos estan seguros", "Llama a tu familiar o contador de confianza."

Primary UX rule:

The user should see what matters for the tax process, not internal implementation state.

## Visual System

Use these patterns from the mock:

- Page shell: full-height light background.
- Header: white card background, bottom border 2px, centered max width.
- Main container: max width about `64rem` / `max-w-5xl`, horizontal padding 16px mobile and 32px desktop.
- Base font size: large and accessible, approximately 18px.
- Cards: white background, `2px` border, rounded large corners, subtle shadow only when needed.
- Buttons: large touch targets, minimum height around 56px, rounded large corners, semibold text.
- Icons: simple line icons next to actions/status where useful.
- Progress: five-step horizontal stepper for process pages.
- Avoid dense dashboard UI on user-flow screens.
- Avoid tiny technical tables as the primary output.

Approximate design tokens from the mock:

- `--radius: .75rem`
- background: near-white warm neutral.
- foreground: dark blue-gray.
- primary: strong blue.
- secondary: very light blue-gray.
- success: green.
- warning: warm yellow.
- destructive: red.
- border/input: light blue-gray.

## Required Structure

Shared header:

- Left brand block with square `SRI` mark.
- Text: `Proceso SRI`.
- Subtitle: `Devolucion de IVA`.
- Right nav link: `Historial`.

Shared footer:

- `Necesitas ayuda? Llama a tu familiar o contador de confianza.`

Home:

- H1: `Bienvenido`.
- Intro text: `Te ayudamos a hacer tu Devolucion de IVA, paso a paso.`
- Main card explaining what the app does.
- Primary CTA: `Empezar Proceso`.
- Last execution card if data exists.
- Security/info alert: data stays local, no passwords stored.

Process pages:

- Include 5-step progress:
  1. Carpeta
  2. Revisar
  3. Clasificar
  4. Iniciar
  5. Resultados
- Active step uses primary styling.
- Completed steps use success styling and check icon.
- Use large cards and explicit primary/secondary actions.

Upload/folder page:

- H1: `Elige la carpeta de tus facturas`.
- Explain where XML files are expected.
- Prefer a single obvious upload/folder input area.
- Provide help text for non-technical users.

Results page:

- Use a success alert when process completes.
- Highlight business totals in large cards.
- Show evidence/report actions only when available.
- User-facing invoice rows must prioritize:
  - Razon social
  - Comprador
  - Fecha
  - Total
  - IVA status
  - Clasificacion

History:

- Use list rows/cards, not dense tables.
- Each row shows status icon, process number, date/time, amount returned, and `Ver` action.

## Implementation Guidance For Razor Pages

- Keep Razor Pages server-rendered. Do not introduce SPA frameworks without S4 approval.
- Recreate the mock using local CSS and Razor partials/components where practical.
- Prefer shared layout/partials for header, footer, progress stepper, alert, large button, and result card.
- Keep styles in `src/Presentation/wwwroot/css/site.css` or existing Razor CSS files.
- Do not depend on the Lovable hosted CSS or JS at runtime.
- Do not include the Lovable badge.
- Use accessible labels, clear button text, and large touch targets.

## What Needs Source Code

The public mock is enough for layout, copy, routes, visual hierarchy, and UX rules.

Ask S4 for the Lovable/GitHub source export only if exact component parity is required, including:

- Exact Tailwind class composition.
- React component boundaries.
- Generated route code.
- Original icon/component imports.
- Asset files not visible in deployed HTML.
