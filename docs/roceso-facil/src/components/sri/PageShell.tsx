import type { ReactNode } from "react";
import { AppHeader } from "./AppHeader";
import { Stepper, type StepKey } from "./Stepper";

export function PageShell({
  step,
  title,
  subtitle,
  children,
  help,
}: {
  step?: StepKey;
  title: string;
  subtitle?: string;
  children: ReactNode;
  help?: ReactNode;
}) {
  return (
    <div className="min-h-screen bg-background">
      <AppHeader />
      {step && <Stepper current={step} />}
      <main className="mx-auto max-w-5xl px-4 py-8 md:px-8 md:py-12">
        <div className="mb-8">
          <h1 className="text-foreground">{title}</h1>
          {subtitle && (
            <p className="mt-2 text-lg text-muted-foreground">{subtitle}</p>
          )}
        </div>
        {children}
        {help && (
          <aside
            className="mt-10 rounded-2xl border-2 border-accent bg-accent/40 p-5 text-base text-accent-foreground"
            aria-label="Ayuda"
          >
            {help}
          </aside>
        )}
        <footer className="mt-12 border-t border-border pt-6 text-sm text-muted-foreground">
          ¿Necesitas ayuda? Llama a tu familiar o contador de confianza.
        </footer>
      </main>
    </div>
  );
}
