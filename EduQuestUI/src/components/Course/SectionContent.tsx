import { FC } from "react";
import { GoVideo } from "react-icons/go";
import { TbArticle } from "react-icons/tb";

interface SectionContentProps {
  title: string;
  contentType: "Video" | "Article";
  contentId: number;
  onClick: (contentId: number) => void;
  isActive: boolean;
}

const SectionContent: FC<SectionContentProps> = ({
  title,
  contentType,
  contentId,
  isActive,
  onClick,
}) => {
  return (
    <div
      className={`font-bold bg-neutral-content rounded-md p-4 cursor-pointer ${
        isActive ? "bg-slate-400 " : ""
      }`}
      onClick={() => onClick(contentId)}
    >
      <div className="font-semibold flex items-center gap-2">
        {contentType === "Video" ? <GoVideo /> : <TbArticle></TbArticle>}
        {title}
      </div>
    </div>
  );
};

export default SectionContent;
