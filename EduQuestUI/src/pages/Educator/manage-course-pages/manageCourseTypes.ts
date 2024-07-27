import { Course } from "../../../interfaces/course";

export interface ManageCoursePageProps {
  mode: "creating" | "updating";
  onSave: (Course: Course) => void;
  initialCourse?: Course | undefined | null;
}
