import { createFileRoute } from "@tanstack/react-router";
import { PageShell } from "@/components/sri/PageShell";
import { BigLink } from "@/components/sri/BigButton";
import { CheckCircle2, AlertCircle, ArrowRight } from "lucide-react";

export const Route = createFileRoute("/paso-2")({
  head: () => ({
    meta: [
      { title: "Paso 2: Revisar facturas — Proceso SRI" },
      { name: "description", content: "Revisa las facturas encontradas." },
    ],
  }),
  component: Paso2,
});

type Invoice = {
  id: string;
  date: string;
  provider: string;
  total: number;
  status: "ok" | "error";
};

const INVOICES: Invoice[] = [
  { id: "F-001", date: "03/02/2026", provider: "Farmacia Sana", total: 24.5, status: "ok" },
  { id: "F-002", date: "10/02/2026", provider: "Supermaxi", total: 87.32, status: "ok" },
  { id: "F-003", date: "15/02/2026", provider: "Clínica Vida", total: 120.0, status: "ok" },
  { id: "F-004", date: "22/02/2026", provider: "Restaurante El Sol", total: 18.75, status: "ok" },
  { id: "F-005", date: "28/02/2026", provider: "Desconocido", total: 0, status: "error" },
];

const total = INVOICES.reduce((s, i) => s + i.total, 0);

function StatusBadge({ status }: { status: Invoice["status"] }) {
  if (status === "ok") {
    return (
      <span className="inline-flex items-center gap-1 rounded-full bg-success/15 px-3 py-1 text-sm font-semibold text-success">
        <CheckCircle2 className="h-4 w-4" aria-hidden /> Lista
      </span>
    );
  }
  return (
    <span className="inline-flex items-center gap-1 rounded-full bg-destructive/15 px-3 py-1 text-sm font-semibold text-destructive">
      <AlertCircle className="h-4 w-4" aria-hidden /> Con error
    </span>
  );
}

function Paso2() {
  return (
    <PageShell
      step={2}
      title="Encontramos tus facturas"
      subtitle="Revisa el resumen y continúa cuando estés listo."
    >
      <section className="grid gap-4 sm:grid-cols-3 mb-8">
        <div className="rounded-2xl border-2 border-border bg-card p-5">
          <div className="text-sm text-muted-foreground">Total de facturas</div>
          <div className="mt-1 text-3xl font-bold">{INVOICES.length}</div>
        </div>
        <div className="rounded-2xl border-2 border-border bg-card p-5">
          <div className="text-sm text-muted-foreground">Periodo</div>
          <div className="mt-1 text-2xl font-bold">Febrero 2026</div>
        </div>
        <div className="rounded-2xl border-2 border-primary bg-primary/5 p-5">
          <div className="text-sm text-muted-foreground">Monto total</div>
          <div className="mt-1 text-3xl font-bold text-primary">
            ${total.toFixed(2)}
          </div>
        </div>
      </section>

      {/* Mobile cards */}
      <ul className="space-y-3 md:hidden" aria-label="Facturas">
        {INVOICES.map((inv) => (
          <li
            key={inv.id}
            className="rounded-2xl border-2 border-border bg-card p-4"
          >
            <div className="flex items-start justify-between gap-3">
              <div>
                <div className="text-base font-semibold">{inv.provider}</div>
                <div className="text-sm text-muted-foreground">{inv.date}</div>
              </div>
              <div className="text-right">
                <div className="text-lg font-bold">${inv.total.toFixed(2)}</div>
                <div className="mt-1"><StatusBadge status={inv.status} /></div>
              </div>
            </div>
          </li>
        ))}
      </ul>

      {/* Desktop table */}
      <div className="hidden md:block overflow-hidden rounded-2xl border-2 border-border bg-card">
        <table className="w-full text-left">
          <caption className="sr-only">Lista de facturas encontradas</caption>
          <thead className="bg-secondary">
            <tr>
              <th scope="col" className="px-5 py-3 text-base">Fecha</th>
              <th scope="col" className="px-5 py-3 text-base">Proveedor</th>
              <th scope="col" className="px-5 py-3 text-base text-right">Total</th>
              <th scope="col" className="px-5 py-3 text-base">Estado</th>
            </tr>
          </thead>
          <tbody>
            {INVOICES.map((inv) => (
              <tr key={inv.id} className="border-t border-border">
                <td className="px-5 py-4 text-base">{inv.date}</td>
                <td className="px-5 py-4 text-base">{inv.provider}</td>
                <td className="px-5 py-4 text-base text-right font-semibold">
                  ${inv.total.toFixed(2)}
                </td>
                <td className="px-5 py-4"><StatusBadge status={inv.status} /></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <div className="mt-8 flex flex-col gap-3 sm:flex-row sm:justify-between">
        <BigLink to="/paso-1" variant="ghost">
          ← Volver a elegir carpeta
        </BigLink>
        <BigLink to="/paso-3">
          Clasificar automáticamente <ArrowRight className="h-5 w-5" aria-hidden />
        </BigLink>
      </div>
    </PageShell>
  );
}
