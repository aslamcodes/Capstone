import axios from "axios";
import React, { FC, useState } from "react";
import { FaStar } from "react-icons/fa";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { toast } from "react-toastify";
import { customToast } from "../../utils/toast";
import useReviews from "../../hooks/fetchers/useReviews";

export const StarRating: FC<{
  rating: number;
  setRating: React.Dispatch<React.SetStateAction<number>>;
}> = ({ rating, setRating }) => {
  return (
    <div className="flex items-center gap-2">
      {[1, 2, 3, 4, 5].map((star) => {
        return (
          <FaStar
            className="start"
            style={{
              cursor: "pointer",
              color: rating >= star ? "gold" : "gray",
              fontSize: `35px`,
            }}
            onClick={() => {
              setRating(star);
            }}
          ></FaStar>
        );
      })}
    </div>
  );
};

const Review: FC<{ courseId: number }> = ({ courseId }) => {
  const [rating, setRating] = useState(1);
  const [review, setReview] = useState("");
  const { user } = useAuthContext();

  const handleSubmit = async () => {
    try {
      if (!review)
        return customToast("Review cannot be empty", { type: "error" });
      await axios.post(
        "/api/Reviews",
        {
          courseId,
          rating,
          reviewText: review,
          reviewedById: user?.id,
        },
        {
          headers: {
            Authorization: `Bearer ${user?.token}`,
          },
        }
      );
      customToast("Review submitted successfully", {
        type: "success",
      });
    } catch {
      customToast("Failed to submit review", {
        type: "error",
      });
    }
  };

  return (
    <>
      <div className="flex flex-col justify-center gap-4 p-4">
        <div className="flex flex-col gap-3">
          <h1 className="text-xl font-bold">Write a Review</h1>
          <StarRating rating={rating} setRating={setRating} />
          <textarea
            className="textarea textarea-bordered w-full"
            placeholder="Write a review"
            value={review}
            onChange={(e) => setReview(e.target.value)}
          ></textarea>
        </div>
        <button onClick={handleSubmit} className="btn btn-outline">
          Submit
        </button>
      </div>
      <CourseReviews courseId={courseId} />
    </>
  );
};

const CourseReviews: FC<{ courseId: number }> = ({ courseId }) => {
  const { reviews } = useReviews(courseId);
  return (
    <div>
      {reviews?.map((review) => {
        return (
          <div className="flex flex-col gap-2 p-4 border-b">
            <div className="flex flex-col gap-2">
              <h1 className="text-xl font-bold">
                {review.reviewedBy.firstName} {review.reviewedBy.lastName}
              </h1>
              <StarRating rating={review.rating} setRating={() => {}} />
            </div>
            <p>{review.reviewText}</p>
          </div>
        );
      })}
    </div>
  );
};

export default Review;
