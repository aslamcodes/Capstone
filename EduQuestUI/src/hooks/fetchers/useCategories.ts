import { CourseCategory } from "../../interfaces/course";
import useFetchAxios from "./useFetchAxios";

export default function useCategories() {
  const { data, error, isLoading } = useFetchAxios<CourseCategory[], any>({
    url: "/api/CourseCategory",
  });

  return { categories: data, error, isLoading };
}
