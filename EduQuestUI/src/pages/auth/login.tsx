import { useForm, SubmitHandler } from "react-hook-form";
import { Link } from "react-router-dom";
import { login } from "../../contexts/auth/actions";
import { useAuthDispatchContext } from "../../contexts/auth/authReducer";

type Inputs = {
  email: string;
  password: string;
};

export default function LoginPage() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Inputs>();

  const dispatch = useAuthDispatchContext();
  const onSubmit: SubmitHandler<Inputs> = async (data) => {
    await login(dispatch, data);
  };

  return (
    <div className="min-h-screen">
      <form
        onSubmit={handleSubmit(onSubmit)}
        className="max-w-72 mx-auto flex flex-col bg-base-100 gap-4 p-4 border border-gray-300 rounded-lg mt-4"
      >
        <h1 className="text-2xl font-bold">Login</h1>
        <div className="flex flex-col gap-2 items-start">
          <label>Email</label>
          <input
            type="email"
            className="input input-bordered w-full max-w-xs"
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
            className="input input-bordered w-full max-w-xs"
            {...register("password", { required: true })}
          />
          {errors.password && (
            <span className="text-error">Password is required</span>
          )}
        </div>
        <button className="btn btn-primary">Submit</button>
        <Link to={"/register"} className="text-primary">
          Don't have an account? Register
        </Link>
      </form>
    </div>
  );
}
