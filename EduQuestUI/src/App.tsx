import { useState } from "react";
import "./App.css";
import { FaArrowAltCircleDown } from "react-icons/fa";

function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <FaArrowAltCircleDown />
      <button className="btn">Hello Daisy</button>
    </>
  );
}

export default App;
