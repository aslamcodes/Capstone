import { FC } from "react";

interface LoaderProps {
  type?: "spinner" | "dots" | "ring" | "bars";
  size?: "sm" | "md" | "lg";
}

const Loader: FC<LoaderProps> = ({ type = "spinner", size = "lg" }) => {
  return <span className={`loading loading-${type} loading-${size}`}></span>;
};

export default Loader;
