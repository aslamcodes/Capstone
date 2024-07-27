import React, { FC } from "react";
import { ManageCoursePageProps } from "./manageCourseTypes";
import Form, {
  FormButton,
  FormError,
  FormGroup,
  FormLabel,
  FormTitle,
} from "../../../components/common/Form";
import { useForm, SubmitHandler } from "react-hook-form";
import axios from "axios";
import { useAuthContext } from "../../../contexts/auth/authReducer";
import { Course } from "../../../interfaces/course";

type Inputs = {
  name: string;
  description: string;
  price: number;
  level: string;
};

interface CourseInfoProps extends ManageCoursePageProps {}

const CourseInfo: FC<CourseInfoProps> = ({ onSave }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<Inputs>();

  const { user } = useAuthContext();

  const onSubmit: SubmitHandler<Inputs> = async (data) => {
    console.log({ ...data, educatorId: user?.id });

    const res = await axios.post<Course>(
      "/api/Course",
      { ...data, educatorId: user?.id },
      {
        headers: { Authorization: `Bearer ${user?.token}` },
      }
    );

    onSave(res.data);
  };

  return (
    <Form onSubmit={handleSubmit(onSubmit)}>
      <FormTitle>Course Details</FormTitle>
      <FormGroup>
        <FormLabel>Course Name</FormLabel>
        <input
          type="text"
          placeholder="Course Name"
          className="input input-bordered"
          {...register("name", { required: true })}
        />
      </FormGroup>
      <FormGroup>
        <FormLabel>Course Description</FormLabel>
        <textarea
          placeholder="Course Description"
          className="textarea textarea-bordered"
          {...register("description", { required: true })}
        />
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
      <FormButton title="Save" />
    </Form>
  );
};

export default CourseInfo;
