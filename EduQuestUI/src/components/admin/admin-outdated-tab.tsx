import useCoursesByStatus from "../../hooks/fetchers/useCoursesByStatus";
import { CourseStatusEnum } from "../../interfaces/course";
import { useNavigate } from "react-router-dom";
import Loader from "../common/Loader";
import CourseCard from "../Course/CourseCard";
import NoData from "../../assets/no_data.svg";

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
    <div>
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
      {courses?.length === 0 && (
        <div className="flex flex-col gap-4 items-center justify-center w-full min-h-[60vh]">
          <img src={NoData} className="max-w-52" />
          <p className="text-2xl font-bold">No Courses found to review</p>
        </div>
      )}
    </div>
  );
};

export default AdminOutdatedTab;
