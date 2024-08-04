import { useAuthContext } from "../../contexts/auth/authReducer";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { Course } from "../../interfaces/course";
import CourseCard from "../../components/Course/CourseCard";
import axiosInstance from "../../utils/fetcher";

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
      </div>
    </div>
  );
};

export default MyCourses;
