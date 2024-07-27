import React, { FC, useEffect, useState } from "react";
import SectionContent from "./SectionContent";
import { Content } from "../../interfaces/course";

type SectionProps = {
  name: string;
  description: string;
  id: number;
};

const SectionDrop: FC<SectionProps> = ({ name: title, description, id }) => {
  const [isVisited, setIsVisited] = useState(false);
  const [contents, setContents] = useState<Content[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    const fetchContents = async () => {
      setIsLoading(true);
      const res = await fetch(`/api/Section/Contents?sectionId=${id}`);
      const data = await res.json();
      setContents(data);
      setIsLoading(false);
    };
    isVisited && fetchContents();
  }, [isVisited]);

  return (
    <div
      className="collapse collapse-arrow bg-base-200"
      onClick={() => setIsVisited(true)}
    >
      <input type="checkbox" />

      <div className="collapse-title text-xl font-medium">{title}</div>
      <div className="collapse-content">
        <p>{description}</p>
        <div>
          {isLoading && <div>Loading...</div>}
          {contents.length > 0 &&
            contents.map((content) => (
              <SectionContent
                key={content.id}
                title={content.title}
              ></SectionContent>
            ))}
        </div>
      </div>
    </div>
  );
};

export default SectionDrop;
