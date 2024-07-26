import { Children } from "react";
import App from "../App";
import Apitest from "./Apitest";
import LoginPage from "./auth/login";
import RegisterPage from "./auth/register";
import MyCourses from "./course/MyCourse";
import Course from "./course/Course";

export const pages = [
  {
    path: "/",
    element: <App />,
    children: [
      { path: "/myCourses", element: <MyCourses /> },
      { path: "/myCourses/:courseId", element: <Course /> },
    ],
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
