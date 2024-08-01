import React, { FC } from "react";
import { Course } from "../../interfaces/course";
import { Link } from "react-router-dom";

const CourseCard: FC<{
  course: Course;
  actions: { actionTitle: string; action: (courseId: number) => void }[];
}> = ({ course, actions }) => {
  return (
    <div className="card bg-base-100 w-72 shadow-xl ">
      <figure>
        <img
          src="https://img.daisyui.com/images/stock/photo-1606107557195-0e29a4b5b4aa.webp"
          alt="Shoes"
        />
      </figure>
      <div className="card-body p-6">
        <h2 className="card-title">{course.name}</h2>
        <p className="text-balance">
          {course.description.length > 50
            ? course.description.slice(0, 50) + "..."
            : course.description}
        </p>
        <div className="card-actions justify-end mt-5 ">
          {actions.map(({ action, actionTitle }) => (
            <button
              className="btn btn-outline btn-md"
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
