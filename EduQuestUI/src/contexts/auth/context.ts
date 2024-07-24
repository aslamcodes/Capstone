import { createContext } from "react";

const localStateString = localStorage.getItem("userInfo");

export const initialState = {
  user: (localStateString && JSON.parse(localStateString)) || null,
  isLoading: false,
  error: null,
};

export const AuthContext = createContext(initialState);
export const AuthDispatchContext = createContext<React.Dispatch<any> | null>(
  null
);
