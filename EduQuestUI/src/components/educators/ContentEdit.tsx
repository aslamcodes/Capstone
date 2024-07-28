import React, { FC } from "react";
import { Content } from "../../interfaces/course";
import { RiCloseFill, RiCloseLargeLine } from "react-icons/ri";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { GoVideo } from "react-icons/go";
import { TbArticle } from "react-icons/tb";
import { useNavigate } from "react-router-dom";

const ContentEdit: FC<{
  content: Content;
  onDelete: (contentId: number) => void;
}> = ({ content, onDelete }) => {
  const navigate = useNavigate();
  return (
    <div className="w-full bg-base-300 p-4 rounded-md flex items-center justify-between">
      <div className="flex gap-2 items-center">
        {content.contentType === "Video" ? <GoVideo /> : <TbArticle />}
        <div>{content.title}</div>
      </div>
      <div className="flex gap-3 items-center">
        <div className="hover:font-bold">
          {content.contentType === "Video" ? (
            <button className="flex gap-2 items-center">
              <GoVideo />
              <p>Upload Video</p>
            </button>
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
