import React, { FC, PropsWithChildren, useReducer } from "react";
import { AuthContext, AuthDispatchContext, initialState } from "./context";
import { authReducer } from "./authReducer";

export const AuthContextProvider: FC<PropsWithChildren> = ({ children }) => {
  const [state, dispatch] = useReducer(authReducer, initialState);
  return (
    <AuthContext.Provider value={state}>
      <AuthDispatchContext.Provider value={dispatch}>
        {children}
      </AuthDispatchContext.Provider>
    </AuthContext.Provider>
  );
};
export default AuthContextProvider;
