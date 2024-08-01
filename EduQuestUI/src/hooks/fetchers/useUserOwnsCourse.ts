import { useAuthContext } from "../../contexts/auth/authReducer";
import useFetchAxios from "./useFetchAxios";

interface Res {
  userOwnsCourse: boolean;
  studentId: number;
  courseId: number;
}

export default function useUserOwnsCourse(courseId: number | null) {
  const { user } = useAuthContext();

  const { data, error, isLoading } = useFetchAxios<Res, any>({
    url: "/api/Student/user-owns-course",
    params: { courseId },
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
  });

  return { isUserOwns: data?.userOwnsCourse, error, isLoading };
}
