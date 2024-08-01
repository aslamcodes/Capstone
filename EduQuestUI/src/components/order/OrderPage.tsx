import React, { useEffect, useState } from "react";
import useOrder from "../../hooks/fetchers/useOrder";
import { useParams, Navigate, useNavigate } from "react-router-dom";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { customToast } from "../../utils/toast";
import Loader from "../common/Loader";

const OrderPage = () => {
  const { user } = useAuthContext();
  const { orderId } = useParams();

  const { order, isLoading, error } = useOrder(Number(orderId));
  const [isPaying, setIsPaying] = useState(false);
  const navigate = useNavigate();
  const [orderState, setOrderState] = useState(order?.orderStatus);

  useEffect(() => {
    if (order) {
      setOrderState(order.orderStatus);
    }
  }, [order]);

  if (!orderId) {
    return <Navigate to="/orders" replace />;
  }

  if (!user) return <Navigate to="/login" replace />;

  const handlePayment = async () => {
    if (!order) return;
    try {
      setIsPaying(true);

      await axios.post(
        "/api/Payments/Make-Payment",
        {},
        {
          params: { orderId: order.id },
          headers: { Authorization: `Bearer ${user.token}` },
        }
      );

      setIsPaying(false);
      setOrderState("Completed");
      customToast("Payment successful", { type: "success" });
      // Handle successful payment (e.g., show success message, redirect)
    } catch (error) {
      setIsPaying(false);
      customToast("Payment failed", { type: "error" });
      // Handle payment error (e.g., show error message)
    }
  };

  const handleCancel = async () => {
    if (!order) return;
    try {
      setIsPaying(true);
      await axios.put(
        "/api/Order/Cancel",
        {},
        {
          params: { orderId: order.id },
          headers: { Authorization: `Bearer ${user.token}` },
        }
      );

      setIsPaying(false);
      setOrderState("Cancelled");
      customToast("Payment Cancelled", { type: "info" });
      // Handle successful payment (e.g., show success message, redirect)
    } catch (error) {
      setIsPaying(false);
      customToast("Payment failed", { type: "error" });
      // Handle payment error (e.g., show error message)
    }
  };

  if (isLoading)
    return <div className="text-center">Loading order details...</div>;
  if (error)
    return (
      <div className="text-center text-error">
        Error loading order: {error.message}
      </div>
    );
  if (!order) return <div className="text-center">Order not found</div>;

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-6">Order Details</h1>
      <div className="card bg-base-100 shadow-xl">
        <div className="card-body">
          <h2 className="card-title">Order #{order.id}</h2>
          <p>
            Status: <span className="badge badge-primary">{orderState}</span>
          </p>
          <p>Created: {new Date(order.createdAt).toLocaleString()}</p>
          <p>Course ID: {order.orderedCourseId}</p>
          <p className="text-xl font-semibold mt-4">
            Total Price: Rs.{order.price.toFixed(2)}
          </p>
          {order.discountAmount > 0 && (
            <p className="text-sm text-success">
              Discount Applied: ${order.discountAmount.toFixed(2)}
            </p>
          )}
          {orderState === "Pending" && (
            <div className="card-actions justify-end mt-4">
              <button
                className="btn btn-outline"
                onClick={handleCancel}
                disabled={isLoading}
              >
                {isPaying ? <Loader /> : "Cancel"}
              </button>
              <button
                className="btn btn-outline bg-base-content text-base-100"
                onClick={handlePayment}
                disabled={isLoading}
              >
                {isPaying ? <Loader /> : "Pay for Order"}
              </button>
            </div>
          )}
          {orderState === "Completed" && (
            <button
              className="btn btn-block"
              onClick={() => {
                navigate(`/myCourses/${order.orderedCourseId}`);
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

export default OrderPage;
