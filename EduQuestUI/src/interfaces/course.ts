export interface Content {
  id: number;
  title: string;
  sectionId: number;
  orderIndex: number;
  contentType: "Video" | "Article";
  video: Video | null;
  article: Article | null;
}

export interface Video {
  contentId: number;
  durationHours: number;
  durationMinutes: number;
  durationSeconds: number;
  url: string;
}

export interface Article {
  id: number;
  contentId: number;
  title: string;
  description: string;
  body: string;
}

export interface Course {
  id: number;
  name: string;
  description: string;
  educatorId: number;
  level: "Begginer" | "Intermediate" | "Advanced";
  price: number;
  courseStatus: "Live" | "Draft" | "Archived" | "Outdated" | "Review";
}

export interface Section {
  name: string;
  description: string;
  id: number;
  orderId: number;
}

export interface Validity {
  isValid: boolean;
  criterias: [
    {
      criteria: string;
      isPassed: boolean;
    }
  ];
}

export interface CourseCategory {
  id: number;
  name: string;
  description: string;
}
