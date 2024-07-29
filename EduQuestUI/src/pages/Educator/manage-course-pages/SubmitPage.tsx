import React, { FC, useEffect } from "react";
import { ManageCoursePageProps } from "./manageCourseTypes";
import axios from "axios";
import useCourseValidity from "../../../hooks/fetchers/useCourseValidity";
import { CiCircleAlert, CiCircleCheck, CiRainbow } from "react-icons/ci";
import { useAuthContext } from "../../../contexts/auth/authReducer";
import { Course } from "../../../interfaces/course";

interface SubmitCoursePageProps extends ManageCoursePageProps {}

const SubmitCoursePage: FC<SubmitCoursePageProps> = ({ initialCourse }) => {
  const { validity, isLoading, error } = useCourseValidity(
    initialCourse?.id as number
  );

  const { user } = useAuthContext();

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>{error.response.data?.message}</div>;
  }

  const handleSubmit = async () => {
    try {
      await axios.put(
        `/api/Course/Submit-For-Review?courseId=${initialCourse?.id}`,
        {},
        {
          headers: {
            Authorization: `Bearer ${user?.token}`,
          },
        }
      );
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="flex flex-col justify-center items-start gap-3 max-w-xl mx-auto min-h-[50vh]">
      <p className="font-bold space-x-1">
        <span>Current Status:</span>
        <span
          className={`badge ${getBadgeForStatus(
            initialCourse?.courseStatus as Course["courseStatus"]
          )} `}
        >
          {" "}
          {initialCourse?.courseStatus}
        </span>
      </p>
      {validity?.criterias.map((criteria) => (
        <div
          className={`p-2 flex gap-2 items-center ${
            criteria.isPassed ? "text-success" : "text-error"
          }`}
        >
          {criteria.isPassed ? (
            <CiCircleCheck className=" scale-150" />
          ) : (
            <CiCircleAlert className=" scale-150" />
          )}
          <span>{criteria.criteria}</span>
        </div>
      ))}
      <button
        className={`btn ${validity?.isValid ? "btn-primary" : "btn-disabled"}`}
        disabled={!validity?.isValid || initialCourse?.courseStatus == "Review"}
        onClick={handleSubmit}
      >
        Submit For Review
      </button>
    </div>
  );
};

function getBadgeForStatus(status: Course["courseStatus"]): string {
  let map = {
    Live: "badge-primary",
    Draft: "badge-secondary",
    Archived: "badge-error",
    Outdated: "badge-warning",
    Review: "badge-info",
  };

  return map[status];
}

export default SubmitCoursePage;
