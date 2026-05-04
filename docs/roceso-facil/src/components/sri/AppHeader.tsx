import { Link } from "@tanstack/react-router";

export function AppHeader() {
  return (
    <header className="border-b-2 border-border bg-card">
      <div className="mx-auto flex max-w-5xl items-center justify-between px-4 py-4 md:px-8">
        <Link to="/" className="flex items-center gap-3 text-foreground no-underline">
          <div
            aria-hidden="true"
            className="flex h-12 w-12 items-center justify-center rounded-xl bg-primary text-primary-foreground text-xl font-bold"
          >
            SRI
          </div>
          <div>
            <div className="text-xl font-bold leading-tight">Proceso SRI</div>
            <div className="text-sm text-muted-foreground">Devolución de IVA</div>
          </div>
        </Link>
        <nav aria-label="Ayuda">
          <Link
            to="/historial"
            className="rounded-lg px-4 py-2 text-base font-semibold text-foreground hover:bg-secondary"
          >
            Historial
          </Link>
        </nav>
      </div>
    </header>
  );
}
