import axios, { AxiosHeaders } from "axios";
import { useEffect, useState } from "react";

export default function useFetchAxios<T, K>(
  {
    url,
    params = {},
    headers = {},
  }: {
    url: string;
    params?: any;
    headers?: any;
  },
  deps: any[] = []
) {
  const [data, setData] = useState<T | null>(null);

  const [isLoading, setIsLoading] = useState<boolean>(false);

  const [error, setError] = useState<K | null>(null);

  const memoizedHeaders = JSON.stringify(headers);
  const memoizedParams = JSON.stringify(params);

  useEffect(() => {
    async function fetchData() {
      try {
        setIsLoading(true);
        setError(null);
        var { data } = await axios.get<T>(url, {
          headers: { ...headers },
          params: { ...params },
        });
        setData(data);
        setIsLoading(false);
      } catch (error) {
        setError(error);
      }

      setIsLoading(false);
    }

    fetchData();
  }, [url, memoizedHeaders, memoizedParams, ...deps]);

  return { data, error, isLoading };
}
