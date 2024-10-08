import { FC, useEffect, useState } from "react";
import SectionContent from "./SectionContent";
import { Content } from "../../interfaces/course";
import Loader from "../common/Loader";
import { customToast } from "../../utils/toast";
import axiosInstance from "../../utils/fetcher";

type SectionProps = {
  name: string;
  description: string;
  id: number;
  currentContentId: number | null;
  onContentChange: (contentId: number) => void;
};

const SectionDrop: FC<SectionProps> = ({
  name: title,
  description,
  currentContentId,
  id,
  onContentChange,
}) => {
  const [isVisited, setIsVisited] = useState(false);
  const [contents, setContents] = useState<Content[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchContents = async () => {
      try {
        setIsLoading(true);
        const { data } = await axiosInstance.get<Content[]>(
          `/api/Section/Contents?sectionId=${id}`
        );
        setContents(data);
      } catch {
        customToast("Cannot fetch the contents", {
          type: "error",
        });
      } finally {
        setIsLoading(false);
      }
    };
    isVisited && fetchContents();
  }, [isVisited]);

  return (
    <div
      className="collapse collapse-arrow bg-base-200 rounded-lg flex-shrink-0"
      onClick={() => setIsVisited(true)}
    >
      <input type="checkbox" />

      <div className="collapse-title text-lg md:text-xl font-medium">
        {title}
      </div>
      <div className="collapse-content">
        <p>{description}</p>
        <div className="mt-4">
          {isLoading && <Loader />}
          <div className="flex flex-col gap-2">
            {contents.length > 0 &&
              contents.map((content) => (
                <SectionContent
                  isActive={content.id === currentContentId}
                  contentId={content.id}
                  contentType={content.contentType}
                  key={content.id}
                  title={content.title}
                  onClick={(content) => onContentChange(content)}
                ></SectionContent>
              ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default SectionDrop;
