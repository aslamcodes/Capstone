import { useEffect, useState } from "react";
import Loader from "../common/Loader";
import { useNavigate } from "react-router-dom";
import useCoursesByStatus from "../../hooks/fetchers/useCoursesByStatus";
import { Course, CourseStatusEnum } from "../../interfaces/course";
import { useAuthContext } from "../../contexts/auth/authReducer";
import CourseCard from "../Course/CourseCard";
import axios from "axios";
import { customToast } from "../../utils/toast";
import axiosInstance from "../../utils/fetcher";

const AdminLiveTab = () => {
  const { user } = useAuthContext();
  const { courses, error, coursesLoading } = useCoursesByStatus(
    CourseStatusEnum.Live
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

  const handleOutdated = async (id: number) => {
    try {
      await axiosInstance.put(
        `/api/Course/set-course-outdated?courseId=${id}`,
        {},
        {
          headers: {
            Authorization: `Bearer ${user?.token}`,
          },
        }
      );
      setLocalCourses((prev) => prev?.filter((course) => course.id !== id));
    } catch {
      customToast("Failed to set course to outdated", { type: "error" });
    } finally {
    }
  };

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-6 mt-10 gap-2">
      {localCourses?.map((course) => (
        <CourseCard
          course={course}
          key={course.id}
          actions={[
            {
              action: handleOutdated,
              actionTitle: "Set to Outdated",
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

export default AdminLiveTab;
