import { Link } from "react-router-dom";

const ErrorPage = () => {
  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100">
      <div className="text-center">
        <h1 className="text-9xl font-bold text-warning">404</h1>
        <h2 className="text-3xl font-semibold mt-4">Page Not Found</h2>
        <p className="text-lg mt-2 mb-6">
          Oops! The page you're looking for doesn't exist.
        </p>
        <Link to={"/"} className="btn btn-ghost">
          Go Back Home
        </Link>
      </div>
    </div>
  );
};

export default ErrorPage;
