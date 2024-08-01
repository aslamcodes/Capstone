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
    [
      `/api/Student/${
        user ? `recommended-courses?studentId=${studentId}` : `home-courses`
      }`,
      user?.token,
    ],
    ([url, token]) => fetcherWithToken(url, token as string)
  );

  return { courses: data, coursesLoading, error };
}
