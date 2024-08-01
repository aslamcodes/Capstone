import React, { useEffect, useState } from "react";
import useCoursesByStatus from "../../hooks/fetchers/useCoursesByStatus";
import { Course, CourseStatusEnum } from "../../interfaces/course";
import Loader from "../common/Loader";
import CourseCard from "../Course/CourseCard";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";

const AdminReviewTab = () => {
  const { user } = useAuthContext();

  const { courses, error, coursesLoading } = useCoursesByStatus(
    CourseStatusEnum.Review
  );
  const [localCourses, setLocalCourses] = useState<Course[]>();

  const navigate = useNavigate();

  useEffect(() => {
    if (courses) {
      setLocalCourses(courses);
    }
  }, [courses]);

  if (error) {
    return (
      <div className="alert alert-error">Problem trying to fetch courses</div>
    );
  }

  if (coursesLoading) {
    return <Loader />;
  }

  const handleAuthorize = async (id: number) => {
    await axios.put(
      `/api/Course/set-course-live?courseId=${id}`,
      {},
      {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      }
    );

    setLocalCourses((prev) => prev?.filter((course) => course.id !== id));
  };

  return (
    <div className="p-4">
      {localCourses?.map((course) => (
        <CourseCard
          course={course}
          key={course.id}
          actions={[
            {
              action: handleAuthorize,
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
