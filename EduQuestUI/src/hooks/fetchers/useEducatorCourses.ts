import useSWRImmutable from "swr/immutable";
import { fetcherWithToken } from "../../utils/fetcher";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { Course } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useEducatorCourses(educatorId: number) {
  const { user } = useAuthContext();
  const {
    data,
    isLoading: coursesLoading,
    error,
  } = useFetchAxios<Course[], any>(
    {
      url: `/api/Course/Educator-Courses?educatorId=${educatorId}`,
      headers: {
        Authorization: `Bearer ${user?.token}`,
      },
    },
    [educatorId]
  );

  return { courses: data, coursesLoading, error };
}
