import App from "../App";
import Apitest from "./Apitest";
import LoginPage from "./auth/login";
import RegisterPage from "./auth/register";

export const pages = [
  {
    path: "/",
    element: <App />,
  },
  {
    path: "/login",
    element: <LoginPage />,
  },
  {
    path: "/register",
    element: <RegisterPage />,
  },
];
