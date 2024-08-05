import { Course } from "../interfaces/course";

export function getBadgeForStatus(status: Course["courseStatus"]): string {
  let map = {
    Live: "badge-primary",
    Draft: "badge-secondary",
    Archived: "badge-error",
    Outdated: "badge-warning",
    Review: "badge-info",
  };

  return map[status];
}
