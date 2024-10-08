import { FC, useCallback, useState } from "react";
import { ManageCoursePageProps } from "./manageCourseTypes";
import Form, {
  FormButton,
  FormError,
  FormGroup,
  FormLabel,
  FormTitle,
} from "../../../components/common/Form";
import {
  useForm,
  SubmitHandler,
  useFieldArray,
  FieldArray,
  FieldArrayPath,
} from "react-hook-form";
import axios from "axios";
import { useAuthContext } from "../../../contexts/auth/authReducer";
import { Course } from "../../../interfaces/course";
import Divider from "../../../components/common/divider";
import { ImImage } from "react-icons/im";
import Loader from "../../../components/common/Loader";
import { customToast } from "../../../utils/toast";
import { getErrorMessage } from "../../../utils/error";
import { IoClose } from "react-icons/io5";
import axiosInstance from "../../../utils/fetcher";

type Inputs = {
  name: string;
  description: string;
  price: number;
  level: string;
  objectives: string[];
  targetAudience: string[];
  prerequisites: string[];
};

interface CourseInfoProps extends ManageCoursePageProps {}

const CourseInfo: FC<CourseInfoProps> = ({ onSave, initialCourse, mode }) => {
  const { user } = useAuthContext();
  const [isSaving, setIsSaving] = useState(false);
  const isEditing = mode === "updating";

  const {
    register,
    handleSubmit,
    formState: { errors },
    control,
  } = useForm<Inputs>({
    defaultValues: {
      ...initialCourse,
      objectives: initialCourse?.courseObjective?.split("|"),
      prerequisites: initialCourse?.prerequisites?.split("|"),
      targetAudience: initialCourse?.targetAudience?.split("|"),
    },
  });

  const {
    append: appendObjective,
    fields: objectiveFields,
    remove: removeObjective,
  } = useFieldArray<Inputs>({
    control,
    name: "objectives",
  });

  const [courseImage, setCourseImage] = useState<File | null>(null);

  const [coursePreview, setCoursePreview] = useState<string | null>(
    initialCourse?.courseThumbnailPicture
      ? initialCourse?.courseThumbnailPicture + "?" + Date.now()
      : null
  );

  const {
    append: appendPrerequisites,
    fields: prerequisiteFields,
    remove: removePrerequisites,
  } = useFieldArray<Inputs>({
    control,
    name: "prerequisites",
  });

  const {
    append: appendTargetAudience,
    fields: targetAudienceFields,
    remove: removeTargetAudience,
  } = useFieldArray<Inputs>({
    control,
    name: "targetAudience",
  });

  const onSubmit: SubmitHandler<Inputs> = async (data) => {
    try {
      handleImageUpload();
      setIsSaving(true);
      const payload = {
        ...data,
        courseObjective: data.objectives.join("|"),
        prerequisites: data.prerequisites.join("|"),
        targetAudience: data.targetAudience.join("|"),
      };
      if (isEditing) {
        const res = await axiosInstance.put<Course>("/api/Course", payload, {
          headers: { Authorization: `Bearer ${user?.token}` },
        });
        setIsSaving(false);
        customToast("Course Details Updated!", {
          type: "success",
        });
        return onSave(res.data);
      }
      const res = await axiosInstance.post<Course>(
        "/api/Course",
        { ...payload, educatorId: user?.id },
        {
          headers: { Authorization: `Bearer ${user?.token}` },
        }
      );
      onSave(res.data);
      setIsSaving(false);
      customToast("Updates Saved!", {
        type: "success",
      });
    } catch (error) {
      setIsSaving(false);
      customToast(getErrorMessage(error, "Error Saving Course Details"), {
        type: "error",
      });
    }
  };

  const handleImageUpload = useCallback(async () => {
    try {
      if (!courseImage) return;
      setIsSaving(true);
      const formData = new FormData();
      formData.append("thumbnail", courseImage as Blob);
      await axiosInstance.put(
        `/api/Course/Course-Thumbnail?courseId=${initialCourse?.id}`,
        formData,
        {
          headers: {
            Authorization: `Bearer ${user?.token}`,
            "Content-Type": "multipart/form-data",
          },
        }
      );
    } catch (error: any) {
      customToast(
        error.response?.data?.message ||
          error.message ||
          "Failed to upload the image",
        { type: "error" }
      );
    } finally {
      setIsSaving(false);
    }
  }, [initialCourse?.id, user?.token, courseImage]);

  return (
    <div className="mb-24">
      <Form className="" onSubmit={handleSubmit(onSubmit)}>
        <FormTitle>Course Details</FormTitle>
        <FormGroup>
          <FormLabel>Course Image</FormLabel>
          {coursePreview ? (
            <img
              src={coursePreview}
              alt="Course Thumbnail"
              className="rounded-lg h-48 max-w-xl object-cover"
            />
          ) : (
            <div className="h-48 w-xl bg-base-content rounded-lg flex items-center justify-center object-contain">
              <ImImage className="text-base-300" size={30} />
            </div>
          )}
          <input
            type="file"
            className="file-input"
            accept="image/*"
            onChange={(e) => {
              const file = e.target.files?.[0];
              if (file) {
                setCourseImage(file);
                setCoursePreview(URL.createObjectURL(file));
              }
            }}
          />
        </FormGroup>
        <FormGroup>
          <FormLabel>Course Name</FormLabel>
          <input
            type="text"
            placeholder="Course Name"
            className="input input-bordered"
            {...register("name", { required: true })}
          />
          {errors.name && <FormError message="Name is Required" />}
        </FormGroup>
        <FormGroup>
          <FormLabel>Course Description</FormLabel>
          <textarea
            placeholder="Course Description"
            className="textarea textarea-bordered"
            {...register("description", { required: true })}
          />
          {errors.description && (
            <FormError message="Description is Required" />
          )}
        </FormGroup>
        <FormGroup>
          <FormLabel>Price</FormLabel>
          <select
            className="select select-bordered"
            {...register("price", { required: true })}
          >
            <option value={0}>Free</option>
            <option value={99}>99 Rs</option>
            <option value={199}>199 Rs</option>
            <option value={299}>299 Rs</option>
            <option value={399}>399 Rs</option>
            <option value={499}>499 Rs</option>
            <option value={599}>599 Rs</option>
            <option value={699}>699 Rs</option>
            <option value={799}>799 Rs</option>
            <option value={899}>899 Rs</option>
            <option value={999}>999 Rs</option>
            <option value={1999}>1999 Rs</option>
            <option value={3999}>3999 Rs</option>
            <option value={5999}>5999 Rs</option>
            <option value={13999}>13999 Rs</option>
          </select>
          {errors.price && <FormError message={"Please Determine a Price"} />}
        </FormGroup>
        <FormGroup>
          <FormLabel>Level</FormLabel>
          <select
            className="select select-bordered"
            {...register("level", { required: true })}
          >
            <option value="Beginner">Beginner</option>
            <option value="Intermediate">Intermediate</option>
            <option value="Advanced">Advanced</option>
          </select>
        </FormGroup>
        <Divider />
        <div className="space-y-2">
          <FormTitle>Course Objectives</FormTitle>
          <div className="alert text-base-content">
            Add your course objectives, these will be shown to the students
            before they enroll in your course.
          </div>
          {objectiveFields.length < 4 && (
            <p className="alert alert-warning text-error-content">
              Please Add atleast 4 objectives for your course
            </p>
          )}
          <FormGroup>
            {objectiveFields.map((field, index) => (
              <div key={field.id} className="flex gap-2">
                <input
                  type="text"
                  className="input input-bordered w-full"
                  {...register(`objectives.${index}` as const, {
                    required: true,
                  })}
                />
                <button
                  type="button"
                  onClick={() => removeObjective(index)}
                  className="btn btn-error"
                >
                  <IoClose color="#fff" />
                </button>
              </div>
            ))}
            <button
              type="button"
              onClick={() => appendObjective("")}
              className="btn"
            >
              Add a Objective
            </button>
          </FormGroup>
        </div>

        <Divider />
        <div className="space-y-2">
          <FormTitle>Prerequisites</FormTitle>
          <div className="alert text-base-content">
            List the required skills, experience, tools or equipment learners
            should have prior to taking your course. If there are no
            requirements, use this space as an opportunity to lower the barrier
            for beginners.
          </div>
          {prerequisiteFields.length < 1 && (
            <p className="alert alert-warning text-error-content">
              Please Add atleast 1 Prerequisite for your course
            </p>
          )}

          <FormGroup>
            {prerequisiteFields.map((field, index) => (
              <div key={field.id} className="flex gap-2">
                <input
                  type="text"
                  className="input input-bordered w-full"
                  {...register(`prerequisites.${index}` as const)}
                />
                <button
                  type="button"
                  onClick={() => removePrerequisites(index)}
                  className="btn btn-error"
                >
                  <IoClose color="#fff" />
                </button>
              </div>
            ))}
            <button
              type="button"
              onClick={() => appendPrerequisites("")}
              className="btn"
            >
              Add a Prerequisite
            </button>
          </FormGroup>
        </div>

        <Divider />
        <div className="space-y-2">
          <FormTitle>Who is this course for?</FormTitle>
          <div className="alert text-base-content">
            Write a clear description of the intended learners for your course
            who will find your course content valuable. This will help you
            attract the right learners to your course.
          </div>
          {targetAudienceFields.length < 1 && (
            <p className="alert alert-warning text-error-content">
              Please Add atleast 1 Target Audience for your course
            </p>
          )}
          <FormGroup>
            {targetAudienceFields.map((field, index) => (
              <div key={field.id} className="flex gap-2">
                <input
                  type="text"
                  className="input input-bordered w-full"
                  {...register(`targetAudience.${index}` as const)}
                />
                <button
                  type="button"
                  onClick={() => removeTargetAudience(index)}
                  className="btn btn-error"
                >
                  <IoClose color="#fff" />
                </button>
              </div>
            ))}
            <button
              type="button"
              onClick={() => appendTargetAudience("")}
              className="btn"
            >
              Add a Target Audience
            </button>
          </FormGroup>
        </div>

        <FormButton
          className="btn-primary fixed bottom-10 right-10 hover:bg-primary-700 hover:shadow-md"
          disabled={isSaving}
        >
          {isSaving ? <Loader size="md" type="dots"></Loader> : "Save"}
        </FormButton>
      </Form>
    </div>
  );
};

export default CourseInfo;
