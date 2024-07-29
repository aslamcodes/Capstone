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
        <h1 className="text-2xl font-bold">{content.title}</h1>
      </div>
    </div>
  );
};

export default ContentViewer;
