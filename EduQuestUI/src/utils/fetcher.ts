import axios from "axios";

export const fetcher = (url: string) => axios.get(url).then((res) => res.data);

export const fetcherWithToken = (
  url: string,
  token: string,
  params?: object
) => {
  return axios
    .get(url, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
      params,
    })
    .then((res) => res.data);
};
