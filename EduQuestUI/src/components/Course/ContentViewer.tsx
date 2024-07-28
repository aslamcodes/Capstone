import { FC } from "react";
import Skeleton from "../common/Skeleton";
import VideoPlayer from "./VideoPlayer";
import useVideoForContent from "../../hooks/fetchers/useVideo";

interface ContentViewerProps {
  contentId: number | null;
}

const ContentViewer: FC<ContentViewerProps> = ({ contentId }) => {
  const { video, isLoading, error } = useVideoForContent(contentId as number);

  if (isLoading) {
    return <Skeleton />;
  }

  if (error) return <div>{error.response.data?.message}</div>;

  if (!video) {
    return <div>No Video</div>;
  }

  return (
    <div className="h-full">
      <VideoPlayer type="Youtube" url={video.url} />
      <h1></h1>
    </div>
  );
};

export default ContentViewer;
