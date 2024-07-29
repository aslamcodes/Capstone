import useSWRImmutable from "swr/immutable";
import { useAuthContext } from "../../contexts/auth/authReducer";
import axios from "axios";
import { Content, Course } from "../../interfaces/course";
import { fetcherWithToken } from "../../utils/fetcher";

export default function useCourse(courseId: number) {
  const { user } = useAuthContext();

  const { data, isLoading, error } = useSWRImmutable<Course>(
    ["/api/Course", user?.token],
    ([url, token]) => fetcherWithToken(url, token as string, { courseId })
  );

  return { course: data, isLoading, error };
}
