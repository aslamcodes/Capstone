import React, { FC, useEffect, useState } from "react";
import { ManageCoursePageProps } from "./manageCourseTypes";
import useSections from "../../../hooks/fetchers/useSections";
import SectionEdit from "../../../components/educators/SectionEdit";
import { Section } from "../../../interfaces/course";

interface CourseCurriculumProps extends ManageCoursePageProps {}

const CourseCurriculum: FC<CourseCurriculumProps> = ({
  mode,
  onSave,
  initialCourse,
}) => {
  const { sections, isLoading, error } = useSections(
    initialCourse?.id as number
  );

  const [newSections, setNewSections] = useState<Section[]>(sections || []);

  useEffect(() => {
    if (sections && newSections.length === 0) {
      setNewSections(sections);
    }
  }, [sections]);

  if (isLoading) {
    return (
      <div className="w-1/2 mx-auto flex flex-col gap-3">
        <div className="w-full skeleton h-10"></div>
        <div className="w-full skeleton h-10"></div>
        <div className="w-full skeleton h-10"></div>
        <div className="w-full skeleton h-10"></div>
      </div>
    );
  }

  if (error) {
    return <div>{JSON.stringify(error)}</div>;
  }

  const handleAddSection = () => {
    setNewSections((prev) => [
      ...prev,
      {
        name: "",
        description: "",
        id: 0,
        courseId: initialCourse?.id as number,
      },
    ]);
  };

  return (
    <div>
      <div className="flex flex-col items-center gap-3 w-2/3 mx-auto">
        {newSections?.map((section) => (
          <SectionEdit initialSection={section} />
        ))}
        <button className="btn mt-10 w-full" onClick={handleAddSection}>
          Add Section
        </button>
      </div>
    </div>
  );
};

export default CourseCurriculum;
