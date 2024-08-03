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
  const [currentCategory, setCurrentCategory] = React.useState<number>();
  const navigate = useNavigate();
  return (
    <div>
      <div>
        <h1 className="text-2xl font-bold my-4 bg-base-100 ">
          Explore Courses
        </h1>
        {isLoading ? (
          <Loader size="md" type="bars" />
        ) : (
          <div className="flex mb-4 max-w-screen overflow-x-scroll gap-3 no-scrollbar sticky top-16 z-30 bg-base-100">
            {categories?.map((category) => (
              <button
                className={`btn btn-sm md:btn-md font-semibold md:font-bold ${
                  category.id === currentCategory &&
                  "bg-base-content text-base-300"
                }`}
                onClick={() => {
                  if (category.id === currentCategory) {
                    setCurrentCategory(undefined);
                    return;
                  }
                  setCurrentCategory(category.id);
                }}
              >
                {category.name}
              </button>
            ))}
          </div>
        )}
        <div className="grid gap-2 md:gap-4 grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-6 ">
          {courses
            ?.filter((c) => {
              if (!currentCategory) return true;
              return c.courseCategoryId == currentCategory;
            })
            .map((course) => (
              <CourseCard
                course={course}
                key={course.id}
                actions={[
                  {
                    actionTitle: "View Course",
                    action: (id) => {
                      navigate(`/course-description/${id}`);
                    },
                  },
                ]}
              />
            ))}
        </div>
      </div>
    </div>
  );
};

export default Home;
