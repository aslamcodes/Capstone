import { useNavigate, useParams } from "react-router-dom";
import SectionDrop from "../../components/Course/SectionDrop";
import useSWR from "swr";
import { fetcher } from "../../utils/fetcher";
import VideoPlayer from "../../components/Course/VideoPlayer";
import { Content, Section, Video } from "../../interfaces/course";
import { useEffect, useState } from "react";
import useSections from "../../hooks/fetchers/useSections";
import useVideoForContent from "../../hooks/fetchers/useVideo";
import ContentViewer from "../../components/Course/ContentViewer";
import Loader from "../../components/common/Loader";

const CoursePage = () => {
  const { courseId } = useParams();

  const [currentContentId, setCurrentContentId] = useState<number | null>(null);

  const { isLoading, sections } = useSections(courseId as string);

  const { video } = useVideoForContent(currentContentId as number);

  if (isLoading) {
    return <Loader />;
  }

  function handleContentChange(contentId: number) {
    setCurrentContentId(contentId);
  }

  return (
    <div className="min-h-[70vh] grid grid-cols-1 md:grid-cols-3 gap-3">
      <div className="col-span-2">
        <ContentViewer contentId={currentContentId} />
      </div>
      <div className="flex flex-col gap-2 lg:max-h-screen lg:overflow-scroll no-scrollbar">
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
