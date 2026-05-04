import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useState } from "react";
import { PageShell } from "@/components/sri/PageShell";
import { BigButton, BigLink } from "@/components/sri/BigButton";
import { Alert } from "@/components/sri/Alert";
import { Folder, ArrowRight } from "lucide-react";

export const Route = createFileRoute("/paso-1")({
  head: () => ({
    meta: [
      { title: "Paso 1: Elegir carpeta — Proceso SRI" },
      {
        name: "description",
        content: "Selecciona la carpeta donde están tus facturas XML.",
      },
    ],
  }),
  component: Paso1,
});

function Paso1() {
  const navigate = useNavigate();
  const [path, setPath] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  function onSubmit(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    if (!path.trim()) {
      setError("Por favor escribe o elige la carpeta donde están tus facturas.");
      return;
    }
    // Maqueta: simulamos carga
    setLoading(true);
    setTimeout(() => {
      setLoading(false);
      navigate({ to: "/paso-2" });
    }, 800);
  }

  return (
    <PageShell
      step={1}
      title="Elige la carpeta de tus facturas"
      subtitle="Necesitamos saber dónde guardaste los archivos XML que te envió el SRI."
      help={
        <>
          <strong>¿Dónde están mis facturas?</strong>
          <p className="mt-1">
            Tus archivos XML suelen estar en la carpeta <em>Descargas</em> o{" "}
            <em>Documentos</em>. Si no estás seguro, pide ayuda a un familiar.
          </p>
        </>
      }
    >
      <form
        onSubmit={onSubmit}
        className="rounded-2xl border-2 border-border bg-card p-6 md:p-8 space-y-6"
        noValidate
      >
        <div>
          <label
            htmlFor="folder"
            className="block text-lg font-semibold text-foreground"
          >
            Carpeta de facturas
          </label>
          <p className="mt-1 text-base text-muted-foreground">
            Ejemplo: C:\Users\TuNombre\Descargas\Facturas
          </p>
          <div className="mt-3 flex flex-col gap-3 sm:flex-row">
            <input
              id="folder"
              type="text"
              value={path}
              onChange={(e) => setPath(e.target.value)}
              placeholder="Pega aquí la ruta de la carpeta"
              aria-invalid={!!error}
              aria-describedby={error ? "folder-error" : undefined}
              className="flex-1 rounded-xl border-2 border-input bg-background px-4 py-4 text-lg text-foreground placeholder:text-muted-foreground focus-visible:outline-none focus-visible:border-primary"
            />
            <BigButton
              type="button"
              variant="secondary"
              onClick={() => setPath("C:\\Users\\Maria\\Descargas\\Facturas")}
            >
              <Folder className="h-5 w-5" aria-hidden /> Elegir carpeta
            </BigButton>
          </div>
        </div>

        {error && (
          <div id="folder-error">
            <Alert tone="error" title="Revisa la carpeta">
              {error}
            </Alert>
          </div>
        )}

        <div className="flex flex-col gap-3 sm:flex-row sm:justify-between">
          <BigLink to="/" variant="ghost">
            ← Volver al inicio
          </BigLink>
          <BigButton type="submit" disabled={loading}>
            {loading ? "Cargando facturas…" : "Cargar facturas"}
            {!loading && <ArrowRight className="h-5 w-5" aria-hidden />}
          </BigButton>
        </div>
      </form>
    </PageShell>
  );
}
