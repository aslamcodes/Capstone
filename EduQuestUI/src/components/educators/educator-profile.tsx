import React, { FC } from "react";
import useUserProfile from "../../hooks/fetchers/useUserProfile";
import useEducatorProfile from "../../hooks/fetchers/useEducatorProfile";

const EducatorProfile: FC<{ educatorId: number }> = ({ educatorId }) => {
  const { educatorProfile } = useEducatorProfile(educatorId);
  return (
    <div className="flex p-4 bg-base-200 rounded-xl max-w-[32rem] gap-8">
      <img
        src={educatorProfile?.profilePictureUrl}
        className="w-36 h-36 object-cover rounded-full"
      />
      <div className="space-y-2">
        <h1 className="text-lg font-semibold">
          {educatorProfile?.firstName} {educatorProfile?.lastName}
        </h1>
        <p>
          Lorem ipsum dolor sit amet consectetur adipisicing elit. Atque,
          molestiae fugiat. Consectetur fugit laboriosam non deserunt eius nihil
          impedit cum nobis, vo
        </p>
        <button className="btn btn-outline btn-sm">Educator's Courses</button>
      </div>
    </div>
  );
};

export default EducatorProfile;
