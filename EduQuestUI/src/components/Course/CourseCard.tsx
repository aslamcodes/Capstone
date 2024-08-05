import { FC } from "react";
import { Course } from "../../interfaces/course";
import { getBadgeForStatus } from "../../utils/common";

const CourseCard: FC<{
  course: Course;
  actions: { actionTitle: string; action: (courseId: number) => void }[];
}> = ({ course, actions }) => {
  return (
    <div className="card bg-base-100 shadow-xl  ">
      {course.courseThumbnailPicture ? (
        <img
          src={course.courseThumbnailPicture}
          alt={course.name}
          className="rounded-t-lg h-32 object-cover"
        />
      ) : (
        <div className="w-full h-32 bg-base-content rounded-t-lg"></div>
      )}
      <div className="card-body p-6">
        <p
          className={`badge max-h-2 ${getBadgeForStatus(
            course.courseStatus as Course["courseStatus"]
          )} `}
        >
          {course.courseStatus}
        </p>
        <h2 className="card-title">{course.name} </h2>
        <p className="break-all">
          {course.description.length > 50
            ? course.description.slice(0, 50) + "..."
            : course.description}
        </p>
        <div className="card-actions justify-end mt-5">
          {actions.map(({ action, actionTitle }) => (
            <button
              className="btn btn-outline btn-sm"
              onClick={() => action(course.id)}
            >
              {actionTitle}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};

export default CourseCard;
