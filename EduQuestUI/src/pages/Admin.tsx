import { useAuthContext } from "../contexts/auth/authReducer";
import { useNavigate } from "react-router-dom";
import { customToast } from "../utils/toast";
import AdminTabs from "../components/admin/admin-tabs";

const Admin = () => {
  const { user } = useAuthContext();
  const navigate = useNavigate();

  if (!user) {
    navigate("/login");
    customToast("Please login to view this page", { type: "info" });
  }

  if (!user || !user.isAdmin) {
    navigate("/login");
    customToast("You are not authorized to view this page", {
      type: "error",
    });
    return;
  }

  return (
    <div className="space-y-4">
      <div>
        <h1 className="text-2xl font-bold">Welcome to the admin panel</h1>
        <div>
          <p>Here you can manage courses and other admin tasks </p>
        </div>
      </div>
      <AdminTabs />
    </div>
  );
};

export default Admin;
