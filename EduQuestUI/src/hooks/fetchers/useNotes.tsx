import { useAuthContext } from "../../contexts/auth/authReducer";
import { Notes } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useNotes(contentId: number) {
  const { user } = useAuthContext();

  const { data, error, isLoading } = useFetchAxios<Notes, any>(
    {
      url: `/api/Notes/Content-Notes?contentId=${contentId}`,
      headers: {
        Authorization: `Bearer ${user?.token}`,
      },
    },
    [contentId]
  );

  return { notes: data, error, isLoading };
}
