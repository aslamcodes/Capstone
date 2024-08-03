import { Link, useNavigate, useParams } from "react-router-dom";
import SectionDrop from "../../components/Course/SectionDrop";
import { useState } from "react";
import useSections from "../../hooks/fetchers/useSections";
import Loader from "../../components/common/Loader";
import ContentViewer from "../../components/Course/ContentViewer";
import ContentTabs from "../../components/Course/ContentTabs";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useUserOwnsCourse from "../../hooks/fetchers/useUserOwnsCourse";

const CoursePage = () => {
  const { courseId } = useParams();
  const { user } = useAuthContext();
  const navigate = useNavigate();
  const [currentContentId, setCurrentContentId] = useState<number | null>(null);

  const { isUserOwns, isLoading: checkingCourseOwnership } = useUserOwnsCourse(
    Number(courseId)
  );
  const { isLoading, sections } = useSections(courseId as string);

  if (isLoading || checkingCourseOwnership) {
    return <Loader />;
  }

  function handleContentChange(contentId: number) {
    setCurrentContentId(contentId);
  }

  if (!user) {
    navigate("/login");
    return;
  }

  if (!isUserOwns) {
    return (
      <div>
        <div className="alert alert-error">
          Please Purchase the course to watch its content
        </div>
        <Link to={`/course-description/${courseId}`}>
          <button className="btn btn-block">Go to Course</button>
        </Link>
      </div>
    );
  }

  return (
    <div className="min-h-[70vh] grid grid-cols-1 md:grid-cols-3 gap-4">
      <div className="col-span-2 h-full">
        <ContentViewer contentId={currentContentId} />
        <ContentTabs contentId={currentContentId} courseId={Number(courseId)} />
      </div>
      <div className="flex flex-col gap-2 lg:max-h-screen lg:overflow-scroll no-scrollbar lg:sticky lg:top-10">
        {sections ? (
          sections.map((section) => (
            <SectionDrop
              name={section.name}
              description={section.description}
              id={section.id}
              onContentChange={handleContentChange}
            />
          ))
        ) : (
          <h1>This Course Dont have sections</h1>
        )}
      </div>
    </div>
  );
};

export default CoursePage;
