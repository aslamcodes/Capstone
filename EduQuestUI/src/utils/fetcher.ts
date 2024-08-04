import axios from "axios";

export const fetcher = (url: string) => axios.get(url).then((res) => res.data);

export const fetcherWithToken = (
  url: string,
  token: string,
  params?: object
) => {
  return axios
    .get(url, {
      params: params,
      headers: {
        Authorization: `Bearer ${token}`,
      },
    })
    .then((res) => res.data);
};

const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

const axiosInstance = axios.create({
  baseURL: `${apiBaseUrl}`,
});

export default axiosInstance;
