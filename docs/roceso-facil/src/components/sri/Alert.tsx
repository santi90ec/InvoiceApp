import { AlertCircle, CheckCircle2, Info, AlertTriangle } from "lucide-react";
import type { ReactNode } from "react";

type Tone = "info" | "success" | "warning" | "error";

const styles: Record<Tone, string> = {
  info: "bg-secondary border-primary/40 text-foreground",
  success: "bg-success/10 border-success text-foreground",
  warning: "bg-warning/15 border-warning text-foreground",
  error: "bg-destructive/10 border-destructive text-foreground",
};

const Icons: Record<Tone, typeof Info> = {
  info: Info,
  success: CheckCircle2,
  warning: AlertTriangle,
  error: AlertCircle,
};

export function Alert({
  tone = "info",
  title,
  children,
}: {
  tone?: Tone;
  title: string;
  children?: ReactNode;
}) {
  const Icon = Icons[tone];
  return (
    <div
      role={tone === "error" ? "alert" : "status"}
      className={`flex gap-3 rounded-xl border-2 p-4 ${styles[tone]}`}
    >
      <Icon className="h-6 w-6 shrink-0 mt-0.5" aria-hidden />
      <div>
        <div className="text-base font-semibold">{title}</div>
        {children && <div className="mt-1 text-base">{children}</div>}
      </div>
    </div>
  );
}
