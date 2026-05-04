import { createFileRoute } from "@tanstack/react-router";
import { PageShell } from "@/components/sri/PageShell";
import { BigLink } from "@/components/sri/BigButton";
import { Alert } from "@/components/sri/Alert";
import { ArrowRight, FileText } from "lucide-react";

export const Route = createFileRoute("/")({
  head: () => ({
    meta: [
      { title: "Inicio — Proceso SRI" },
      {
        name: "description",
        content: "Empieza tu Devolución de IVA en pocos pasos guiados.",
      },
    ],
  }),
  component: Home,
});

function Home() {
  // Maqueta: simulamos una "última ejecución" para mostrar la tarjeta
  const lastRun = {
    date: "12 de abril de 2026",
    status: "Completado" as const,
  };

  return (
    <PageShell
      title="Bienvenido"
      subtitle="Te ayudamos a hacer tu Devolución de IVA, paso a paso."
    >
      <div className="grid gap-6">
        <section className="rounded-2xl border-2 border-border bg-card p-6 md:p-8 shadow-sm">
          <h2 className="text-foreground">¿Qué hace esta aplicación?</h2>
          <p className="mt-3 text-lg text-foreground">
            Esta aplicación te guía para enviar tus facturas electrónicas al SRI
            y solicitar tu Devolución de IVA. No necesitas conocimientos
            técnicos: te llevamos paso a paso.
          </p>
          <div className="mt-6 flex flex-col gap-3 sm:flex-row">
            <BigLink to="/paso-1">
              Empezar Proceso <ArrowRight className="h-5 w-5" aria-hidden />
            </BigLink>
          </div>
        </section>

        {lastRun && (
          <section
            aria-labelledby="last-run-title"
            className="rounded-2xl border-2 border-border bg-secondary/40 p-6"
          >
            <div className="flex items-start gap-4">
              <FileText className="h-8 w-8 text-primary mt-1" aria-hidden />
              <div className="flex-1">
                <h2 id="last-run-title" className="text-foreground">
                  Última ejecución
                </h2>
                <p className="mt-1 text-base text-muted-foreground">
                  Fecha: {lastRun.date}
                </p>
                <p className="mt-1 text-base">
                  Estado:{" "}
                  <span className="font-semibold text-success">
                    {lastRun.status}
                  </span>
                </p>
                <div className="mt-4">
                  <BigLink to="/resultados" variant="secondary">
                    Ver resultados
                  </BigLink>
                </div>
              </div>
            </div>
          </section>
        )}

        <Alert tone="info" title="Tus datos están seguros">
          Esta aplicación funciona en tu computadora. No guardamos tus
          contraseñas.
        </Alert>
      </div>
    </PageShell>
  );
}
