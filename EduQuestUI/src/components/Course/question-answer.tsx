import axios from "axios";
import React, { FC, useState } from "react";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useQuestion from "../../hooks/fetchers/useQuestions";
import Loader from "../common/Loader";
import QuestionBox from "./QuestionDrop";

const QuestionAnswer: FC<{ contentId: number }> = ({ contentId }) => {
  const [question, setQuestion] = useState("");

  const { user } = useAuthContext();

  const { questions, isLoading, error } = useQuestion(contentId);

  const handleSubmit = async () => {
    await axios.post(
      `/api/Question`,
      {
        contentId: contentId,
        questionText: question,
        postedById: user?.id,
      },
      {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      }
    );
  };

  return (
    <div className="flex flex-col gap-4 p-4">
      <div className="space-y-3">
        <div className="text-xl font-bold">Questions</div>
        <textarea
          className="textarea textarea-bordered w-full"
          placeholder="Write a question"
          value={question}
          onChange={(e) => setQuestion(e.target.value)}
        ></textarea>
        <div>
          {isLoading ? (
            <Loader type="spinner" />
          ) : error ? (
            "Error loading questions"
          ) : (
            questions?.map((question) => (
              <QuestionBox key={question.id} question={question} />
            ))
          )}
        </div>
      </div>
      <button className="btn btn-outline" onClick={handleSubmit}>
        Submit
      </button>
    </div>
  );
};

export default QuestionAnswer;
