import {
  AUTH_FAILURE,
  AUTH_LOGOUT,
  AUTH_REQUEST,
  AUTH_SUCCESS,
} from "./constants";
import axios from "axios";

export const login = async (
  dispatch: React.Dispatch<any>,
  { email, password }: { email: string; password: string }
) => {
  dispatch({ type: AUTH_REQUEST });
  try {
    const { data } = await axios.post("/api/v1/users/login", {
      email,
      password,
    });
    dispatch({ type: AUTH_SUCCESS, payload: data });
    localStorage.setItem("userInfo", JSON.stringify(data));
  } catch (error) {
    dispatch({
      type: AUTH_FAILURE,
      payload: error,
    });
  }
};

export const register = async (dispatch: React.Dispatch<any>, payload: any) => {
  dispatch({ type: AUTH_REQUEST });
  try {
    const config = {
      headers: { "content-type": "multipart/form-data" },
    };

    const { data } = await axios.post(
      "/api/v1/users/register",
      payload,
      config
    );
    dispatch({ type: AUTH_SUCCESS, payload: data });
  } catch (error) {
    dispatch({
      type: AUTH_FAILURE,
      payload: error,
    });
  }
};

export const logout = async (dispatch: React.Dispatch<any>) => {
  localStorage.removeItem("userInfo");
  dispatch({ type: AUTH_LOGOUT });
};
