import useSWRImmutable from "swr/immutable";
import { useAuthContext } from "../../contexts/auth/authReducer";
import axios from "axios";
import { Content, Course, Review } from "../../interfaces/course";
import { fetcherWithToken } from "../../utils/fetcher";
import useFetchAxios from "./useFetchAxios";
import { RevalidateCallback } from "swr/_internal";

export default function useReviews(courseId: number) {
  const { user } = useAuthContext();
  const { data, isLoading, error } = useFetchAxios<Review[], any>({
    url: "/api/Reviews/For-Courses",
    headers: { Authorization: `Bearer ${user?.token}` },
    params: { courseId },
  });

  return { reviews: data, isLoading, error };
}
