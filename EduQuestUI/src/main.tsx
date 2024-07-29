import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "./index.css";
import { pages } from "./pages/PagesConfig.tsx";
import AuthContextProvider from "./contexts/auth/authProvider.tsx";
import { SWRConfig } from "swr";
import { fetcher } from "./utils/fetcher.ts";
import { ErrorBoundary } from "react-error-boundary";
import ErrorPage from "./pages/ErrorPage.tsx";

const router = createBrowserRouter(pages);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <AuthContextProvider>
    <ErrorBoundary fallback={<ErrorPage />}>
      <SWRConfig
        value={{
          fetcher,
        }}
      >
        <RouterProvider router={router} />
      </SWRConfig>
    </ErrorBoundary>
  </AuthContextProvider>
);
