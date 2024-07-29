import { useAuthContext } from "../../contexts/auth/authReducer";
import { Validity } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useCourseValidity(courseId: number) {
  const { user } = useAuthContext();

  const { data, error, isLoading } = useFetchAxios<Validity, any>({
    url: "/api/Course/Get-Validity",
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
    params: {
      courseId,
    },
  });

  return { validity: data, error, isLoading };
}
