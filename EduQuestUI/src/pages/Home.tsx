import React from "react";
import { useAuthContext } from "../contexts/auth/authReducer";
import useRecomendedCoureses from "../hooks/fetchers/useRecomendedCourses";
import CourseCard from "../components/Course/CourseCard";

const Home = () => {
  const { user } = useAuthContext();

  const { courses } = useRecomendedCoureses(user?.id as number);

  return (
    <div className="grid  grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-6 ">
      {courses?.map((course) => (
        <CourseCard type="view" course={course} />
      ))}
    </div>
  );
};

export default Home;
