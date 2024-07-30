import { useAuthContext } from "../../contexts/auth/authReducer";
import { Answer } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useAnswersForQuestion(questionId: number) {
  const { user } = useAuthContext();

  const { data, isLoading, error } = useFetchAxios<Answer[], any>({
    url: `/api/Answers/For-Question`,
    params: {
      questionId: questionId,
    },
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
  });

  return { answers: data, isLoading, error };
}
