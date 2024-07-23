import React from "react";
import useSWR from "swr";

const fetcher = (url: string) => fetch(url).then((r) => r.json());

const Apitest = () => {
  const { data, error } = useSWR("https://dummyjson.com/test", fetcher);

  console.log(data, error);

  return <div>Apitest</div>;
};

export default Apitest;
