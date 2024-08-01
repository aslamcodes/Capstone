import React from "react";
import useOrder from "../../hooks/fetchers/useOrder";
import { useParams, Navigate } from "react-router-dom";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";

const OrderPage = () => {
  const { user } = useAuthContext();
  const { orderId } = useParams();

  if (!orderId) {
    return <Navigate to="/orders" replace />;
  }

  if (!user) return <Navigate to="/login" replace />;

  const { order, isLoading, error } = useOrder(Number(orderId));

  const handlePayment = async () => {
    if (!order) return;
    try {
      await axios.post(
        "/api/Payments/Make-Payment",
        {},
        {
          params: { orderId: order.id },
          headers: { Authorization: `Bearer ${user.token}` },
        }
      );
      // Handle successful payment (e.g., show success message, redirect)
    } catch (error) {
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
            Status:{" "}
            <span className="badge badge-primary">{order.orderStatus}</span>
          </p>
          <p>Created: {new Date(order.createdAt).toLocaleString()}</p>
          <p>Course ID: {order.orderedCourseId}</p>
          <p className="text-xl font-semibold mt-4">
            Total Price: ${order.totalPrice.toFixed(2)}
          </p>
          {order.discountAmount > 0 && (
            <p className="text-sm text-success">
              Discount Applied: ${order.discountAmount.toFixed(2)}
            </p>
          )}
          {order.orderStatus === "Pending" && (
            <div className="card-actions justify-end mt-4">
              <button className="btn btn-primary" onClick={handlePayment}>
                Pay for Order
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default OrderPage;
