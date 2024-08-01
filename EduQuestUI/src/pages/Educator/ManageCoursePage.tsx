import React, { useEffect, useState } from "react";
import { Tab } from "../../interfaces/common";
import Tabs from "../../components/common/Tabs";
import CourseInfo from "./manage-course-pages/CourseInfoPage";
import CourseCurriculum from "./manage-course-pages/CourseCurriculumPage";
import SubmitCoursePage from "./manage-course-pages/SubmitPage";
import { ManageCoursePageProps } from "./manage-course-pages/manageCourseTypes";
import { Course } from "../../interfaces/course";
import { useNavigate, useParams } from "react-router-dom";
import axios from "axios";
import Loader from "../../components/common/Loader";
import { useAuthContext } from "../../contexts/auth/authReducer";

interface CourseTab extends Tab {
  value: "course_info" | "course_curriculum" | "submit";
}

const tabs: CourseTab[] = [
  { label: "Course Info", value: "course_info" },
  { label: "Course Curriculum", value: "course_curriculum" },
  { label: "Submit", value: "submit" },
];

const manageCoursePages: {
  [key in CourseTab["value"]]: React.FC<ManageCoursePageProps>;
} = {
  course_info: CourseInfo,
  course_curriculum: CourseCurriculum,
  submit: SubmitCoursePage,
};

const ManageCoursePage = () => {
  const [activeTab, setActiveTab] = useState<CourseTab["value"]>("course_info");
  const [managingCourse, setManagingCourse] = useState<Course | null>(null);
  const [isCourseLoading, setIsCourseLoading] = useState<boolean>(false);
  const { courseId } = useParams();
  const { user } = useAuthContext();
  const navigate = useNavigate();
  const mode: ManageCoursePageProps["mode"] = courseId
    ? "updating"
    : "creating";

  // fetch the course using the courseId from params, and the mode is updating else creating
  useEffect(() => {
    if (!courseId) {
      return;
    }

    const fetch = async () => {
      setIsCourseLoading(true);
      // try catch
      const { data } = await axios.get<Course>(`/api/Course/`, {
        params: {
          courseId,
        },
      });
      setManagingCourse(data);
      setIsCourseLoading(false);
    };

    fetch();
  }, [courseId]);

  if (!user) {
    navigate("/login");
  }

  if (isCourseLoading) {
    return <Loader />;
  }

  return (
    <div>
      <Tabs
        tabs={tabs}
        activeTab={activeTab}
        onClick={(tab: Tab) => setActiveTab(tab.value as CourseTab["value"])}
      />
      <div className="mt-5">
        {/* {manageCoursePages[activeTab]({
          mode: "creating",
        })} */}

        {activeTab === "course_info" && (
          <CourseInfo
            mode={mode}
            initialCourse={managingCourse}
            onSave={(course) => setManagingCourse(course)}
          />
        )}
        {activeTab === "course_curriculum" && (
          <CourseCurriculum
            initialCourse={managingCourse}
            onSave={(course) => setManagingCourse(course)}
            mode={mode}
          />
        )}
        {activeTab === "submit" && (
          <SubmitCoursePage
            initialCourse={managingCourse}
            onSave={(course) => setManagingCourse(course)}
            mode={mode}
          />
        )}
      </div>
    </div>
  );
};

export default ManageCoursePage;
