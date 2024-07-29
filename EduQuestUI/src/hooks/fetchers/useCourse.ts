import useSWRImmutable from "swr/immutable";
import { useAuthContext } from "../../contexts/auth/authReducer";
import axios from "axios";
import { Content } from "../../interfaces/course";
import { fetcherWithToken } from "../../utils/fetcher";

export default function useCourse(courseId: number) {
  const { user } = useAuthContext();

  const { data, isLoading, error } = useSWRImmutable<Content[]>(
    ["/api/Course", user?.token],
    ([url, token]) => fetcherWithToken(url, token as string, { courseId })
  );

  return { contents: data, isLoading, error };
}
