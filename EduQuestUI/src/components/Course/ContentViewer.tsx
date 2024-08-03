import React, { FC } from "react";
import useContent from "../../hooks/fetchers/useContent";
import VideoViewer from "./VideoViewer";
import ArticleViewer from "./ArticleViewer";
import { FcNoVideo } from "react-icons/fc";
import { getErrorMessage } from "../../utils/error";
import { GrArticle } from "react-icons/gr";
import { BiVideo } from "react-icons/bi";
import { MdArticle } from "react-icons/md";

interface ContentViewerProps {
  contentId: number | null;
}

const ContentViewer: FC<ContentViewerProps> = ({ contentId }) => {
  const { content, isLoading, error } = useContent(contentId);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!content) {
    return (
      <div className="w-full h-full max-h-96 rounded-lg bg-base-content text-base-100 flex items-center justify-center font-bold">
        <div className="flex flex-col items-center justify-center">
          <FcNoVideo color="#fff" size={32} />
          <p>No Content</p>
          <i className="font-light">
            Please select a content from the sections
          </i>
        </div>
      </div>
    );
  }

  if (error)
    return <div className="alert alert-error">{getErrorMessage(error)}</div>;

  return (
    <div>
      <div className="space-y-4">
        <div>
          {content.contentType === "Article" ? (
            <ArticleViewer contentId={contentId as number} />
          ) : (
            <VideoViewer contentId={contentId} />
          )}
        </div>
        <div className="flex items-center pb-8 gap-1">
          {content.contentType === "Article" ? (
            <MdArticle size={28} />
          ) : (
            <BiVideo size={24} className="mt-1" />
          )}
          <h1 className="text-2xl font-bold">{content.title}</h1>
        </div>
      </div>
    </div>
  );
};

export default ContentViewer;
