import React, { FC, useEffect, useState } from "react";
import { ManageCoursePageProps } from "./manageCourseTypes";
import useSections from "../../../hooks/fetchers/useSections";
import SectionEdit from "../../../components/educators/SectionEdit";
import { Section } from "../../../interfaces/course";
import {
  closestCenter,
  closestCorners,
  DndContext,
  PointerSensor,
  useSensor,
  useSensors,
} from "@dnd-kit/core";
import {
  arrayMove,
  SortableContext,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";
import { cornersOfRectangle } from "@dnd-kit/core/dist/utilities/algorithms/helpers";
import Sortable from "../../../components/common/dnd/Sortable";

interface CourseCurriculumProps extends ManageCoursePageProps {}

const CourseCurriculum: FC<CourseCurriculumProps> = ({
  mode,
  onSave,
  initialCourse,
}) => {
  const { sections, isLoading, error } = useSections(
    initialCourse?.id as number
  );

  const sensors = useSensors(useSensor(PointerSensor));

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
        orderId: prev.length,
      },
    ]);
  };

  const handleDragEnd = (event: any) => {
    const { active, over } = event;

    if (active.id === over.id) {
      return;
    }

    setNewSections((prev) => {
      const oldIndex = prev.findIndex((section) => section.id === active.id);
      const newIndex = prev.findIndex((section) => section.id === over.id);
      prev[oldIndex].orderId = newIndex;
      prev[newIndex].orderId = oldIndex;
      return arrayMove(prev, oldIndex, newIndex);
    });
  };

  return (
    <DndContext
      sensors={sensors}
      collisionDetection={closestCenter}
      onDragEnd={handleDragEnd}
    >
      <SortableContext
        items={newSections}
        strategy={verticalListSortingStrategy}
      >
        <div className="flex flex-col  gap-3 w-2/3 mx-auto">
          {newSections?.map((section) => (
            <Sortable id={section.id} key={section.id}>
              <SectionEdit initialSection={section} />
            </Sortable>
          ))}
          <button className="btn mt-10 w-full" onClick={handleAddSection}>
            Add Section
          </button>
        </div>
      </SortableContext>
    </DndContext>
  );
};

export default CourseCurriculum;
