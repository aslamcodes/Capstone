import useSWR from "swr";
import { Content, Section, Video } from "../../interfaces/course";
import { fetcher, fetcherWithToken } from "../../utils/fetcher";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useSWRImmutable from "swr/immutable";
import useFetchAxios from "./useFetchAxios";
import { UserProfile } from "../../interfaces/common";

export default function useUserProfile() {
  const { user } = useAuthContext();

  const {
    data: content,
    isLoading,
    error,
  } = useFetchAxios<UserProfile, any>({
    url: "/api/User/",
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
    params: {
      userId: user?.id,
    },
  });

  return { user: content, isLoading, error };
}
