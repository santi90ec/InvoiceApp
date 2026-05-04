import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useState } from "react";
import { PageShell } from "@/components/sri/PageShell";
import { BigButton, BigLink } from "@/components/sri/BigButton";
import { Alert } from "@/components/sri/Alert";
import { ShieldCheck, Play } from "lucide-react";

export const Route = createFileRoute("/paso-4")({
  head: () => ({
    meta: [
      { title: "Paso 4: Iniciar trámite — Proceso SRI" },
      { name: "description", content: "Confirma e inicia el trámite en el SRI." },
    ],
  }),
  component: Paso4,
});

function Paso4() {
  const navigate = useNavigate();
  const [user, setUser] = useState("");
  const [pass, setPass] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  function onStart(e: React.FormEvent) {
    e.preventDefault();
    setError(null);
    if (!user.trim() || !pass.trim()) {
      setError("Ingresa tu usuario y contraseña del SRI para continuar.");
      return;
    }
    setSubmitting(true);
    setTimeout(() => navigate({ to: "/progreso" }), 600);
  }

  return (
    <PageShell
      step={4}
      title="Vamos a iniciar el trámite"
      subtitle="Esto puede tardar unos minutos. No cierres la aplicación durante el proceso."
      help={
        <>
          <strong>¿Por qué pedimos tu usuario?</strong>
          <p className="mt-1">
            La aplicación necesita ingresar al portal del SRI con tus datos.{" "}
            <strong>No guardamos tu contraseña.</strong> Solo se usa para esta
            ejecución.
          </p>
        </>
      }
    >
      <form
        onSubmit={onStart}
        className="rounded-2xl border-2 border-border bg-card p-6 md:p-8 space-y-6"
        noValidate
      >
        <div className="flex items-start gap-3 rounded-xl bg-primary/5 p-4 border border-primary/30">
          <ShieldCheck className="h-6 w-6 text-primary mt-0.5" aria-hidden />
          <p className="text-base">
            Tus datos solo se usan para iniciar sesión en el SRI durante este
            trámite.
          </p>
        </div>

        <div>
          <label htmlFor="user" className="block text-lg font-semibold">
            Usuario del SRI (cédula o RUC)
          </label>
          <input
            id="user"
            type="text"
            autoComplete="username"
            value={user}
            onChange={(e) => setUser(e.target.value)}
            className="mt-2 w-full rounded-xl border-2 border-input bg-background px-4 py-4 text-lg focus-visible:outline-none focus-visible:border-primary"
          />
        </div>

        <div>
          <label htmlFor="pass" className="block text-lg font-semibold">
            Contraseña del SRI
          </label>
          <input
            id="pass"
            type="password"
            autoComplete="current-password"
            value={pass}
            onChange={(e) => setPass(e.target.value)}
            className="mt-2 w-full rounded-xl border-2 border-input bg-background px-4 py-4 text-lg focus-visible:outline-none focus-visible:border-primary"
          />
        </div>

        {error && <Alert tone="error" title="Faltan datos">{error}</Alert>}

        <div className="flex flex-col gap-3 sm:flex-row sm:justify-between">
          <BigLink to="/paso-3" variant="ghost">← Volver</BigLink>
          <BigButton type="submit" disabled={submitting}>
            <Play className="h-5 w-5" aria-hidden />
            {submitting ? "Iniciando…" : "Iniciar trámite"}
          </BigButton>
        </div>
      </form>
    </PageShell>
  );
}
