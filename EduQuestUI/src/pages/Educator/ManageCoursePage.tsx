import React, { useState } from "react";
import { Tab } from "../../interfaces/common";
import Tabs from "../../components/common/Tabs";
import CourseInfo from "./manage-course-pages/CourseInfoPage";
import CourseCurriculum from "./manage-course-pages/CourseCurriculumPage";
import SubmitCoursePage from "./manage-course-pages/SubmitPage";
import { ManageCoursePageProps } from "./manage-course-pages/manageCourseTypes";
import { Course } from "../../interfaces/course";

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

  return (
    <div>
      <Tabs
        tabs={tabs}
        activeTab={activeTab}
        onClick={(tab: Tab) => {
          setActiveTab(tab.value as CourseTab["value"]);
        }}
      />
      <div className="mt-5">
        {/* {manageCoursePages[activeTab]({
          mode: "creating",
        })} */}

        {activeTab === "course_info" && (
          <CourseInfo
            mode="creating"
            initialCourse={managingCourse}
            onSave={(course) => setManagingCourse(course)}
          />
        )}
        {activeTab === "course_curriculum" && (
          <CourseCurriculum
            initialCourse={managingCourse}
            onSave={(course) => setManagingCourse(course)}
            mode="creating"
          />
        )}
        {activeTab === "submit" && (
          <SubmitCoursePage
            initialCourse={managingCourse}
            onSave={(course) => setManagingCourse(course)}
            mode="creating"
          />
        )}
      </div>
    </div>
  );
};

export default ManageCoursePage;
