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
import SearchBar from "./search";

const Navbar = () => {
  const { user } = useAuthContext();
  const { user: userProfile, isLoading } = useUserProfile();

  const dispatch = useAuthDispatchContext();
  const location = useLocation();
  return (
    <div className="navbar bg-base-100 fixed left-0 right-0 top-0 z-50  backdrop-filter backdrop-blur-lg  bg-opacity-95  justify-between ">
      <div className="space-x-4">
        <div className="">
          <Link to={"/"} className="btn btn-ghost text-xl">
            EduQuest
          </Link>
        </div>
        <div className="form-control">
          <SearchBar />
        </div>
      </div>
      <div className="space-x-4">
        {user?.isEducator && (
          <div>
            <div
              className="tooltip  tooltip-bottom"
              data-tip="Goes to Educator view, where courses can be managed"
            >
              <button className="btn btn-ghost">
                <Link to={"/Educator"}>Educator</Link>
              </button>
            </div>
          </div>
        )}

        {user?.isAdmin && (
          <div>
            <div
              className="tooltip  tooltip-bottom"
              data-tip="Goes to Admin view, where courses can be managed"
            >
              <button className="btn btn-ghost">
                <Link to={"/admin"}>Admin</Link>
              </button>
            </div>
          </div>
        )}
        <button className="btn btn-ghost">
          <Link to={"/wishlist"}>Wishlist</Link>
        </button>

        {user && location.pathname !== "login" && (
          <div className="flex-none gap-2 ">
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
                        className="w-32 h-22"
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

                <li onClick={() => logout(dispatch)}>
                  <a>Logout</a>
                </li>
                <li>
                  <Link to={"/myCourses"}>My Courses </Link>
                </li>
              </ul>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Navbar;
