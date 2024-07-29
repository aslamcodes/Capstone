import useSWRImmutable from "swr/immutable";
import { useAuthContext } from "../../contexts/auth/authReducer";
import axios from "axios";
import { Content, Course } from "../../interfaces/course";
import { fetcherWithToken } from "../../utils/fetcher";
import { Order } from "../../interfaces/order";

export default function useOrder(orderId: number) {
  const { user } = useAuthContext();

  const { data, isLoading, error } = useSWRImmutable<Order>(
    ["/api/Order", user?.token],
    ([url, token]) => fetcherWithToken(url, token as string, { orderId })
  );

  return { order: data, isLoading, error };
}
