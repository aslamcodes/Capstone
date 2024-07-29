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
