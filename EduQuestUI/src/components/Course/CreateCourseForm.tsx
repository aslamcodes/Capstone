import React, { FC, useState } from "react";
import Form, {
  FormError,
  FormGroup,
  FormLabel,
  FormTitle,
} from "../common/Form";
import { useForm } from "react-hook-form";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useCategories from "../../hooks/fetchers/useCategories";
import { customToast } from "../../utils/toast";
import { useNavigate } from "react-router-dom";

const CreateCourseForm: FC<{
  onClose: () => void;
}> = ({ onClose }) => {
  const {
    handleSubmit,
    formState: { errors },
    register,
  } = useForm();
  const [isLoading, setIsLoading] = useState<boolean>();
  const navigate = useNavigate();
  const { user } = useAuthContext();
  const {
    categories,
    isLoading: isCategoriesLoading,
    error: categoriesError,
  } = useCategories();

  const onSubmit = async (data: any) => {
    try {
      setIsLoading(true);
      var { data } = await axios.post(
        "/api/Course",
        { ...data, educatorId: user?.id },
        {
          headers: {
            Authorization: `Bearer ${user?.token}`,
          },
        }
      );
      customToast(
        "Course created successfully, redirecting to course management page"
      );

      setIsLoading(false);
      onClose();
      navigate(`/manage-course/${data.id}`);
    } catch (error) {
      console.log(error);
      onClose();
      customToast("Cannot create the course");
    }
  };

  return (
    <Form>
      <FormTitle>Create Course</FormTitle>
      <FormGroup>
        <FormLabel>Course Name</FormLabel>
        <input
          type="text"
          className="input input-bordered"
          {...register("name", { required: true })}
        />
      </FormGroup>
      <FormGroup>
        <FormLabel>Description</FormLabel>
        <textarea
          className="textarea textarea-bordered"
          {...register("description", { required: true })}
        />
      </FormGroup>
      <FormGroup>
        {isCategoriesLoading ? (
          <div>Loading...</div>
        ) : (
          <>
            <FormLabel>Category</FormLabel>
            <select
              className="select select-bordered"
              {...register("courseCategoryId", { required: true })}
            >
              {categories?.map((category) => (
                <option value={category.id}>{category.name}</option>
              ))}
            </select>
          </>
        )}
      </FormGroup>
      <FormGroup>
        <FormLabel>Price</FormLabel>
        <select
          className="select select-bordered"
          {...register("price", { required: true })}
        >
          <option value={0}>Free</option>
          <option value={100}>99 Rs</option>
          <option value={200}>199 Rs</option>
          <option value={300}>299 Rs</option>
          <option value={400}>399 Rs</option>
          <option value={500}>499 Rs</option>
        </select>
        {errors.price && <FormError message={"Please Determine a Price"} />}
      </FormGroup>

      <FormGroup>
        <FormLabel>Level</FormLabel>
        <select
          className="select select-bordered"
          {...register("level", { required: true })}
        >
          <option value="Begginer">Begginer</option>
          <option value="Intermediate">Intermediate</option>
          <option value="Advanced">Advanced</option>
        </select>
      </FormGroup>
      <button
        type="submit"
        className="btn btn-primary"
        onClick={handleSubmit(onSubmit)}
        disabled={
          isCategoriesLoading || !categories || categoriesError || isLoading
        }
      >
        Create
      </button>
    </Form>
  );
};

export default CreateCourseForm;
