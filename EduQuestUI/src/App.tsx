import { useState } from "react";
import "./App.css";
import { FaArrowAltCircleDown } from "react-icons/fa";
import { Outlet } from "react-router-dom";
import Navbar from "./components/common/Navbar";

function App() {
  const [count, setCount] = useState(0);

  return (
    <div>
      <Navbar />
      <Outlet />
    </div>
  );
}

export default App;
