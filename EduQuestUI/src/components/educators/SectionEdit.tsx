import React, { FC, useEffect, useState } from "react";
import { Content, Section } from "../../interfaces/course";
import useContentsForSection from "../../hooks/fetchers/useContent";
import ContentEdit from "./ContentEdit";
import { RiCloseLargeLine, RiDraggable } from "react-icons/ri";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import Form, { FormButton, FormError, FormGroup } from "../common/Form";
import { FieldValues, useForm } from "react-hook-form";
import Loader from "../common/Loader";
import Sortable from "../common/dnd/Sortable";

interface SectionEditProps {
  initialSection: Section;
  onDelete: (sectionId: number) => void;
}

const SectionEdit: FC<SectionEditProps> = ({ initialSection, onDelete }) => {
  const { user } = useAuthContext();
  // const { contents, isLoading, error } = useContentsForSection(
  //   initialSection.id
  // );

  const [newContents, setContents] = useState<Content[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  console.log(JSON.stringify(initialSection));

  const [showAddContentForm, setShowAddContentForm] = useState<boolean>(false);
  const [isContentLoading, setIsContentLoading] = useState<boolean>(false);

  useEffect(() => {
    const fetchContents = async () => {
      try {
        setIsLoading(true);
        const { data } = await axios.get<Content[]>("/api/Section/Contents", {
          params: { sectionId: initialSection.id },
          headers: { Authorization: `Bearer ${user?.token}` },
        });
        setContents(data);
        setIsLoading(false);
      } catch (error) {
        setIsLoading(false);
      }
    };

    fetchContents();
  }, []);

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

  async function handleAddContent(content: FieldValues) {
    setIsContentLoading(() => true);
    const { data: Content } = await axios.post<Content>(
      "/api/Content",
      {
        sectionId: initialSection.id,
        title: content.title,
        contentType: content.contentType,
      },
      {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      }
    );
    setContents((prev) => [...prev, Content]);
    setShowAddContentForm(() => false);
    setIsContentLoading(() => false);
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
            {newContents?.map((content) => (
              <ContentEdit content={content} />
            ))}
            {showAddContentForm ? (
              isContentLoading ? (
                <Loader></Loader>
              ) : (
                <AddContentForm
                  onAdd={handleAddContent}
                  onClose={() => {
                    setShowAddContentForm(() => false);
                  }}
                />
              )
            ) : (
              <button
                onClick={() => {
                  setShowAddContentForm(() => true);
                }}
                className="btn mt-2 btn-outline"
              >
                Add a Section Content
              </button>
            )}
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

interface ContentInputs {
  title: string;
  contentType: string;
}

const AddContentForm: FC<{
  onAdd: (content: FieldValues) => void;
  onClose: () => void;
}> = ({ onAdd, onClose }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ContentInputs>({});

  return (
    <Form onSubmit={handleSubmit(onAdd)}>
      <FormGroup>
        <label htmlFor="contentName">Content Name</label>
        <input
          className="input"
          type="text"
          id="contentName"
          {...register("title", { required: true })}
        />
        {errors.title && (
          <FormError message=" Content Name is required"></FormError>
        )}
      </FormGroup>

      <FormGroup>
        <label htmlFor="contentType">Content Type</label>
        <select
          className="select select-bordered"
          id="contentType"
          {...register("contentType")}
        >
          <option value="Video">Video</option>
          <option value="Article">Article</option>
        </select>
      </FormGroup>

      <FormGroup row>
        <FormButton type="submit" title="Add Content" />
        <FormButton type="button" title="Cancel" onClick={onClose} />
      </FormGroup>
    </Form>
  );
};

export default SectionEdit;
