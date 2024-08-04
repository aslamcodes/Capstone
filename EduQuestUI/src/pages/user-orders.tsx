import React from "react";
import useOrders from "../hooks/fetchers/useOrders";
import OrderCard from "../components/order/OrderCard";
import { useAuthContext } from "../contexts/auth/authReducer";
import { useNavigate } from "react-router-dom";

const UserOrders: React.FC = () => {
  const { orders } = useOrders();

  const { user } = useAuthContext();

  const navigate = useNavigate();

  if (!user) {
    navigate("/login");
  }

  return (
    <div className="container mx-auto ">
      <h1 className="text-2xl font-bold mb-6">Your Orders</h1>
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
        {orders?.map((order) => (
          <OrderCard order={order} />
        ))}
      </div>
    </div>
  );
};

export default UserOrders;
