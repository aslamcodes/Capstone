import React, { FC, useState } from "react";
import { FaStar } from "react-icons/fa";

export const StarRating = ({ rating, setRating }) => {
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
  const [rating, setRating] = useState(2);
  const [review, setReview] = useState("");

  const handleSubmit = async () => {};

  return (
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
  );
};

export default Review;
