export interface Order {
  id: number;
  userId: number;
  price: number;
  orderStatus: string;
  createdAt: string;
  orderedCourseId: number;
  processedAt: any;
  discountAmount: number;
  completedAt: any;
}
