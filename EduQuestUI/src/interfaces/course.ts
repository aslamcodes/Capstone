export interface Content {
  id: number;
  title: string;
  sectionId: number;
  orderIndex: number;
  contentType: number;
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
}

export interface Section {
  name: string;
  description: string;
  id: number;
  orderId: number;
}
