import { useEffect, useState } from "react";
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
import { customToast } from "../../utils/toast";
import { getErrorMessage } from "../../utils/error";
import useUserManagesCourse from "../../hooks/fetchers/useUserManagesCourse";

interface CourseTab extends Tab {
  value: "course_info" | "course_curriculum" | "submit";
}

const tabs: CourseTab[] = [
  { label: "Course Info", value: "course_info" },
  { label: "Course Curriculum", value: "course_curriculum" },
  { label: "Submit", value: "submit" },
];

// const manageCoursePages: {
//   [key in CourseTab["value"]]: React.FC<ManageCoursePageProps>;
// } = {
//   course_info: CourseInfo,
//   course_curriculum: CourseCurriculum,
//   submit: SubmitCoursePage,
// };

const ManageCoursePage = () => {
  const [activeTab, setActiveTab] = useState<CourseTab["value"]>("course_info");
  const [managingCourse, setManagingCourse] = useState<Course | null>(null);
  const [isCourseLoading, setIsCourseLoading] = useState<boolean>(false);
  const [error, setError] = useState<any>(null);
  const { courseId } = useParams();
  const { user } = useAuthContext();
  const navigate = useNavigate();

  const { isUserManages, isLoading: checkingCourseOwnership } =
    useUserManagesCourse(Number(courseId));
  const mode: ManageCoursePageProps["mode"] = courseId
    ? "updating"
    : "creating";

  // fetch the course using the courseId from params, and the mode is updating else creating
  useEffect(() => {
    if (!courseId) {
      return;
    }

    const fetch = async () => {
      try {
        setIsCourseLoading(true);
        const { data } = await axios.get<Course>(`/api/Course/`, {
          params: {
            courseId,
          },
        });
        console.log(data);
        setManagingCourse(data);
        setIsCourseLoading(false);
      } catch (error) {
        setError(error);
        customToast(getErrorMessage(error, "Error saving course"), {
          type: "error",
        });
      } finally {
        setIsCourseLoading(false);
      }
    };

    fetch();
  }, [courseId]);

  if (!user) {
    navigate("/login");
    return;
  }

  if (isCourseLoading || checkingCourseOwnership) {
    return <Loader />;
  }

  if (error) {
    return (
      <p className="alert alert-error">
        {getErrorMessage(error, "Problem loading the course")}
      </p>
    );
  }

  if (!isUserManages) {
    return <p className="alert alert-error">Cannot Manage this course</p>;
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
