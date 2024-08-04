import type { Section } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useSections(courseId: number | string) {
  const {
    data: sections,
    isLoading,
    error,
  } = useFetchAxios<Section[], any>({
    url: `/api/course/sections?courseId=${courseId}`,
  });

  return { sections, isLoading, error };
}
