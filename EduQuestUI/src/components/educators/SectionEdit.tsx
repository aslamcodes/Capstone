import React, { FC } from "react";
import { Section } from "../../interfaces/course";
import useContentsForSection from "../../hooks/fetchers/useContent";
import ContentEdit from "./ContentEdit";
import { RiCloseLargeLine, RiDraggable } from "react-icons/ri";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";

interface SectionEditProps {
  initialSection: Section;
  onDelete: (sectionId: number) => void;
}

const SectionEdit: FC<SectionEditProps> = ({ initialSection, onDelete }) => {
  const { user } = useAuthContext();
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

  async function handleDeleteSection(sectionId: number) {
    await axios.delete("/api/Section", {
      params: { sectionId },
      headers: { Authorization: `Bearer ${user?.token}` },
    });
  }

  return (
    <div className="flex p-5  bg-base-100 border border-base-300 rounded-lg">
      <RiDraggable className="scale-150 m-2" />
      <div className="flex flex-col gap-4 w-full">
        <div>
          <h3 className="font-semibold text-2xl">{initialSection.name}</h3>
          <p className="font-normal text-md">{initialSection.description}</p>
        </div>
        <div className="flex gap-5 justify-between bg-base-200 p-4 rounded-lg w-full">
          <div className="flex-grow">
            {contents?.map((content) => (
              <ContentEdit content={content} />
            ))}
            <button className="btn mt-2 btn-outline">
              Add a Section Content
            </button>
          </div>
        </div>
      </div>
      <div>
        <button
          onClick={(e) => {
            e.stopPropagation();
            handleDeleteSection(initialSection.id);
            onDelete(initialSection.id);
          }}
        >
          <RiCloseLargeLine className="hover:scale-125 transition-transform cursor-pointer" />
        </button>
      </div>
    </div>
  );
};

export default SectionEdit;
