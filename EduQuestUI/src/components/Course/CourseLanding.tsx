import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import useCourse from "../../hooks/fetchers/useCourse";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useSections from "../../hooks/fetchers/useSections";
import SectionDrop from "./SectionDrop";

const CourseLanding = () => {
  const { courseId } = useParams();
  const { course, isLoading } = useCourse(Number(courseId));
  const {
    sections,
    isLoading: sectionsLoading,
    error,
  } = useSections(courseId as string);
  const { user } = useAuthContext();
  const navigate = useNavigate();

  if (isLoading || sectionsLoading) {
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
        <h1 className="text-4xl font-bold"></h1>
        <p></p>
      </div>

      <div className="hero bg-base-200 min-h-screen">
        <div className="hero-content flex-col lg:flex-row-reverse">
          <img
            src="https://img.daisyui.com/images/stock/photo-1635805737707-575885ab0820.webp"
            className="max-w-sm rounded-lg shadow-2xl"
          />
          <div>
            <h1 className="text-5xl font-bold">Box Office News!</h1>
            <p className="py-6">{course.description}</p>
            <button className="btn btn-primary" onClick={handleBuyCourse}>
              Buy Course
            </button>
          </div>
        </div>
      </div>
      <div className=" max-w-3xl mx-auto">
        <div className=" my-10 space-y-2">
          <h1 className="text-2xl font-bold ">Sections</h1>
          {sections ? (
            sections.map((section) => (
              <SectionDrop
                name={section.name}
                description={section.description}
                id={section.id}
                onContentChange={() => {}}
              />
            ))
          ) : (
            <h1>This Course Dont have sections</h1>
          )}
        </div>
        <div>
          <h1 className="text-2xl font-bold ">Instructor</h1>
        </div>
      </div>
    </div>
  );
};

export default CourseLanding;
