import useCoursesByStatus from "../../hooks/fetchers/useCoursesByStatus";
import { CourseStatusEnum } from "../../interfaces/course";
import { useNavigate } from "react-router-dom";
import Loader from "../common/Loader";
import CourseCard from "../Course/CourseCard";

const AdminOutdatedTab = () => {
  const { courses, error, coursesLoading } = useCoursesByStatus(
    CourseStatusEnum.Outdated
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
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-6 mt-10 gap-2">
      {courses?.map((course) => (
        <CourseCard
          course={course}
          key={course.id}
          actions={[
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

export default AdminOutdatedTab;
