import App from "../App";
import Apitest from "./Apitest";
import LoginPage from "./login";

export const pages = [
  {
    path: "/",
    element: <App />,
  },
  {
    path: "/login",
    element: <LoginPage />,
  },
];
