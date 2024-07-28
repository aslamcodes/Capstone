import React, { act, FC, useEffect, useState } from "react";
import { ManageCoursePageProps } from "./manageCourseTypes";
import useSections from "../../../hooks/fetchers/useSections";
import SectionEdit from "../../../components/educators/SectionEdit";
import { Section } from "../../../interfaces/course";
import {
  closestCenter,
  closestCorners,
  DndContext,
  DragEndEvent,
  PointerSensor,
  TouchSensor,
  useSensor,
  useSensors,
} from "@dnd-kit/core";
import {
  arrayMove,
  SortableContext,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";
import Sortable from "../../../components/common/dnd/Sortable";
import axios from "axios";
import { useAuthContext } from "../../../contexts/auth/authReducer";
import Loader from "../../../components/common/Loader";

interface CourseCurriculumProps extends ManageCoursePageProps {}

const CourseCurriculum: FC<CourseCurriculumProps> = ({
  mode,
  onSave,
  initialCourse,
}) => {
  const { user } = useAuthContext();

  const { sections, isLoading, error } = useSections(
    initialCourse?.id as number
  );

  const sensors = useSensors(useSensor(PointerSensor), useSensor(TouchSensor));

  const [newSections, setNewSections] = useState<Section[]>(sections || []);
  const [isAddingSection, setIsAddingSection] = useState<boolean>(false);
  // update the sections when the sections are fetched
  useEffect(() => {
    if (sections) {
      // sort by orderid

      setNewSections((prev) =>
        [...sections, ...prev].sort((a, b) => a.orderId - b.orderId)
      );
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

  const handleOrderChange = async (sectionId: number, orderId: number) => {
    const section = newSections.find((section) => section.id === sectionId);
    console.log(sectionId);
    await axios.put(
      "/api/Section",
      {
        ...section,
        orderId,
      },
      {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      }
    );
  };

  const handleAddSection = async () => {
    // find a better way to generate unique id instread of string
    setIsAddingSection(true);
    var { data: newSection } = await axios.post<Section>(
      "/api/Section",
      {
        name: "Dummy Name",
        description: "Dummyguy",
        courseId: initialCourse?.id,
        orderId: newSections.length,
      },
      {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      }
    );

    setIsAddingSection(false);

    setNewSections((prev) => [...prev, newSection]);
  };

  const handleDragEnd = (event: DragEndEvent) => {
    const { active, over } = event;

    if (!over) {
      return;
    }

    if (active.id === over?.id) {
      return;
    }

    setNewSections((prev) => {
      const oldIndex = prev.findIndex((section) => section.id === active.id);
      const newIndex = prev.findIndex((section) => section.id === over.id);
      prev[oldIndex].orderId = newIndex;
      prev[newIndex].orderId = oldIndex;
      handleOrderChange(prev[oldIndex].id, newIndex);
      handleOrderChange(prev[newIndex].id, oldIndex);
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
              <h1>
                {section.id} {section.orderId}
              </h1>
              <SectionEdit initialSection={section} />
            </Sortable>
          ))}
          <button className="btn mt-10 w-full" onClick={handleAddSection}>
            {isAddingSection ? <Loader></Loader> : <h1>Add Section</h1>}
          </button>
        </div>
      </SortableContext>
    </DndContext>
  );
};

export default CourseCurriculum;
