import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import useCourse from "../../hooks/fetchers/useCourse";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";

const CourseLanding = () => {
  const { courseId } = useParams();
  const { course, isLoading } = useCourse(Number(courseId));
  const { user } = useAuthContext();
  const navigate = useNavigate();
  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!course) {
    return <div>Course not found</div>;
  }

  if (!user) {
    return <div>You must be logged in to view this page</div>;
  }

  const handleBuyCourse = async () => {
    let order = await axios.post(
      "/api/Order",
      {
        orderedCourse: course.id,
        userId: user.id,
      },
      {
        headers: {
          Authorization: `Bearer ${user.token}`,
        },
      }
    );

    navigate(`/order/${order.data.id}`);
  };

  return (
    <div>
      <div>
        <h1>{course.name}</h1>
        <p>{course.description}</p>
      </div>
      <button className="btn btn-primary" onClick={handleBuyCourse}>
        Buy Course
      </button>
    </div>
  );
};

export default CourseLanding;
