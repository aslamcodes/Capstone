import Loader from "../../components/common/Loader";
import { Link, useNavigate } from "react-router-dom";
import useEducatorCourses from "../../hooks/fetchers/useEducatorCourses";
import { BiPlus } from "react-icons/bi";
import { useRef, useState } from "react";
import CreateCourseForm from "../../components/Course/CreateCourseForm";
import { useAuthContext } from "../../contexts/auth/authReducer";
import CourseCard from "../../components/Course/CourseCard";

const Educator = () => {
  const { user } = useAuthContext();
  const ref = useRef<HTMLDialogElement>(null);
  const navigate = useNavigate();
  var { courses, coursesLoading, error } = useEducatorCourses(
    user?.id as number
  );

  if (!courses) {
    return <div>No courses found</div>;
  }

  const handleOnCreateCourse = () => {
    ref.current?.showModal();
  };

  if (coursesLoading) {
    return (
      <div className="min-h-screen min-w-screen flex items-center justify-center">
        <Loader />
      </div>
    );
  }

  return (
    <div>
      <dialog className="modal" ref={ref}>
        <div className="modal-box">
          <CreateCourseForm />
        </div>
        <form method="dialog" className="modal-backdrop">
          <button></button>
        </form>
      </dialog>

      <div className="space-y-3">
        <div className="flex justify-between my-3">
          <h1 className="text-2xl font-bold">Your Courses</h1>
          <button className="btn" onClick={handleOnCreateCourse}>
            <BiPlus /> Create Course
          </button>
        </div>
        <div className="grid gap-4 grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 2xl:grid-cols-6">
          {courses?.map((course) => (
            <CourseCard
              course={course}
              actions={[
                {
                  actionTitle: "Manage Course",
                  action: (id) => {
                    navigate(`/manage-course/${id}`);
                  },
                },
              ]}
              key={course.id}
            ></CourseCard>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Educator;
