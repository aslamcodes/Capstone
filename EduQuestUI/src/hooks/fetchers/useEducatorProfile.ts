import { useAuthContext } from "../../contexts/auth/authReducer";
import useFetchAxios from "./useFetchAxios";
import { EducatorProfile } from "../../interfaces/common";

export default function useEducatorProfile(educatorId: number) {
  const { user } = useAuthContext();

  const {
    data: educatorProfile,
    isLoading,
    error,
  } = useFetchAxios<EducatorProfile, any>(
    {
      url: `/api/User/Educator-Profile?educatorId=${educatorId}`,
      headers: {
        Authorization: `Bearer ${user?.token}`,
      },
    },
    [educatorId]
  );

  return { educatorProfile, isLoading, error };
}
