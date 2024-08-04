import "./App.css";
import { Outlet } from "react-router-dom";
import Navbar from "./components/common/Navbar";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
  console.log("API Base URL:", import.meta.env.VITE_API_BASE_URL);

  return (
    <div>
      <Navbar />
      <div className="my-20 mx-2 px-4">
        <Outlet />
        <ToastContainer className={"z-50"} />
      </div>
    </div>
  );
}

export default App;
