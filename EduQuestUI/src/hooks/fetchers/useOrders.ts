import { useAuthContext } from "../../contexts/auth/authReducer";
import { Order } from "../../interfaces/order";
import useFetchAxios from "./useFetchAxios";

export default function useOrders() {
  const { user } = useAuthContext();

  const {
    data: orders,
    error,
    isLoading,
  } = useFetchAxios<Order[], any>({
    url: "/api/Order/user-orders",
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
  });

  return { orders, error, isLoading };
}
