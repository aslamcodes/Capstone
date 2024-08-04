import { useAuthContext } from "../../contexts/auth/authReducer";
import { Course, CourseStatusEnum } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useCoursesByStatus(status: CourseStatusEnum) {
  const { user } = useAuthContext();
  const {
    data,
    isLoading: coursesLoading,
    error,
  } = useFetchAxios<Course[], any>({
    url: `/api/Course/courses-by-status?status=${status}`,
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
  });

  return { courses: data, coursesLoading, error };
}
