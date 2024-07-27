import "./App.css";
import { Outlet } from "react-router-dom";
import Navbar from "./components/common/Navbar";

function App() {
  return (
    <div>
      <Navbar />
      <div className="mt-20 px-4">
        <Outlet />
      </div>
    </div>
  );
}

export default App;
