import React, { useEffect, useState } from "react";
import useUserProfile from "../hooks/fetchers/useUserProfile";
import { BiPencil, BiSave } from "react-icons/bi";
import { GiTeacher } from "react-icons/gi";
import { FaChalkboardTeacher } from "react-icons/fa";
import axios from "axios";
import Loader from "../components/common/Loader";
import { useAuthContext } from "../contexts/auth/authReducer";
import { customToast } from "../utils/toast";
import { useNavigate } from "react-router-dom";
import type { UserProfile } from "../interfaces/common";

const UserProfile = () => {
  const { user, isLoading } = useUserProfile();
  const [userProfile, setUserProfile] = useState<UserProfile | null>(null);
  const [isEditing, setIsEditing] = useState(false);
  const { user: authUser } = useAuthContext();
  const [userProfileImage, setUserProfileImage] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string | null>();
  const navigate = useNavigate();

  useEffect(() => {
    setUserProfile(user);
    setPreviewUrl(user?.profilePictureUrl as string);
  }, [user]);

  const handleUpdate = async () => {
    await handleUserProfileImageUpdate();
    await axios.put(
      "/api/user",
      {
        id: authUser?.id,
        firstName: userProfile?.firstName,
        lastName: userProfile?.lastName,
        email: userProfile?.email,
      },
      {
        headers: {
          Authorization: `Bearer ${authUser?.token}`,
        },
      }
    );
  };

  const handleBecomeEducator = async () => {
    var { data } = await axios.put<UserProfile>(
      "/api/user/Become-Educator",
      {},
      {
        params: {
          userId: authUser?.id,
        },
        headers: {
          Authorization: `Bearer ${authUser?.token}`,
        },
      }
    );
    customToast("Your're now an Educator, Please login again to see effects", {
      type: "success",
    });
    setUserProfile(data);
  };

  const handleUserProfileImageUpdate = async () => {
    if (!userProfileImage) return;

    const formData = new FormData();

    formData.append("file", userProfileImage);

    await axios.put("/api/user/User-Profile", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
        Authorization: `Bearer ${authUser?.token}`,
      },
    });
  };

  if (!authUser) navigate("/login");

  if (isLoading)
    return (
      <div className="w-screen h-sceen flex flex-col items-center justify-center">
        <Loader type="spinner" size="lg" />
      </div>
    );

  return (
    <div className="grid grid-cols-1 lg:grid-cols-2 max-w-[80vw] mx-auto">
      <div className="flex flex-col gap-3 items-center">
        {userProfile?.profilePictureUrl ? (
          <img
            src={previewUrl as string}
            alt="profile"
            className="w-32 h-32 rounded-full object-cover"
          />
        ) : (
          <div className="w-32 h-32 rounded-full bg-slate-500 flex items-center justify-center text-white font-bold text-2xl ">
            {user?.firstName[0]}
            {user?.lastName[0]}
          </div>
        )}
        <p className="text-xl font-bold ">
          {user?.firstName} {user?.lastName}
        </p>
      </div>
      <div className="space-y-3">
        {!isEditing && (
          <button className="btn btn-ghost" onClick={() => setIsEditing(true)}>
            <BiPencil /> Edit
          </button>
        )}
        {isEditing && (
          <button
            className="btn btn-ghost"
            onClick={() => {
              setIsEditing(false);
              handleUpdate();
            }}
          >
            <BiSave /> Save
          </button>
        )}
        {!userProfile?.isEducator && (
          <button
            className="btn btn-ghost"
            onClick={() => {
              setIsEditing(false);
              handleBecomeEducator();
            }}
          >
            <FaChalkboardTeacher />
            Become a Educator
          </button>
        )}
        <div className="flex flex-col gap-2">
          <label className="text-xl">Profile</label>
          <input
            type="file"
            className="file-input file-input-bordered w-full"
            accept="image/*"
            disabled={!isEditing}
            onChange={(e) => {
              setUserProfileImage(e.target.files?.[0] as File);
              setPreviewUrl(URL.createObjectURL(e.target.files?.[0] as File));
            }}
          />
        </div>
        <div className="flex flex-col gap-2">
          <label className="text-xl">Firstname</label>
          <input
            className="input input-bordered"
            value={userProfile?.firstName}
            onChange={(e) =>
              setUserProfile(
                (prev) =>
                  ({ ...prev, firstName: e.target.value } as UserProfile)
              )
            }
            disabled={!isEditing}
          ></input>
        </div>
        <div className="flex flex-col gap-2 ">
          <label className=" text-xl">Lastname</label>
          <input
            className="input  input-bordered"
            value={userProfile?.lastName}
            onChange={(e) =>
              setUserProfile(
                (prev) => ({ ...prev, lastName: e.target.value } as UserProfile)
              )
            }
            disabled={!isEditing}
          ></input>
        </div>

        <div className="flex flex-col gap-2 ">
          <label className=" text-xl">Email</label>
          <input
            className="input  input-bordered"
            value={userProfile?.email}
            onChange={(e) =>
              setUserProfile(
                (prev) => ({ ...prev, email: e.target.value } as UserProfile)
              )
            }
            disabled={!isEditing}
          ></input>
        </div>
      </div>
    </div>
  );
};

export default UserProfile;
