import { createFileRoute } from "@tanstack/react-router";
import { PageShell } from "@/components/sri/PageShell";
import { BigButton, BigLink } from "@/components/sri/BigButton";
import { Alert } from "@/components/sri/Alert";
import { CheckCircle2, Download, Image as ImageIcon } from "lucide-react";

export const Route = createFileRoute("/resultados")({
  head: () => ({
    meta: [
      { title: "Resultados — Proceso SRI" },
      { name: "description", content: "Resumen final de tu Devolución de IVA." },
    ],
  }),
  component: Resultados,
});

function Resultados() {
  const expected = 32.45;
  const refunded = 32.45;
  const diff = expected - refunded;

  return (
    <PageShell
      step={5}
      title="¡Listo! Tu trámite se completó"
      subtitle="Aquí tienes el resumen de tu Devolución de IVA."
    >
      <Alert tone="success" title="Trámite enviado correctamente">
        El SRI recibió tu solicitud. Guarda este reporte para tus registros.
      </Alert>

      <section className="mt-6 grid gap-4 sm:grid-cols-2">
        <div className="rounded-2xl border-2 border-border bg-card p-5">
          <div className="text-sm text-muted-foreground">Monto esperado</div>
          <div className="mt-1 text-3xl font-bold">${expected.toFixed(2)}</div>
        </div>
        <div className="rounded-2xl border-2 border-success bg-success/10 p-5">
          <div className="text-sm text-muted-foreground">Monto devuelto</div>
          <div className="mt-1 text-3xl font-bold text-success">
            ${refunded.toFixed(2)}
          </div>
        </div>
        <div className="rounded-2xl border-2 border-border bg-card p-5">
          <div className="text-sm text-muted-foreground">Diferencia</div>
          <div className="mt-1 text-2xl font-bold">${diff.toFixed(2)}</div>
        </div>
        <div className="rounded-2xl border-2 border-border bg-card p-5">
          <div className="text-sm text-muted-foreground">Número de trámite</div>
          <div className="mt-1 text-2xl font-bold">SRI-2026-04-0012</div>
          <div className="mt-1 text-sm text-muted-foreground">
            12 de abril de 2026, 10:42
          </div>
        </div>
      </section>

      <section className="mt-8">
        <h2 className="text-foreground">Evidencias</h2>
        <p className="mt-1 text-base text-muted-foreground">
          Capturas tomadas durante el trámite.
        </p>
        <ul className="mt-4 grid gap-3 sm:grid-cols-3">
          {["Inicio de sesión", "Envío de facturas", "Confirmación"].map((label) => (
            <li
              key={label}
              className="rounded-xl border-2 border-border bg-card p-3"
            >
              <div className="flex aspect-video items-center justify-center rounded-lg bg-secondary text-muted-foreground">
                <ImageIcon className="h-8 w-8" aria-hidden />
              </div>
              <div className="mt-2 text-base font-medium">{label}</div>
            </li>
          ))}
        </ul>
      </section>

      <div className="mt-10 flex flex-col gap-3 sm:flex-row sm:justify-between">
        <BigLink to="/" variant="secondary">
          Empezar nuevo proceso
        </BigLink>
        <BigButton>
          <Download className="h-5 w-5" aria-hidden />
          Guardar reporte
        </BigButton>
      </div>

      <div className="mt-6 flex items-center gap-2 text-base text-success">
        <CheckCircle2 className="h-5 w-5" aria-hidden />
        Reporte disponible para descargar (PDF / CSV).
      </div>
    </PageShell>
  );
}
