import React, { FC, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import useCourse from "../../hooks/fetchers/useCourse";
import axios, { AxiosError } from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useSections from "../../hooks/fetchers/useSections";
import SectionDrop from "./SectionDrop";
import CourseDescription from "./ContentDescription";
import { CourseReviews } from "./review";
import { customToast } from "../../utils/toast";
import useUserOwnsCourse from "../../hooks/fetchers/useUserOwnsCourse";
import useEducatorProfile from "../../hooks/fetchers/useEducatorProfile";

const CourseLanding = () => {
  const { courseId } = useParams();
  const { course, isLoading } = useCourse(Number(courseId));
  const [isBuying, setIsBuying] = React.useState(false);
  const {
    sections,
    isLoading: sectionsLoading,
    error,
  } = useSections(courseId as string);
  const { user } = useAuthContext();
  const { educatorProfile } = useEducatorProfile(course?.educatorId as number);

  const navigate = useNavigate();

  const { isUserOwns, isLoading: checkingCourseOwnership } = useUserOwnsCourse(
    Number(courseId)
  );

  if (isLoading || sectionsLoading || checkingCourseOwnership) {
    return <div>Loading...</div>;
  }

  if (!course) {
    return <p className="alert alert-info">Course not found</p>;
  }

  if (!user) {
    customToast("Please login to view this page", { type: "info" });
    navigate("/login");
    return;
  }

  const handleBuyCourse = async () => {
    try {
      setIsBuying(true);
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
      customToast("Order created successfully", { type: "success" });
      navigate(`/order/${order.data.id}`);
    } catch (error: any) {
      customToast(error.response.data.message, { type: "error" });
    } finally {
      setIsBuying(false);
    }
  };

  return (
    <div>
      <div className="hero bg-base-200 min-h-[80vh] rounded-lg">
        <div className="hero-content flex-col lg:flex-row-reverse">
          {course.courseThumbnailPicture && (
            <img
              src={course.courseThumbnailPicture as string}
              className=" rounded-lg shadow-2xl"
            />
          )}
          <div>
            <h1 className="text-2xl md:text-5xl font-bold">{course.name}</h1>
            <p className="py-3 font-semibold break-all">
              <span>By</span>
              {educatorProfile?.profilePictureUrl ? (
                <img
                  className="inline w-8 h-8 rounded-full mx-2"
                  src={educatorProfile?.profilePictureUrl}
                />
              ) : (
                <div className=" w-8 h-8 rounded-full mx-2"></div>
              )}
              {educatorProfile?.firstName} {educatorProfile?.lastName}
            </p>
            <p className="py-4 break-all">{course.description}</p>

            <p className="py-6 break-all text-2xl font-bold">
              Rs. {course.price}
            </p>
            {isUserOwns ? (
              <button
                className="btn btn-outline"
                onClick={() => {
                  navigate(`/myCourses/${course.id}`);
                }}
              >
                Go to Course
              </button>
            ) : (
              <button
                disabled={isBuying}
                className="btn btn-lg glow-on-hover"
                onClick={handleBuyCourse}
              >
                Buy Course
              </button>
            )}
          </div>
        </div>
      </div>
      <div className=" max-w-3xl mx-auto space-y-4">
        <div className=" my-10 space-y-2">
          <h1 className="text-2xl font-bold ">Course Contents</h1>
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
        <CourseDescription courseId={Number(courseId)}></CourseDescription>
        <div>
          <h1 className="text-2xl font-bold">Reviews</h1>
          <CourseReviews courseId={Number(courseId)} />
        </div>
      </div>
    </div>
  );
};

export default CourseLanding;
