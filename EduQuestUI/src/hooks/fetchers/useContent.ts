import { useAuthContext } from "../../contexts/auth/authReducer";
import { Content } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useContent(contentId: number | null) {
  const { user } = useAuthContext();

  const { data, error, isLoading } = useFetchAxios<Content, any>({
    url: "/api/Content",
    params: { contentId },
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
  });

  return { content: data, error, isLoading };
}
