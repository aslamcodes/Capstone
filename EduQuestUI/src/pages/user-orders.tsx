import React from "react";
import useOrders from "../hooks/fetchers/useOrders";
import OrderCard from "../components/order/OrderCard";
import { useAuthContext } from "../contexts/auth/authReducer";
import { Link, useNavigate } from "react-router-dom";
import NoData from "../assets/no_data.svg";

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
        {orders?.length === 0 && (
          <div className="min-h-[60vh] w-screen flex flex-col gap-2 justify-center items-center text-center">
            <img src={NoData} className="max-w-52"></img>
            <h2 className="text-xl font-bold">No Orders Found</h2>
            <p className="text-base-content">
              You have not placed any orders yet
            </p>
            <button>
              <Link to="/" className="btn btn-bordered">
                Explore Courses
              </Link>
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default UserOrders;
