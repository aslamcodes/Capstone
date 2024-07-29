import React from "react";
import { useAuthContext } from "../contexts/auth/authReducer";
import useRecomendedCoureses from "../hooks/fetchers/useRecomendedCourses";
import CourseCard from "../components/Course/CourseCard";
import { useNavigate } from "react-router-dom";
import useCategories from "../hooks/fetchers/useCategories";
import Loader from "../components/common/Loader";

const Home = () => {
  const { user } = useAuthContext();
  const { categories, isLoading } = useCategories();

  const { courses } = useRecomendedCoureses(user?.id as number);
  const navigate = useNavigate();
  if (!user) {
    navigate("/login");
  }

  return (
    <div>
      {isLoading ? (
        <Loader size="md" type="bars" />
      ) : (
        <div className="flex max-w-screen overflow-x-scroll gap-3 no-scrollbar">
          {categories?.map((category) => (
            <button className="btn font-bold my-4">{category.name}</button>
          ))}
        </div>
      )}
      <h1 className="text-2xl font-bold my-4">Recommended Courses</h1>
      <div className="grid  grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-6 ">
        {courses?.map((course) => (
          <CourseCard type="view" course={course} />
        ))}
      </div>
    </div>
  );
};

export default Home;
