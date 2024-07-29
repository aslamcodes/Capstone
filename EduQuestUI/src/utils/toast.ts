import { toast, ToastOptions } from "react-toastify";

export const customToast = (message: string, options?: ToastOptions) => {
  toast(message, {
    autoClose: 2000,
    theme: "light",
    closeOnClick: true,
    position: "bottom-right",
    hideProgressBar: true,
    ...options,
  });
};
