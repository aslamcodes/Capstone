import React, { FC } from "react";
import { Section } from "../../interfaces/course";
import { BiDownArrow, BiUpArrow } from "react-icons/bi";
import useContentsForSection from "../../hooks/fetchers/useContent";
import ContentEdit from "./ContentEdit";
import Sortable from "../common/dnd/Sortable";

interface SectionEditProps {
  initialSection: Section;
}

const SectionEdit: FC<SectionEditProps> = ({ initialSection }) => {
  const { contents, error, isLoading } = useContentsForSection(
    initialSection.id
  );

  if (isLoading) {
    return (
      <div>
        <div className="mx-auto skeleton h-5 w-1/2"></div>
      </div>
    );
  }

  if (error) return <div>Error: </div>;

  return (
    <div className="flex flex-col gap-4 w-full p-5 rounded-lg bg-base-100 border border-base-300">
      <div>
        <h3 className="font-semibold text-2xl">{initialSection.name}</h3>
        <p className="font-normal text-md">{initialSection.description}</p>
      </div>
      <div className="flex gap-5 justify-between bg-base-200 p-4 rounded-lg w-full">
        <div className="flex-grow">
          {contents?.map((content) => (
            <ContentEdit />
          ))}
          <button className="btn mt-2 btn-outline">
            Add a Section Content
          </button>
        </div>
        <div>
          <BiUpArrow />
          <BiDownArrow />
        </div>
      </div>
    </div>
  );
};

export default SectionEdit;
