import Loader from "../../components/common/Loader";
import { useNavigate } from "react-router-dom";
import useEducatorCourses from "../../hooks/fetchers/useEducatorCourses";
import { BiPlus } from "react-icons/bi";
import { useRef } from "react";
import CreateCourseForm from "../../components/Course/CreateCourseForm";
import { useAuthContext } from "../../contexts/auth/authReducer";
import CourseCard from "../../components/Course/CourseCard";
import books from "../../assets/blank_canvas.svg";

const Educator = () => {
  const { user } = useAuthContext();
  const ref = useRef<HTMLDialogElement>(null);
  const navigate = useNavigate();

  var { courses, coursesLoading, error } = useEducatorCourses(
    user?.id as number
  );

  if (!user || !user.isEducator) {
    navigate("/");
    return;
  }

  if (!courses) {
    return <div>No courses found</div>;
  }

  const handleOnCreateCourse = () => {
    ref.current?.showModal();
  };

  const handleOnClose = () => {
    ref.current?.close();
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
      <dialog className="modal z-10" ref={ref}>
        <div className="modal-box">
          <CreateCourseForm onClose={handleOnClose} />
        </div>
        <form method="dialog" className="modal-backdrop">
          <button></button>
        </form>
      </dialog>

      <div className="space-y-3">
        <div className="flex justify-between my-3">
          <h1 className="text-lg md:text-2xl font-bold">Your Courses</h1>
          <button
            className="btn btn-sm md:btn-md"
            onClick={handleOnCreateCourse}
          >
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
        <div>
          {courses?.length === 0 && (
            <div className="flex flex-col items-center justify-center gap-4 min-h-[60vh]">
              <img src={books} className="max-w-52" />
              <h1 className="text-xl font-bold">No Courses Found</h1>
              <p className="text-base-content">
                You have not created any courses yet
              </p>

              <button className="btn btn-ghost" onClick={handleOnCreateCourse}>
                Create a Course, start influencing the world
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default Educator;
