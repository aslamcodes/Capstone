import { FC, useEffect, useState } from "react";
import { ManageCoursePageProps } from "./manageCourseTypes";
import useSections from "../../../hooks/fetchers/useSections";
import SectionEdit from "../../../components/educators/SectionEdit";
import { Section } from "../../../interfaces/course";
import {
  closestCenter,
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
import Form, {
  FormButton,
  FormError,
  FormGroup,
  FormTitle,
} from "../../../components/common/Form";
import { FieldValues, useForm } from "react-hook-form";
import axiosInstance from "../../../utils/fetcher";

interface CourseCurriculumProps extends ManageCoursePageProps {}

const CourseCurriculum: FC<CourseCurriculumProps> = ({ initialCourse }) => {
  const { user } = useAuthContext();

  const { sections, isLoading, error } = useSections(
    initialCourse?.id as number
  );

  const [newSections, setNewSections] = useState<Section[]>(sections || []);
  const [isAddingSection, setIsAddingSection] = useState<boolean>(false);
  const [showAddSectionForm, setShowAddSectionForm] = useState<boolean>(false);

  const sensors = useSensors(
    useSensor(PointerSensor, {
      activationConstraint: {
        distance: 10,
      },
    }),
    useSensor(TouchSensor)
  );

  // update the sections when the sections are fetched
  useEffect(() => {
    if (sections) {
      setNewSections(() => sections.sort((a, b) => a.orderId - b.orderId));
    }
  }, [sections]);

  if (isLoading) {
    // skeleton loader
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
    return (
      <p className="alert alert-error">
        {error.response?.data?.message || error.message}
      </p>
    );
  }

  const handleOrderChange = async (sectionId: number, orderId: number) => {
    const section = newSections.find((section) => section.id === sectionId);

    await axiosInstance.put(
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

  const handleAddSection = async (data: any) => {
    // find a better way to generate unique id instread of string
    setIsAddingSection(true);
    var { data: newSection } = await axiosInstance.post<Section>(
      "/api/Section",
      {
        name: data.name,
        description: data.description,
        courseId: initialCourse?.id,
        orderId: newSections.length,
      },
      {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      }
    );

    setShowAddSectionForm(false);
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
        <div className="space-y-3 lg:w-2/3 mx-auto">
          {newSections?.map((section) => (
            <Sortable id={section.id} key={section.id}>
              <SectionEdit
                initialSection={section}
                onDelete={(sectionId) => {
                  setNewSections((prev) =>
                    prev.filter((section) => section.id !== sectionId)
                  );
                }}
              />
            </Sortable>
          ))}
          {isAddingSection && <div className="w-full skeleton h-10"></div>}
          {showAddSectionForm && (
            <AddSectionForm
              onSubmit={(data) => {
                handleAddSection(data);
              }}
            />
          )}
          {!showAddSectionForm && (
            <button
              className="btn btn-sm md:btn-md mt-10 w-full"
              onClick={() => setShowAddSectionForm(true)}
            >
              Add Section
            </button>
          )}
        </div>
      </SortableContext>
    </DndContext>
  );
};

interface AddSectionInputs {
  name: string;
  description: string;
}

const AddSectionForm: FC<{ onSubmit: (data: FieldValues) => void }> = ({
  onSubmit,
}) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<AddSectionInputs>();

  return (
    <Form
      onSubmit={handleSubmit(onSubmit)}
      className="border border-neutral-content p-4 rounded-lg"
    >
      <FormTitle>Add Section</FormTitle>
      <FormGroup>
        <label htmlFor="name">Section Name</label>
        <input
          type="text"
          placeholder="Section Name"
          className="input input-bordered"
          {...register("name", { required: true })}
        />
        {errors.name && <FormError message="Name is Required" />}
      </FormGroup>
      <FormGroup>
        <label htmlFor="description">Section Description</label>
        <textarea
          placeholder="Section Description"
          className="textarea textarea-bordered"
          {...register("description", { required: true })}
        />
        {errors.description && <FormError message="Description is Required" />}
      </FormGroup>
      <FormGroup row>
        <FormButton type="button" title="Cancel" />
        <FormButton title="Add" />
      </FormGroup>
    </Form>
  );
};

export default CourseCurriculum;
