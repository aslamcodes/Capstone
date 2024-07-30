import React, { FC } from "react";
import useArticle from "../../hooks/fetchers/useArticle";
import MDEditor from "@uiw/react-md-editor";

const ArticleViewer: FC<{ contentId: number }> = ({ contentId }) => {
  let { content, error, isLoading } = useArticle(contentId);

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!content) {
    return <div>No Content</div>;
  }

  if (error) return <div>{error.response.data?.message}</div>;

  return (
    <div>
      <MDEditor
        hideToolbar
        data-color-mode="light"
        preview="preview"
        value={content.body}
        className="min-h-[60vh]"
      >
        <MDEditor.Markdown />
      </MDEditor>
    </div>
  );
};

export default ArticleViewer;
