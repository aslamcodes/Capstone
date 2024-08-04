import { useEffect, useState } from "react";
import Loader from "../common/Loader";
import { useNavigate } from "react-router-dom";
import useCoursesByStatus from "../../hooks/fetchers/useCoursesByStatus";
import { Course, CourseStatusEnum } from "../../interfaces/course";
import { useAuthContext } from "../../contexts/auth/authReducer";
import CourseCard from "../Course/CourseCard";
import { customToast } from "../../utils/toast";
import axiosInstance from "../../utils/fetcher";
import NoData from "../../assets/no_data.svg";

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
    <div>
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
      {localCourses?.length === 0 && (
        <div className="flex flex-col gap-4 items-center justify-center w-full min-h-[60vh]">
          <img src={NoData} className="max-w-52" />
          <p className="text-2xl font-bold">No Courses found to review</p>
        </div>
      )}
    </div>
  );
};

export default AdminLiveTab;
