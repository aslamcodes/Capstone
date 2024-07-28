import React, { FC } from "react";
import { Content } from "../../interfaces/course";

const ContentEdit: FC<{ content: Content }> = ({ content }) => {
  return (
    <div className="w-full bg-base-300 p-4 rounded-md">{content.title}</div>
  );
};

export default ContentEdit;
