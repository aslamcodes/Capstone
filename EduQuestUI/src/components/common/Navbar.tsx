import React from "react";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { Link } from "react-router-dom";
import ThemeController from "./ThemeController";

const Navbar = () => {
  const { user } = useAuthContext();

  return (
    <div className="navbar bg-base-100 fixed left-0 right-0 top-0 z-10  backdrop-filter backdrop-blur-lg  bg-opacity-95  ">
      <div className="flex-1">
        <Link to={"/"} className="btn btn-ghost text-xl">
          EduQuest
        </Link>
      </div>
      <div className="flex-none gap-2 ">
        <ThemeController />
        <div className="form-control">
          <input
            type="text"
            placeholder="Search"
            className="input input-bordered w-24 md:w-auto"
          />
        </div>
        <div className="dropdown dropdown-end">
          <div
            tabIndex={0}
            role="button"
            className="btn btn-ghost btn-circle avatar"
          >
            <div className="w-10 rounded-full">
              <img
                alt="Tailwind CSS Navbar component"
                src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp"
              />
            </div>
          </div>
          <ul
            tabIndex={0}
            className="menu menu-sm dropdown-content bg-base-100 rounded-box z-[1] mt-3 w-52 p-2 shadow"
          >
            <li>
              <a className="justify-between">
                Profile
                <span className="badge">New</span>
              </a>
            </li>
            <li>
              <a>Settings</a>
            </li>
            <li>
              <a>Logout</a>
            </li>
            {user?.isEducator && (
              <li>
                <Link to={"/Educator"}>Educator</Link>
              </li>
            )}
            <li>
              <Link to={"/myCourses"}>My Courses </Link>
            </li>
          </ul>
        </div>
      </div>
    </div>
  );
};

export default Navbar;
