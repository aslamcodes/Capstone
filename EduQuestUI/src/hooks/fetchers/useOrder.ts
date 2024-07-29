import { useAuthContext } from "../../contexts/auth/authReducer";
import { Order } from "../../interfaces/order";
import useFetchAxios from "./useFetchAxios";

export default function useOrder(orderId: number) {
  const { user } = useAuthContext();

  const { data, isLoading, error } = useFetchAxios<Order, any>({
    url: "/api/Order",
    params: { orderId },
    headers: { Authorization: `Bearer ${user?.token}` },
  });

  return { order: data, isLoading, error };
}
