import axios from "axios";
import React, { useCallback, useEffect } from "react";
import { GoUpload } from "react-icons/go";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useVideoForContent from "../../hooks/fetchers/useVideo";
import { customToast } from "../../utils/toast";
import Loader from "../common/Loader";
import { BiLink } from "react-icons/bi";
import axiosInstance from "../../utils/fetcher";

const VideoEdit = ({ contentId }: { contentId: number }) => {
  const [videoFile, setVideoFile] = React.useState<File | null>(null);
  const { user } = useAuthContext();
  const { video } = useVideoForContent(contentId);
  const [videoUrl, setVideoUrl] = React.useState<string | null>(null);
  const [isUploading, setIsUploading] = React.useState(false);

  const handleUpload = useCallback(
    async (file: File) => {
      const formData = new FormData();
      formData.append("file", file);

      try {
        setIsUploading(true);
        const { data } = await axiosInstance.post(
          `/api/Video/get-upload-url`,
          {
            contentId,
            fileName: file.name,
          },
          {
            headers: {
              Authorization: `Bearer ${user?.token}`,
            },
          }
        );

        const { data: out } = await axiosInstance.put(data.uploadUrl, file, {
          headers: {
            "x-ms-blob-type": "BlockBlob",
            "Content-Type": file.type,
          },
        });

        const { data: res } = await axiosInstance.post(
          `/api/Video/complete-upload`,
          {
            contentId: contentId,
            fileName: file.name,
          },
          {
            headers: {
              Authorization: `Bearer ${user?.token}`,
            },
          }
        );
        setIsUploading(false);
        setVideoFile(out.url);
        customToast("Uploaded", {
          type: "success",
        });
      } catch (error) {
        customToast("Cannot upload the video", {
          type: "error",
        });
        setIsUploading(false);
      }
    },
    [videoFile]
  );

  useEffect(() => {
    if (videoFile) handleUpload(videoFile);
  }, [videoFile]);

  useEffect(() => {
    if (video?.url) {
      setVideoUrl(video.url);
    }
  }, [video?.url]);

  if (isUploading) {
    return <Loader type="bars" size="lg" />;
  }

  return (
    <div className="flex gap-4 items-center cursor-pointer">
      {videoUrl && (
        <a href={videoUrl} target="blank">
          <BiLink />
        </a>
      )}
      {videoFile && (
        <div className="">
          <p className="">{videoFile.name.slice(0, 20)}...</p>
        </div>
      )}

      <div className="relative flex items-center gap-2">
        <input
          className="absolute inset-0 w-full h-full opacity-0 "
          type="file"
          accept="video/*"
          onChange={(e) => {
            const file = e.target.files?.[0];
            if (file) {
              setVideoFile(file);
            }
          }}
        ></input>
        <GoUpload />
        <p>Upload</p>
      </div>
    </div>
  );
};

export default VideoEdit;
