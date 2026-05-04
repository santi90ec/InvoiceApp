import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useState } from "react";
import { PageShell } from "@/components/sri/PageShell";
import { BigButton, BigLink } from "@/components/sri/BigButton";
import { Alert } from "@/components/sri/Alert";
import { ArrowRight } from "lucide-react";

export const Route = createFileRoute("/paso-3")({
  head: () => ({
    meta: [
      { title: "Paso 3: Clasificar facturas — Proceso SRI" },
      { name: "description", content: "Revisa la categoría de cada factura." },
    ],
  }),
  component: Paso3,
});

const CATEGORIES = [
  "Sin clasificar",
  "Medicinas",
  "Alimentación",
  "Educación",
  "Vivienda",
  "Salud",
  "Vestimenta",
] as const;

type Cat = (typeof CATEGORIES)[number];

const INITIAL: { id: string; provider: string; total: number; cat: Cat }[] = [
  { id: "F-001", provider: "Farmacia Sana", total: 24.5, cat: "Medicinas" },
  { id: "F-002", provider: "Supermaxi", total: 87.32, cat: "Alimentación" },
  { id: "F-003", provider: "Clínica Vida", total: 120.0, cat: "Salud" },
  { id: "F-004", provider: "Restaurante El Sol", total: 18.75, cat: "Sin clasificar" },
];

function Paso3() {
  const navigate = useNavigate();
  const [rows, setRows] = useState(INITIAL);
  const [error, setError] = useState<string | null>(null);

  const missing = rows.filter((r) => r.cat === "Sin clasificar").length;

  function onContinue() {
    if (missing > 0) {
      setError(
        `Tienes ${missing} factura(s) sin clasificar. Elige una categoría para cada una.`,
      );
      return;
    }
    setError(null);
    navigate({ to: "/paso-4" });
  }

  return (
    <PageShell
      step={3}
      title="Revisa la clasificación"
      subtitle="Te sugerimos una categoría para cada factura. Cámbiala si no es correcta."
    >
      {error && (
        <div className="mb-6">
          <Alert tone="error" title="Falta clasificar">{error}</Alert>
        </div>
      )}

      <ul className="space-y-3" aria-label="Facturas a clasificar">
        {rows.map((r, idx) => (
          <li
            key={r.id}
            className="rounded-2xl border-2 border-border bg-card p-4 md:p-5"
          >
            <div className="grid gap-3 md:grid-cols-[1fr_auto_minmax(220px,260px)] md:items-center">
              <div>
                <div className="text-lg font-semibold">{r.provider}</div>
                <div className="text-sm text-muted-foreground">Factura {r.id}</div>
              </div>
              <div className="text-lg font-bold md:text-right">
                ${r.total.toFixed(2)}
              </div>
              <div>
                <label
                  htmlFor={`cat-${r.id}`}
                  className="block text-sm font-medium text-muted-foreground"
                >
                  Categoría
                </label>
                <select
                  id={`cat-${r.id}`}
                  value={r.cat}
                  onChange={(e) => {
                    const next = [...rows];
                    next[idx] = { ...r, cat: e.target.value as Cat };
                    setRows(next);
                  }}
                  className="mt-1 w-full rounded-xl border-2 border-input bg-background px-3 py-3 text-lg focus-visible:outline-none focus-visible:border-primary"
                >
                  {CATEGORIES.map((c) => (
                    <option key={c} value={c}>{c}</option>
                  ))}
                </select>
              </div>
            </div>
          </li>
        ))}
      </ul>

      <div className="mt-8 flex flex-col gap-3 sm:flex-row sm:justify-between">
        <BigLink to="/paso-2" variant="ghost">← Volver</BigLink>
        <BigButton onClick={onContinue}>
          Confirmar y continuar <ArrowRight className="h-5 w-5" aria-hidden />
        </BigButton>
      </div>
    </PageShell>
  );
}
