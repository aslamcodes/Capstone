import useSWRImmutable from "swr/immutable";
import { fetcherWithToken } from "../../utils/fetcher";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { Course } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useRecomendedCoureses(studentId: number) {
  const { user } = useAuthContext();

  const {
    data,
    isLoading: coursesLoading,
    error,
  } = useFetchAxios<Course[], any>({
    url: `/api/Student/${
      user ? `recommended-courses?studentId=${studentId}` : `home-courses`
    }`,
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
  });

  return { courses: data, coursesLoading, error };
}
