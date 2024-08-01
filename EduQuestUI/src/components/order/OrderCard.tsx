import React, { FC, useEffect, useState } from "react";
import { Order } from "../../interfaces/order";
import { GoNumber } from "react-icons/go";
import { BiCalendar } from "react-icons/bi";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { customToast } from "../../utils/toast";
import Loader from "../common/Loader";
import { useNavigate } from "react-router-dom";

const OrderCard: FC<{
  order: Order;
}> = ({ order }) => {
  const [isPaying, setIsPaying] = useState(false);
  const { user } = useAuthContext();
  const [orderInState, setOrderInState] = useState(order);
  const navigate = useNavigate();

  useEffect(() => {
    if (order) {
      setOrderInState(order);
    }
  }, [order]);

  const handlePayment = async () => {
    try {
      setIsPaying(true);

      await axios.post(
        "/api/Payments/Make-Payment",
        {},
        {
          params: { orderId: orderInState.id },
          headers: { Authorization: `Bearer ${user?.token}` },
        }
      );

      setIsPaying(false);
      setOrderInState({ ...orderInState, orderStatus: "Completed" });
      customToast("Payment successful", { type: "success" });
      // Handle successful payment (e.g., show success message, redirect)
    } catch (error) {
      setIsPaying(false);
      customToast("Payment failed", { type: "error" });
      // Handle payment error (e.g., show error message)
    }
  };
  const handleCancel = async () => {
    setIsPaying(true);
    try {
      await axios.put(
        `/api/Order/Cancel?orderId=${order.id}`,
        {},
        {
          headers: { Authorization: `Bearer ${user?.token}` },
        }
      );
      customToast("Order canceled", { type: "success" });
      setOrderInState({ ...orderInState, orderStatus: "Canceled" });
      setIsPaying(false);
    } catch (error) {
      setIsPaying(false);
      customToast("Failed to cancel order", { type: "error" });
    }
  };
  const getStatusBadge = (status: string): string => {
    switch (status.toLowerCase()) {
      case "completed":
        return "badge-success text-base-100 ";
      case "pending":
        return "badge-warning";
      case "canceled":
        return "badge-error";
      default:
        return "badge-ghost";
    }
  };

  const formatDate = (dateString: string): string => {
    return new Date(dateString).toLocaleDateString("en-US", {
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  if (!orderInState) return <Loader />;

  return (
    <div key={orderInState.id} className="card bg-base-100 shadow-xl">
      <div className="card-body">
        <div className="flex justify-between items-center">
          <h2 className="card-title">Order #{orderInState.id}</h2>
          <div className={`badge ${getStatusBadge(orderInState.orderStatus)}`}>
            {orderInState.orderStatus}
          </div>
        </div>
        <p className="flex items-center gap-1">
          <GoNumber /> Course ID: {orderInState.orderedCourseId}
        </p>
        <p className="flex items-center gap-1">
          <BiCalendar />
          Date: {formatDate(orderInState.createdAt)}
        </p>
        <div className="flex justify-between items-center mt-2">
          <span className="text-lg font-bold">
            ${orderInState.price.toFixed(2)}
          </span>
          {orderInState.discountAmount > 0 && (
            <span className="text-sm text-success">
              Discount: ${orderInState.discountAmount.toFixed(2)}
            </span>
          )}
        </div>
        <div className="card-actions justify-end mt-4">
          {orderInState.orderStatus === "Pending" && (
            <div className="flex gap-2">
              <button
                className="btn btn-outline"
                disabled={isPaying}
                onClick={handleCancel}
              >
                Cancel
              </button>
              <button
                disabled={isPaying}
                className="btn btn-outline bg-base-content text-base-100 "
                onClick={handlePayment}
              >
                {isPaying ? <Loader></Loader> : "Pay"}
              </button>
            </div>
          )}
          {orderInState.orderStatus === "Completed" && (
            <button
              disabled={isPaying}
              className="btn btn-outline"
              onClick={() => {
                navigate(`/myCourses/${orderInState.orderedCourseId}`);
              }}
            >
              Go to Course
            </button>
          )}
        </div>
      </div>
    </div>
  );
};

export default OrderCard;
