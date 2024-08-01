import { useAuthContext } from "../../contexts/auth/authReducer";
import { Article } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useArticle(contentId: number | null) {
  const { user } = useAuthContext();

  const { data, error, isLoading } = useFetchAxios<Article, any>({
    url: "/api/Article/ForContent",
    params: { contentId },
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
  });

  return { content: data, error, isLoading };
}
