import { useAuthContext } from "../../contexts/auth/authReducer";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { useEffect, useState } from "react";
import { Course } from "../../interfaces/course";
import CourseCard from "../../components/Course/CourseCard";

const MyCourses = () => {
  const { user } = useAuthContext();
  const navigate = useNavigate();

  const [courses, setCourses] = useState<Course[]>([]);
  if (!user?.token) {
    return <div>You need to be logged in to view this page.</div>;
  }

  useEffect(() => {
    let fetch = async () => {
      const { data } = await axios.get("/api/Course/Student-Courses", {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      });

      setCourses(data);
    };
    fetch();
  }, []);

  return (
    <div className="container mx-auto p-4">
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
