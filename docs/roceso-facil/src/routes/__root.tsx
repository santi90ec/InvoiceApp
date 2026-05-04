import { Outlet, Link, createRootRoute, HeadContent, Scripts } from "@tanstack/react-router";

import appCss from "../styles.css?url";

function NotFoundComponent() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-background px-4">
      <div className="max-w-md text-center">
        <h1 className="text-7xl font-bold text-foreground">404</h1>
        <h2 className="mt-4 text-xl font-semibold text-foreground">Page not found</h2>
        <p className="mt-2 text-sm text-muted-foreground">
          The page you're looking for doesn't exist or has been moved.
        </p>
        <div className="mt-6">
          <Link
            to="/"
            className="inline-flex items-center justify-center rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground transition-colors hover:bg-primary/90"
          >
            Go home
          </Link>
        </div>
      </div>
    </div>
  );
}

export const Route = createRootRoute({
  head: () => ({
    meta: [
      { charSet: "utf-8" },
      { name: "viewport", content: "width=device-width, initial-scale=1" },
      { title: "Proceso SRI — Devolución de IVA, paso a paso" },
      {
        name: "description",
        content:
          "Aplicación local que te guía paso a paso para hacer tu Devolución de IVA en el SRI, de forma simple y segura.",
      },
      { name: "author", content: "Proceso SRI" },
      { property: "og:title", content: "Proceso SRI — Devolución de IVA, paso a paso" },
      {
        property: "og:description",
        content: "Te ayudamos paso a paso a recuperar tu IVA.",
      },
      { property: "og:type", content: "website" },
      { name: "twitter:title", content: "Proceso SRI — Devolución de IVA, paso a paso" },
      { name: "description", content: "SRI Process Helper guides users through tax return processes with a simple, step-by-step interface." },
      { property: "og:description", content: "SRI Process Helper guides users through tax return processes with a simple, step-by-step interface." },
      { name: "twitter:description", content: "SRI Process Helper guides users through tax return processes with a simple, step-by-step interface." },
      { property: "og:image", content: "https://pub-bb2e103a32db4e198524a2e9ed8f35b4.r2.dev/0f0eb5bb-f193-4bd1-a3a9-0d9fceae8369/id-preview-3f81c974--d0af7388-cb23-42f1-a4c9-9532dd4102b2.lovable.app-1777935014694.png" },
      { name: "twitter:image", content: "https://pub-bb2e103a32db4e198524a2e9ed8f35b4.r2.dev/0f0eb5bb-f193-4bd1-a3a9-0d9fceae8369/id-preview-3f81c974--d0af7388-cb23-42f1-a4c9-9532dd4102b2.lovable.app-1777935014694.png" },
      { name: "twitter:card", content: "summary_large_image" },
    ],
    links: [
      {
        rel: "stylesheet",
        href: appCss,
      },
    ],
  }),
  shellComponent: RootShell,
  component: RootComponent,
  notFoundComponent: NotFoundComponent,
});

function RootShell({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <head>
        <HeadContent />
      </head>
      <body>
        {children}
        <Scripts />
      </body>
    </html>
  );
}

function RootComponent() {
  return <Outlet />;
}
