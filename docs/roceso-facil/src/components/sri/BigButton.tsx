import type { ButtonHTMLAttributes, ReactNode } from "react";
import { Link } from "@tanstack/react-router";

type Variant = "primary" | "secondary" | "ghost" | "danger";

const base =
  "inline-flex items-center justify-center gap-2 rounded-xl px-6 py-4 text-lg font-semibold min-h-[56px] transition-colors disabled:opacity-50 disabled:cursor-not-allowed focus-visible:outline-none focus-visible:ring-4 focus-visible:ring-ring/40";

const variants: Record<Variant, string> = {
  primary:
    "bg-primary text-primary-foreground hover:bg-primary/90 shadow-md shadow-primary/20",
  secondary:
    "bg-secondary text-secondary-foreground hover:bg-secondary/80 border-2 border-border",
  ghost: "bg-transparent text-foreground hover:bg-secondary",
  danger: "bg-destructive text-destructive-foreground hover:bg-destructive/90",
};

export function BigButton({
  variant = "primary",
  className = "",
  children,
  ...rest
}: ButtonHTMLAttributes<HTMLButtonElement> & {
  variant?: Variant;
  children: ReactNode;
}) {
  return (
    <button className={`${base} ${variants[variant]} ${className}`} {...rest}>
      {children}
    </button>
  );
}

export function BigLink({
  to,
  variant = "primary",
  className = "",
  children,
}: {
  to: string;
  variant?: Variant;
  className?: string;
  children: ReactNode;
}) {
  return (
    <Link
      to={to}
      className={`${base} ${variants[variant]} no-underline ${className}`}
    >
      {children}
    </Link>
  );
}
