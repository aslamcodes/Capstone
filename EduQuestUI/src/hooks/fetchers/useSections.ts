import useSWR from "swr";
import { Section } from "../../interfaces/course";
import { fetcher } from "../../utils/fetcher";
import useSWRImmutable from "swr/immutable";

export default function useSections(courseId: number | string) {
  const {
    data: sections,
    isLoading,
    error,
  } = useSWRImmutable<Section[]>(
    `/api/course/sections?courseId=${courseId}`,
    fetcher
  );

  return { sections, isLoading, error };
}
