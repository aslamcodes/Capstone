import React from "react";
import Form, { FormGroup, FormLabel, FormTitle } from "../common/Form";
import { useForm } from "react-hook-form";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useCategories from "../../hooks/fetchers/useCategories";

const CreateCourseForm = () => {
  const {
    handleSubmit,
    formState: { errors },
    register,
  } = useForm();

  const { user } = useAuthContext();
  const {
    categories,
    isLoading: isCategoriesLoading,
    error: categoriesError,
  } = useCategories();

  const onSubmit = async (data: any) => {
    await axios.post(
      "/api/Course",
      { ...data, educatorId: user?.id },
      {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      }
    );
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
        <input
          type="number"
          className="input input-bordered"
          {...register("price", { required: true })}
        />
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
        disabled={isCategoriesLoading || !categories || categoriesError}
      >
        Create
      </button>
    </Form>
  );
};

export default CreateCourseForm;
