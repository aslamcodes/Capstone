import React from "react";
import useOrder from "../../hooks/fetchers/useOrder";
import { useParams } from "react-router-dom";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";

const OrderPage = () => {
  const { user } = useAuthContext();
  const { orderId } = useParams();

  if (!orderId) {
    return <div>Invalid Order</div>;
  }

  if (!user) return <div>You must be logged in to view this page</div>;

  const { order } = useOrder(Number(orderId));

  const handlePayment = async () => {
    if (!order) return;
    await axios.post(
      "/api/Payments/Make-Payment",
      {},
      {
        params: {
          orderId: order?.id,
        },

        headers: {
          Authorization: `Bearer ${user.token}`,
        },
      }
    );
  };

  return (
    <div>
      {JSON.stringify(order)}
      <button className="btn btn-primary" onClick={handlePayment}>
        Pay for Order
      </button>
    </div>
  );
};

export default OrderPage;
