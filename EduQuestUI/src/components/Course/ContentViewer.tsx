import React, { FC } from "react";
import useContent from "../../hooks/fetchers/useContent";
import { Content } from "../../interfaces/course";
import VideoViewer from "./VideoViewer";
import ArticleViewer from "./ArticleViewer";

interface ContentViewerProps {
  contentId: number | null;
}

const ContentViewer: FC<ContentViewerProps> = ({ contentId }) => {
  const { content, isLoading, error } = useContent(contentId);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!content) {
    return <div>No Content</div>;
  }

  console.log(content, isLoading, error);

  if (error) return <div>{error.response.data?.message}</div>;

  if (content.contentType === "Article") {
    return <ArticleViewer contentId={contentId} />;
  }

  return <VideoViewer contentId={contentId}></VideoViewer>;
};

export default ContentViewer;
