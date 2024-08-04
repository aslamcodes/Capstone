import { useAuthContext } from "../../contexts/auth/authReducer";
import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { Course } from "../../interfaces/course";
import CourseCard from "../../components/Course/CourseCard";
import axiosInstance from "../../utils/fetcher";
import blankCanvas from "../../assets/blank_canvas.svg";

const MyCourses = () => {
  const { user } = useAuthContext();
  const navigate = useNavigate();

  const [courses, setCourses] = useState<Course[]>([]);

  if (!user) {
    navigate("/login");
    return;
  }

  useEffect(() => {
    let fetch = async () => {
      const { data } = await axiosInstance.get("/api/Course/Student-Courses", {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      });

      setCourses(data);
    };
    fetch();
  }, []);

  return (
    <div className="container mx-auto space-y-4">
      <h1 className="text-2xl font-bold">My Courses</h1>
      <div className="grid  grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-6">
        {courses?.map((course) => (
          <CourseCard
            actions={[
              {
                actionTitle: "View",
                action: (courseId) => {
                  navigate(`/myCourses/${courseId}`);
                },
              },
            ]}
            course={course}
            key={course.id}
          />
        ))}
        {courses.length === 0 && (
          <div className="min-h-[60vh] w-screen flex flex-col gap-2 justify-center items-center text-center">
            <img src={blankCanvas} className="max-w-52"></img>
            <h2 className="text-xl font-bold">No Courses Found</h2>
            <p className="text-base-content">
              You have not enrolled in any courses yet
            </p>

            <button>
              <Link to="/" className="btn btn-bordered">
                Explore Courses
              </Link>
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default MyCourses;
