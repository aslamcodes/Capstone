import useSWR from "swr";
import { Content, Section, Video } from "../../interfaces/course";
import { fetcher, fetcherWithToken } from "../../utils/fetcher";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useSWRImmutable from "swr/immutable";
import useFetchAxios from "./useFetchAxios";

export default function useVideoForContent(contentId: number) {
  const { user } = useAuthContext();

  const {
    data: content,
    isLoading,
    error,
  } = useFetchAxios<Video, any>({
    url: "/api/Video/",
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
    params: {
      contentId,
    },
  });

  return { video: content, isLoading, error };
}
