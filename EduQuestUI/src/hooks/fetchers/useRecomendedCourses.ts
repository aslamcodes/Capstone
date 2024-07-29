import useSWRImmutable from "swr/immutable";
import { fetcherWithToken } from "../../utils/fetcher";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { Course } from "../../interfaces/course";

export default function useRecomendedCoureses(studentId: number) {
  const { user } = useAuthContext();
  const {
    data,
    isLoading: coursesLoading,
    error,
  } = useSWRImmutable<Course[], any>(
    [`/api/Student/recommended-courses?studentId=${studentId}`, user?.token],
    ([url, token]) => fetcherWithToken(url, token as string)
  );

  return { courses: data, coursesLoading, error };
}
