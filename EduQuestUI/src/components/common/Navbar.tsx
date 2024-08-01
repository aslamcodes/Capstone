import React from "react";
import {
  useAuthContext,
  useAuthDispatchContext,
} from "../../contexts/auth/authReducer";
import { Link, useLocation } from "react-router-dom";
import ThemeController from "./ThemeController";
import { logout } from "../../contexts/auth/actions";
import useUserProfile from "../../hooks/fetchers/useUserProfile";
import Loader from "./Loader";

const Navbar = () => {
  const { user } = useAuthContext();
  const { user: userProfile, isLoading } = useUserProfile();

  const dispatch = useAuthDispatchContext();
  const location = useLocation();
  return (
    <div className="navbar bg-base-100 fixed left-0 right-0 top-0 z-50  backdrop-filter backdrop-blur-lg  bg-opacity-95  ">
      <div className="flex-1">
        <Link to={"/"} className="btn btn-ghost text-xl">
          EduQuest
        </Link>
      </div>
      {user && location.pathname !== "login" && (
        <div className="flex-none gap-2 ">
          <div className="form-control">
            <input
              type="text"
              placeholder="Search"
              className="input input-bordered w-24 md:w-auto"
            />
          </div>

          <div className="dropdown dropdown-end">
            {isLoading ? (
              <div
                tabIndex={0}
                role="button"
                className="btn btn-ghost btn-circle avatar"
              >
                <Loader></Loader>
              </div>
            ) : (
              <div
                tabIndex={0}
                role="button"
                className="btn btn-ghost btn-circle avatar"
              >
                <div className="w-10 rounded-full">
                  {userProfile?.profilePictureUrl ? (
                    <img
                      alt="Tailwind CSS Navbar component"
                      src={userProfile?.profilePictureUrl as string}
                    />
                  ) : (
                    <div className="w-10 h-10 rounded-full bg-base-300 text-base-content flex items-center justify-center">
                      <p>
                        {userProfile?.firstName[0]}
                        {userProfile?.lastName[0]}
                      </p>
                    </div>
                  )}
                </div>
              </div>
            )}

            <ul
              tabIndex={0}
              className="menu menu-sm dropdown-content z-50 bg-base-100 rounded-box z-[1] mt-3 w-52 p-2 shadow"
            >
              <li>
                <Link to={"/profile"} className="justify-between">
                  Profile
                </Link>
              </li>
              <li>
                <a>Settings</a>
              </li>
              <li onClick={() => logout(dispatch)}>
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
      )}
    </div>
  );
};

export default Navbar;
