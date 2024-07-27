import useSWRImmutable from "swr/immutable";
import { fetcherWithToken } from "../../utils/fetcher";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { Course } from "../../interfaces/course";

export default function useEducatorCourses() {
  const { user } = useAuthContext();
  const {
    data,
    isLoading: coursesLoading,
    error,
  } = useSWRImmutable<Course[], any>(
    ["/api/Course/Educator-Courses?educatorId=1", user?.token],
    ([url, token]) => fetcherWithToken(url, token as string)
  );

  return { courses: data, coursesLoading, error };
}
