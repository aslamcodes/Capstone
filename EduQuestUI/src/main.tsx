import React, { StrictMode } from "react";
import ReactDOM from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import "./index.css";
import { pages } from "./pages/PagesConfig.tsx";
import AuthContextProvider from "./contexts/auth/authProvider.tsx";
import { SWRConfig } from "swr";
import { fetcher } from "./utils/fetcher.ts";

const router = createBrowserRouter(pages);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <AuthContextProvider>
      <SWRConfig
        value={{
          fetcher,
        }}
      >
        <RouterProvider router={router} />
      </SWRConfig>
    </AuthContextProvider>
  </StrictMode>
);
