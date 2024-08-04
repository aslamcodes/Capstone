import { FC } from "react";

interface VideoPlayerProps {
  url: string;
  type: "Youtube" | "Video";
}

const VideoPlayer: FC<VideoPlayerProps> = ({ url, type }) => {
  // if (type === "Youtube") {
  //   return (
  //     <iframe
  //       className="w-full h-full max-h-96 rounded-lg"
  //       src={url}
  //       title="Relaxing nasheeds to listen while studying"
  //       frameBorder="0"
  //       allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
  //       referrerPolicy="strict-origin-when-cross-origin"
  //       allowFullScreen
  //     ></iframe>
  //   );
  // }

  return (
    <video className="w-full h-full max-h-96 rounded-lg" controls>
      <source src={url} type="video/mp4" />
      Your browser does not support the video tag.
    </video>
  );
};

export default VideoPlayer;
