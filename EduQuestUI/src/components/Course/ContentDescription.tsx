import React, { FC } from "react";
import useContent from "../../hooks/fetchers/useContent";
import useCourse from "../../hooks/fetchers/useCourse";
import { GoGoal } from "react-icons/go";

const CourseDescription: FC<{
  courseId: number;
}> = ({ courseId }) => {
  const { course, error, isLoading } = useCourse(courseId);

  return (
    <div className="mt-5 space-y-3">
      <div className="space-y-1">
        <h1 className="text-2xl font-bold">{course?.name}</h1>
        <p className="text-lg ">{course?.description}</p>
      </div>
      <div className="flex flex-wrap gap-4 ">
        <div className="p-4 min-w-72 rounded-md border border-base-300">
          <h1 className="text-xl font-bold">Course Objectives</h1>
          {course?.courseObjective?.split("|").map((objective, index) => (
            <div className="flex gap-2 items-center">
              <GoGoal />
              <p className="">{objective}</p>
            </div>
          ))}
        </div>
        <div className="p-4 min-w-72 rounded-md border border-base-300">
          <h1 className="text-xl font-bold">Prerequisites</h1>
          {course?.prerequisites?.split("|").map((prerequisite, index) => (
            <div className="flex gap-2 items-center">
              <GoGoal />
              <p className="">{prerequisite}</p>
            </div>
          ))}
        </div>
        <div className="p-4 min-w-72 rounded-md border border-base-300">
          <h1 className="text-xl font-bold">Target Audiences</h1>
          {course?.targetAudience?.split("|").map((learning, index) => (
            <div className="flex gap-2 items-center">
              <GoGoal />
              <p className="">{learning}</p>
            </div>
          ))}
        </div>
      </div>
      <div>
        <h1 className="text-xl font-bold">Educator</h1>
        <div></div>
      </div>
    </div>
  );
};

export default CourseDescription;
