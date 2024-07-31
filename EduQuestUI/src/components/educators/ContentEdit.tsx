import React, { FC, useCallback, useEffect } from "react";
import { Content } from "../../interfaces/course";
import { RiCloseFill, RiCloseLargeLine } from "react-icons/ri";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { GoVideo } from "react-icons/go";
import { TbArticle } from "react-icons/tb";
import { useNavigate } from "react-router-dom";
import useVideoForContent from "../../hooks/fetchers/useVideo";
import { toast } from "react-toastify";

const ContentEdit: FC<{
  content: Content;
  onDelete: (contentId: number) => void;
}> = ({ content, onDelete }) => {
  const navigate = useNavigate();
  const [videoFile, setVideoFile] = React.useState<File | null>(null);
  const { user } = useAuthContext();

  const handleUpload = useCallback(
    async (file: File) => {
      const formData = new FormData();

      formData.append("file", file);

      try {
        const { data } = await axios.post(
          `/api/Video/get-upload-url`,
          {
            contentId: content?.id,
            fileName: file.name,
          },
          {
            headers: {
              Authorization: `Bearer ${user?.token}`,
            },
          }
        );

        const response = await fetch(data.uploadUrl, {
          method: "PUT",
          headers: {
            "x-ms-blob-type": "BlockBlob",
            "Content-Type": file.type,
          },
          body: file,
        });

        console.log(uploadData);
      } catch (error) {
        console.log(error);
      }
    },
    [videoFile]
  );

  useEffect(() => {
    if (videoFile) handleUpload(videoFile);
  }, [videoFile]);

  return (
    <div className="w-full bg-base-300 p-4 rounded-md flex items-center justify-between">
      <div className="flex gap-2 items-center">
        {content.contentType === "Video" ? <GoVideo /> : <TbArticle />}
        <div>{content.title}</div>
      </div>
      <div className="flex gap-3 items-center">
        <div className="hover:font-bold">
          {content.contentType === "Video" ? (
            <div className="flex gap-2 items-center relative cursor-pointer">
              {videoFile && (
                <div className="">
                  <p className="">{videoFile.name.slice(0, 20)}...</p>
                </div>
              )}
              <input
                className="absolute inset-0 w-full h-full opacity-0 "
                type="file"
                // accept="video/*"
                onChange={(e) => {
                  const file = e.target.files?.[0];
                  if (file) {
                    setVideoFile(file);
                  }
                }}
              ></input>
              <GoVideo />
              <p>Upload Video</p>
            </div>
          ) : (
            <button
              className="flex gap-2 items-center"
              onClick={() => {
                navigate(`/manage-course/content/article/${content.id}`);
              }}
            >
              <TbArticle />
              <p>Edit Content</p>
            </button>
          )}
        </div>
        <button onClick={() => onDelete(content.id)}>
          <RiCloseFill className="scale-125" />
        </button>
      </div>
    </div>
  );
};

export default ContentEdit;
