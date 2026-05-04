import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useEffect, useState } from "react";
import { PageShell } from "@/components/sri/PageShell";
import { BigButton } from "@/components/sri/BigButton";
import { Alert } from "@/components/sri/Alert";
import { Loader2, ChevronDown, ChevronUp, CheckCircle2 } from "lucide-react";

export const Route = createFileRoute("/progreso")({
  head: () => ({
    meta: [
      { title: "Progreso del trámite — Proceso SRI" },
      { name: "description", content: "Estado actual del trámite SRI." },
    ],
  }),
  component: Progreso,
});

const STAGES = [
  "Preparando información",
  "Iniciando sesión en el SRI",
  "Enviando facturas",
  "Confirmando trámite",
  "Finalizando",
];

const HUMAN_TEXT: Record<string, string> = {
  "Preparando información": "Estamos organizando tus facturas…",
  "Iniciando sesión en el SRI": "Estamos ingresando al SRI…",
  "Enviando facturas": "Estamos enviando tus facturas al SRI…",
  "Confirmando trámite": "Estamos confirmando tu trámite…",
  "Finalizando": "Casi terminamos…",
};

function Progreso() {
  const navigate = useNavigate();
  const [stage, setStage] = useState(0);
  const [showLogs, setShowLogs] = useState(false);

  // Maqueta: simulamos polling cada 3s
  useEffect(() => {
    if (stage >= STAGES.length) {
      const t = setTimeout(() => navigate({ to: "/resultados" }), 1200);
      return () => clearTimeout(t);
    }
    const t = setTimeout(() => setStage((s) => s + 1), 3000);
    return () => clearTimeout(t);
  }, [stage, navigate]);

  const currentStage = STAGES[Math.min(stage, STAGES.length - 1)];
  const pct = Math.round((Math.min(stage, STAGES.length) / STAGES.length) * 100);
  const done = stage >= STAGES.length;

  return (
    <PageShell
      step={5}
      title={done ? "¡Casi listo!" : "Procesando tu trámite"}
      subtitle={
        done
          ? "Estamos preparando tus resultados."
          : "Por favor espera. No cierres esta ventana."
      }
    >
      <section className="rounded-2xl border-2 border-border bg-card p-6 md:p-8">
        <div
          role="status"
          aria-live="polite"
          className="flex items-center gap-3"
        >
          {done ? (
            <CheckCircle2 className="h-7 w-7 text-success" aria-hidden />
          ) : (
            <Loader2 className="h-7 w-7 text-primary animate-spin" aria-hidden />
          )}
          <div className="text-xl font-semibold">
            {done ? "Trámite completado" : HUMAN_TEXT[currentStage]}
          </div>
        </div>

        <div className="mt-6">
          <div
            role="progressbar"
            aria-valuenow={pct}
            aria-valuemin={0}
            aria-valuemax={100}
            aria-label="Progreso del trámite"
            className="h-4 w-full overflow-hidden rounded-full bg-secondary"
          >
            <div
              className="h-full bg-primary transition-all duration-700"
              style={{ width: `${pct}%` }}
            />
          </div>
          <div className="mt-2 text-base text-muted-foreground">{pct}%</div>
        </div>

        <ol className="mt-8 space-y-3">
          {STAGES.map((label, idx) => {
            const status =
              idx < stage ? "done" : idx === stage ? "current" : "todo";
            return (
              <li key={label} className="flex items-center gap-3">
                <span
                  className={[
                    "flex h-8 w-8 items-center justify-center rounded-full text-sm font-bold border-2",
                    status === "done" && "bg-success text-success-foreground border-success",
                    status === "current" && "bg-primary text-primary-foreground border-primary",
                    status === "todo" && "bg-card text-muted-foreground border-border",
                  ].filter(Boolean).join(" ")}
                  aria-hidden
                >
                  {status === "done" ? "✓" : idx + 1}
                </span>
                <span
                  className={
                    status === "todo"
                      ? "text-base text-muted-foreground"
                      : "text-base font-medium text-foreground"
                  }
                >
                  {label}
                </span>
              </li>
            );
          })}
        </ol>

        <div className="mt-8">
          <button
            type="button"
            onClick={() => setShowLogs((v) => !v)}
            aria-expanded={showLogs}
            className="inline-flex items-center gap-2 text-base font-semibold text-primary hover:underline"
          >
            {showLogs ? <ChevronUp className="h-5 w-5" /> : <ChevronDown className="h-5 w-5" />}
            {showLogs ? "Ocultar detalles" : "Ver detalles"}
          </button>
          {showLogs && (
            <pre className="mt-3 max-h-64 overflow-auto rounded-xl bg-foreground/95 p-4 text-sm text-background font-mono">
{`[INFO] Job iniciado
[INFO] Preparando 4 facturas…
[INFO] Iniciando sesión SRI…
[INFO] Sesión OK
[INFO] Enviando lote 1/1…`}
            </pre>
          )}
        </div>
      </section>

      {!done && (
        <div className="mt-6">
          <Alert tone="info" title="Esto puede tardar varios minutos">
            Mantén la ventana abierta. Te avisaremos cuando esté listo.
          </Alert>
        </div>
      )}
    </PageShell>
  );
}
