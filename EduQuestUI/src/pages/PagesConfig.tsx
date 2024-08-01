import App from "../App";
import LoginPage from "./auth/login";
import RegisterPage from "./auth/register";
import MyCourses from "./course/MyCourse";
import CoursePage from "./course/CoursePage";
import Educator from "./Educator/Educator";
import ManageCoursePage from "./Educator/ManageCoursePage";
import AritlceEditor from "../components/educators/AritlceEditor";
import Home from "./Home";
import CourseLanding from "../components/Course/CourseLanding";
import OrderPage from "../components/order/OrderPage";
import UserProfile from "./user-profile";
import Admin from "./Admin";
import UserOrders from "./user-orders";

export const pages = [
  {
    path: "/",
    element: <App />,
    children: [
      { path: "/", element: <Home /> },
      { path: "/course-description/:courseId", element: <CourseLanding /> },
      { path: "/order/:orderId", element: <OrderPage /> },
      { path: "/myCourses", element: <MyCourses /> },
      { path: "/myCourses/:courseId", element: <CoursePage /> },
      { path: "/profile", element: <UserProfile /> },
      { path: "/admin", element: <Admin /> },
      { path: "/orders", element: <UserOrders /> },
      {
        path: "/manage-course/content/article/:contentId",
        element: <AritlceEditor />,
      },
      { path: "/manage-course/:courseId", element: <ManageCoursePage /> },
      { path: "/Educator", element: <Educator /> },
      {
        path: "/login",
        element: <LoginPage />,
      },
      {
        path: "/register",
        element: <RegisterPage />,
      },
    ],
  },
];
