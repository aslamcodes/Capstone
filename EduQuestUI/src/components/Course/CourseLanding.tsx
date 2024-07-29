import React from "react";
import { useParams } from "react-router-dom";

const CourseLanding = () => {
  const { courseId } = useParams();

  return <div>CourseLanding</div>;
};

export default CourseLanding;
