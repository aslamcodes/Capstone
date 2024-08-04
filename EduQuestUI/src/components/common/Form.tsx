import { FC, FormHTMLAttributes, PropsWithChildren } from "react";

interface FormProps extends FormHTMLAttributes<HTMLFormElement> {}

const Form: FC<PropsWithChildren<FormProps>> = ({ children, ...props }) => {
  return (
    <form className="space-y-5 max-w-screen-lg rounded-box md:p-6" {...props}>
      {children}
    </form>
  );
};

export const FormTitle: FC<PropsWithChildren> = ({ children }) => {
  return <h1 className="text-xl md:text-2xl font-bold ">{children}</h1>;
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

export const FormButton: FC<
  PropsWithChildren<{
    title?: string;
    className?: string;
    type?: "submit" | "reset" | "button";
    onClick?: () => void;
    disabled?: boolean;
  }>
> = ({
  title,
  className,
  type = "submit",
  onClick,
  disabled = false,
  children,
}) => {
  return (
    <button
      type={type}
      className={`btn ${className}`}
      onClick={onClick}
      disabled={disabled}
    >
      {title || children}
    </button>
  );
};

export const FormError: FC<{ message: string }> = ({ message }) => {
  return <span className="text-error">{message}</span>;
};

export default Form;
