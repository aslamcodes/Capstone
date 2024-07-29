import "./App.css";
import { Outlet } from "react-router-dom";
import Navbar from "./components/common/Navbar";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function App() {
  return (
    <div>
      <Navbar />
      <div className="mt-20 px-4">
        <Outlet />
        <ToastContainer />
      </div>
    </div>
  );
}

export default App;
