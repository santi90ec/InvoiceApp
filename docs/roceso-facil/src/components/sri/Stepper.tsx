import { Check } from "lucide-react";

export type StepKey = 1 | 2 | 3 | 4 | 5;

const STEPS: { key: StepKey; label: string }[] = [
  { key: 1, label: "Carpeta" },
  { key: 2, label: "Revisar" },
  { key: 3, label: "Clasificar" },
  { key: 4, label: "Iniciar" },
  { key: 5, label: "Resultados" },
];

export function Stepper({ current }: { current: StepKey }) {
  return (
    <nav aria-label={`Paso ${current} de 5`} className="bg-card border-b border-border">
      <ol className="mx-auto flex max-w-5xl items-center gap-2 overflow-x-auto px-4 py-4 md:px-8">
        {STEPS.map((step, idx) => {
          const status =
            step.key < current ? "done" : step.key === current ? "current" : "todo";
          return (
            <li key={step.key} className="flex flex-1 items-center gap-2 min-w-fit">
              <div
                aria-current={status === "current" ? "step" : undefined}
                className={[
                  "flex h-12 w-12 shrink-0 items-center justify-center rounded-full text-base font-bold border-2 transition-colors",
                  status === "done" && "bg-success text-success-foreground border-success",
                  status === "current" &&
                    "bg-primary text-primary-foreground border-primary ring-4 ring-primary/20",
                  status === "todo" && "bg-card text-muted-foreground border-border",
                ]
                  .filter(Boolean)
                  .join(" ")}
              >
                {status === "done" ? <Check className="h-6 w-6" aria-hidden /> : step.key}
              </div>
              <div className="hidden md:block">
                <div
                  className={
                    status === "current"
                      ? "text-base font-semibold text-foreground"
                      : "text-base font-medium text-muted-foreground"
                  }
                >
                  Paso {step.key}
                </div>
                <div className="text-sm text-muted-foreground">{step.label}</div>
              </div>
              {idx < STEPS.length - 1 && (
                <div
                  aria-hidden="true"
                  className={[
                    "mx-1 h-1 flex-1 rounded-full",
                    step.key < current ? "bg-success" : "bg-border",
                  ].join(" ")}
                />
              )}
            </li>
          );
        })}
      </ol>
    </nav>
  );
}
