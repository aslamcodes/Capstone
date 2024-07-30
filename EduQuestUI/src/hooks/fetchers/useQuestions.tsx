import { useAuthContext } from "../../contexts/auth/authReducer";
import { Question } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useQuestion(contentId: number | null) {
  const { user } = useAuthContext();

  const { data, isLoading, error } = useFetchAxios<Question[], any>({
    url: "/api/Question/For-Content",
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
    params: {
      contentId,
    },
  });

  return { questions: data, isLoading, error };
}
