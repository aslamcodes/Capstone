import App from "../App";
import LoginPage from "./auth/login";
import RegisterPage from "./auth/register";
import MyCourses from "./course/MyCourse";
import CoursePage from "./course/CoursePage";
import Educator from "./Educator/Educator";
import ManageCoursePage from "./Educator/ManageCoursePage";
import AritlceEditor from "../components/educators/AritlceEditor";

export const pages = [
  {
    path: "/",
    element: <App />,
    children: [
      { path: "/myCourses", element: <MyCourses /> },
      { path: "/myCourses/:courseId", element: <CoursePage /> },
      {
        path: "/manage-course/content/article/:contentId",
        element: <AritlceEditor />,
      },
      { path: "/manage-course/:courseId", element: <ManageCoursePage /> },
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
