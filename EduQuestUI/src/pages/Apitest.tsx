import axios from "axios";
import useSWR from "swr";

const fetcher = (url: string) => axios.get(url).then((res) => res.data);

const Apitest = () => {
  const { data, error } = useSWR("https://dummyjson.com/test", fetcher);

  console.log(data, error);

  return (
    <div>
      <label className="swap swap-rotate">
        <input onClick={() => {}} type="checkbox" />
        <div className="swap-on">DARKMODE</div>
        <div className="swap-off">LIGHTMODE</div>
      </label>
    </div>
  );
};

export default Apitest;
