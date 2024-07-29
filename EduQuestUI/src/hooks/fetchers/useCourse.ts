import useSWRImmutable from "swr/immutable";
import { useAuthContext } from "../../contexts/auth/authReducer";
import axios from "axios";
import { Content, Course } from "../../interfaces/course";
import { fetcherWithToken } from "../../utils/fetcher";
import useFetchAxios from "./useFetchAxios";

export default function useCourse(courseId: number) {
  const { user } = useAuthContext();
  const { data, isLoading, error } = useFetchAxios({
    url: "/api/Course",
    headers: { Authorization: `Bearer ${user?.token}` },
    params: { courseId },
  });

  return { course: data, isLoading, error };
}
