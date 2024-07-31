import { FC } from "react";
import Skeleton from "../common/Skeleton";
import VideoPlayer from "./VideoPlayer";
import useVideoForContent from "../../hooks/fetchers/useVideo";
import { BiVideoOff } from "react-icons/bi";

interface ContentViewerProps {
  contentId: number | null;
}

const VideoViewer: FC<ContentViewerProps> = ({ contentId }) => {
  const { video, isLoading, error } = useVideoForContent(contentId as number);

  if (isLoading) {
    return <Skeleton />;
  }

  if (error) return <div>{error.response.data?.message}</div>;

  // if (!video || video.url === "") {
  //   return (
  //     <div className="w-full h-96 bg-black rounded flex flex-col items-center justify-center text-white">
  //       <BiVideoOff color="#fff" size={32} />
  //       <p>Author Have not yet uploaded video to this content</p>
  //     </div>
  //   );
  // }

  return (
    <div className="h-full">
      <VideoPlayer
        type="Youtube"
        url={
          "https://eduqueststorage.blob.core.windows.net/videos/23-videoplayback.mp4"
        }
      />
    </div>
  );
};

export default VideoViewer;
