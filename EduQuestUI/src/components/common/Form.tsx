import React, { FC, FormHTMLAttributes, PropsWithChildren } from "react";

interface FormProps extends FormHTMLAttributes<HTMLFormElement> {}

const Form: FC<PropsWithChildren<FormProps>> = ({ children, ...props }) => {
  return (
    <form
      className="flex flex-col gap-5 max-w-screen-lg rounded-box p-6"
      {...props}
    >
      {children}
    </form>
  );
};

export const FormTitle: FC<PropsWithChildren> = ({ children }) => {
  return <h1 className="text-2xl font-bold ">{children}</h1>;
};

export const FormControl: FC<PropsWithChildren> = ({ children }) => {
  return <div>{children}</div>;
};

export const FormGroup: FC<PropsWithChildren<{ row?: boolean }>> = ({
  children,
  row = false,
}) => {
  return <div className={`flex ${!row && "flex-col"} gap-2`}>{children}</div>;
};

export const FormLabel: FC<PropsWithChildren> = ({ children }) => {
  return <label className="font-semibold">{children}</label>;
};

export const FormButton: FC<{
  title: string;
  className?: string;
  type?: "submit" | "reset" | "button";
}> = ({ title, className, type = "submit" }) => {
  return (
    <button type={type} className={`btn ${className}`}>
      {title}
    </button>
  );
};

export const FormError: FC<{ message: string }> = () => {
  return <div />;
};

export default Form;
