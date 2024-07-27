import { useContext } from "react";
import {
  AUTH_FAILURE,
  AUTH_LOGOUT,
  AUTH_REQUEST,
  AUTH_SUCCESS,
} from "./constants";
import {
  AuthContext,
  AuthDispatchContext,
  AuthState,
  initialState,
} from "./context";

export const authReducer = (state = initialState, action: any) => {
  switch (action.type) {
    case AUTH_REQUEST:
      return { ...state, isLoading: true, error: null };
    case AUTH_SUCCESS:
      return { ...state, isLoading: false, user: action.payload };
    case AUTH_FAILURE:
      return { ...state, isLoading: false, error: action.payload };
    case AUTH_LOGOUT:
      return { user: null, isLoading: false, error: false };
    default:
      return state;
  }
};

export const authRegisterReducer = (state = initialState, action: any) => {
  switch (action.type) {
    case AUTH_REQUEST:
      return { ...state, isLoading: true };
    case AUTH_SUCCESS:
      return { ...state, isLoading: false, user: action.payload };
    case AUTH_FAILURE:
      return { ...state, isLoading: false, error: action.payload };
    default:
      return state;
  }
};

export const useAuthContext = () => {
  const context = useContext<AuthState>(AuthContext);

  if (!context)
    throw new Error(
      "useAuthContext Hook must be defined within a context provider"
    );

  return context;
};

export const useAuthDispatchContext = () => {
  const context = useContext(AuthDispatchContext);

  if (!context)
    throw new Error(
      "useAuthDispatchContext Hook must be defined within a context provider"
    );

  return context;
};
