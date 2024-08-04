import { useForm, SubmitHandler } from "react-hook-form";
import { Link, useNavigate } from "react-router-dom";
import {
  useAuthContext,
  useAuthDispatchContext,
} from "../../contexts/auth/authReducer";
import { register as registerUser } from "../../contexts/auth/actions";
import { customToast } from "../../utils/toast";
type Inputs = {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
};

export default function RegisterPage() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Inputs>();

  const { user, error } = useAuthContext();

  const dispatch = useAuthDispatchContext();
  const navigate = useNavigate();

  const onSubmit: SubmitHandler<Inputs> = async (data) => {
    await registerUser(dispatch, data);
  };

  if (user) {
    customToast("Registered", {
      type: "success",
    });
    navigate("/");
  }

  if (error) {
    customToast(error.response.data?.message ?? "Error logging in", {
      type: "error",
    });
  }

  return (
    <div className="min-h-[50vh] my-12">
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="md:max-w-[40vw] mx-auto flex flex-col bg-base-100 gap-4 p-4 border border-gray-300 rounded-lg mt-4"
      >
        <h1 className="text-2xl font-bold">Register</h1>
        <div className="flex flex-col gap-2 items-start">
          <label>First Name</label>
          <input
            type="text"
            className="input input-bordered w-full"
            {...register("firstName", { required: true })}
          />
          {errors.firstName && (
            <span className="text-error">Email is required</span>
          )}
        </div>
        <div className="flex flex-col gap-2 items-start">
          <label>Last Name</label>
          <input
            type="text"
            className="input input-bordered w-full"
            {...register("lastName")}
          />
        </div>
        <div className="flex flex-col gap-2 items-start">
          <label>Email</label>
          <input
            type="email"
            className="input input-bordered w-full"
            {...register("email", { required: true })}
          />
          {errors.email && (
            <span className="text-error">Email is required</span>
          )}
        </div>

        <div className="flex flex-col gap-2 items-start">
          <label>Password</label>
          <input
            type="password"
            className="input input-bordered w-full "
            {...register("password", { required: true })}
          />
          {errors.password && (
            <span className="text-error">Password is required</span>
          )}
        </div>
        <button className="btn btn-outline">Submit</button>
        <Link to={"/"} className="text-">
          Don't have an account? Register
        </Link>
      </form>
    </div>
  );
}
