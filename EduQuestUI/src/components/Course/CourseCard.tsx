import React, { FC } from "react";
import { Course } from "../../interfaces/course";
import { Link } from "react-router-dom";

const CourseCard: FC<{ course: Course; type: "manage" | "view" }> = ({
  course,
  type,
}) => {
  return (
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
          {type === "manage" ? (
            <Link to={`/manage-course/${course.id}`}>
              <button className="btn btn-primary">Manage Course</button>
            </Link>
          ) : (
            <Link to={`/course-description/${course.id}`}>
              <button className="btn btn-primary">View Course</button>
            </Link>
          )}
        </div>
      </div>
    </div>
  );
};

export default CourseCard;
