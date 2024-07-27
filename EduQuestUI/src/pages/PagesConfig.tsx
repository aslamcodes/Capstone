import App from "../App";
import LoginPage from "./auth/login";
import RegisterPage from "./auth/register";
import MyCourses from "./course/MyCourse";
import CoursePage from "./course/CoursePage";
import Educator from "./Educator/Educator";

export const pages = [
  {
    path: "/",
    element: <App />,
    children: [
      { path: "/myCourses", element: <MyCourses /> },
      { path: "/myCourses/:courseId", element: <CoursePage /> },
      { path: "/Educator", element: <Educator /> },
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
