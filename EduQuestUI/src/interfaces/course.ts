import { UserProfile } from "./common";

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
  id: number;
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
  courseObjective: string;
  prerequisites: string;
  targetAudience: string;
  courseCategoryId: number;
  courseThumbnailPicture: string | null;
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

export interface Notes {
  contentId: number;
  noteContent: string;
  userId: number;
  id: number;
}

export interface Question {
  id: number;
  contentId: number;
  questionText: string;
  postedById: number;
  postedBy: UserProfile;
  postedOn: string;
  upvotes: number;
}

export interface Answer {
  id: number;
  questionId: number;
  answerText: string;
  answeredById: number;
  answeredOn: string;
  answeredBy: UserProfile;
}

export interface Review {
  courseId: number;
  rating: number;
  reviewText: string;
  reviewedById: number;
  reviewedBy: UserProfile;
}

export enum CourseStatusEnum {
  Live,
  Draft,
  Archived,
  Outdated,
  Review,
}
