import React from "react";
import { useAuthContext } from "../../contexts/auth/authReducer";
import Loader from "../../components/common/Loader";
import useSWR from "swr";
import { fetcherWithToken } from "../../utils/fetcher";
import ThemeController from "../../components/common/ThemeController";
import CoursePage from "../course/CoursePage";
import { Course } from "../course/MyCourse";
import { Link } from "react-router-dom";
import useSWRImmutable from "swr/immutable";

const Educator = () => {
  const { user, isLoading, error } = useAuthContext();

  const { data, isLoading: coursesLoading } = useSWRImmutable<Course[], any>(
    ["/api/Course/Educator-Courses?educatorId=1", user?.token],
    ([url, token]) => fetcherWithToken(url, token as string)
  );

  if (isLoading || coursesLoading) {
    return (
      <div className="min-h-screen min-w-screen flex items-center justify-center">
        <Loader />
      </div>
    );
  }

  if (!data) {
    return <div>No courses found</div>;
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-6">
      {data?.map((course) => (
        <div className="card bg-base-100 w-72 shadow-xl ">
          <figure>
            <img
              src="https://img.daisyui.com/images/stock/photo-1606107557195-0e29a4b5b4aa.webp"
              alt="Shoes"
            />
          </figure>
          <div className="card-body">
            <h2 className="card-title">{course.name}</h2>
            <p>{course.description}</p>
            <div className="card-actions justify-end">
              <Link to={`/manage-course/${course.id}`}>
                <button className="btn btn-primary">Manage Course</button>
              </Link>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
};

export default Educator;
