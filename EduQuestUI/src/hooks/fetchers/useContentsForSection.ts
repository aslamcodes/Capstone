import useSWRImmutable from "swr/immutable";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { Content } from "../../interfaces/course";
import { fetcherWithToken } from "../../utils/fetcher";

export default function useContentsForSection(sectionId: number) {
  const { user } = useAuthContext();

  const { data, isLoading, error } = useSWRImmutable<Content[]>(
    ["/api/Section/Contents", user?.token],
    ([url, token]) => fetcherWithToken(url, token as string, { sectionId })
  );

  return { contents: data, isLoading, error };
}
