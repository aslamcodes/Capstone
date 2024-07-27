import useSWR from "swr";
import { Content, Section, Video } from "../../interfaces/common";
import { fetcher, fetcherWithToken } from "../../utils/fetcher";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useSWRImmutable from "swr/immutable";

export default function useVideoForContent(contentId: number) {
  const { user } = useAuthContext();

  const {
    data: content,
    isLoading,
    error,
  } = useSWRImmutable<Video, any>(
    [`/api/Video/?contentId=${contentId}`, user?.token],
    ([url, token]) => fetcherWithToken(url, token as string)
  );

  return { video: content, isLoading, error };
}
