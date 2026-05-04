import { createFileRoute } from "@tanstack/react-router";
import { PageShell } from "@/components/sri/PageShell";
import { BigLink } from "@/components/sri/BigButton";
import { CheckCircle2, AlertCircle, ChevronRight } from "lucide-react";

export const Route = createFileRoute("/historial")({
  head: () => ({
    meta: [
      { title: "Historial — Proceso SRI" },
      { name: "description", content: "Tus ejecuciones anteriores." },
    ],
  }),
  component: Historial,
});

const RUNS = [
  { id: "SRI-2026-04-0012", date: "12/04/2026 10:42", status: "ok" as const, amount: 32.45 },
  { id: "SRI-2026-03-0007", date: "08/03/2026 16:10", status: "ok" as const, amount: 41.20 },
  { id: "SRI-2026-02-0003", date: "14/02/2026 09:25", status: "error" as const, amount: 0 },
];

function Historial() {
  return (
    <PageShell
      title="Historial"
      subtitle="Estas son tus solicitudes anteriores."
    >
      {RUNS.length === 0 ? (
        <div className="rounded-2xl border-2 border-border bg-card p-8 text-center">
          <p className="text-lg">Todavía no has realizado ningún trámite.</p>
          <div className="mt-6">
            <BigLink to="/paso-1">Empezar mi primer proceso</BigLink>
          </div>
        </div>
      ) : (
        <ul className="space-y-3">
          {RUNS.map((r) => (
            <li key={r.id}>
              <div className="flex items-center gap-4 rounded-2xl border-2 border-border bg-card p-5">
                {r.status === "ok" ? (
                  <CheckCircle2 className="h-8 w-8 text-success shrink-0" aria-hidden />
                ) : (
                  <AlertCircle className="h-8 w-8 text-destructive shrink-0" aria-hidden />
                )}
                <div className="flex-1 min-w-0">
                  <div className="text-lg font-semibold truncate">{r.id}</div>
                  <div className="text-base text-muted-foreground">{r.date}</div>
                </div>
                <div className="hidden sm:block text-right">
                  <div className="text-sm text-muted-foreground">Devuelto</div>
                  <div className="text-lg font-bold">
                    ${r.amount.toFixed(2)}
                  </div>
                </div>
                <BigLink
                  to="/resultados"
                  variant="secondary"
                  className="!px-4 !py-3 !text-base !min-h-[48px]"
                >
                  Ver <ChevronRight className="h-5 w-5" aria-hidden />
                </BigLink>
              </div>
            </li>
          ))}
        </ul>
      )}
    </PageShell>
  );
}
