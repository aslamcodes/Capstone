import React from "react";
import useCoursesByStatus from "../../hooks/fetchers/useCoursesByStatus";
import { CourseStatusEnum } from "../../interfaces/course";
import Loader from "../common/Loader";
import CourseCard from "../Course/CourseCard";
import { useNavigate } from "react-router-dom";

const AdminReviewTab = () => {
  const { courses, error, coursesLoading } = useCoursesByStatus(
    CourseStatusEnum.Review
  );

  const navigate = useNavigate();

  if (error) {
    return (
      <div className="alert alert-error">Problem trying to fetch courses</div>
    );
  }

  if (coursesLoading) {
    return <Loader />;
  }

  return (
    <div>
      {courses?.map((course) => (
        <CourseCard
          course={course}
          key={course.id}
          actions={[
            {
              action: (id) => {
                alert(id);
              },
              actionTitle: "Authorize",
            },
            {
              action: (id) => {
                navigate("/course-description/" + id);
              },
              actionTitle: "View Landing Page",
            },
          ]}
        />
      ))}
    </div>
  );
};

export default AdminReviewTab;
