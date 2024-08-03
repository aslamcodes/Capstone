import { useAuthContext } from "../../contexts/auth/authReducer";
import useFetchAxios from "./useFetchAxios";

interface Res {
  userOwnsCourse: boolean;
  studentId: number;
  courseId: number;
}

export default function useUserManagesCourse(courseId: number | null) {
  const { user } = useAuthContext();

  const { data, error, isLoading } = useFetchAxios<Res, any>({
    url: "/api/Student/user-manages-course",
    params: { courseId },
    headers: {
      Authorization: `Bearer ${user?.token}`,
    },
  });

  return { isUserManages: data?.userOwnsCourse, error, isLoading };
}
