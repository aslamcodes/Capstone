import { createContext } from "react";

const localStateString = localStorage.getItem("userInfo");

export interface User {
  isEducator: boolean;
  token: string;
  isAdmin: boolean;
  id: number;
}

export interface AuthState {
  user: User | null;
  isLoading: boolean;
  error: any;
}

export const initialState: AuthState = {
  user: (localStateString && JSON.parse(localStateString)) || null,
  isLoading: false,
  error: null,
};

export const AuthContext = createContext(initialState);
export const AuthDispatchContext = createContext<React.Dispatch<any> | null>(
  null
);
