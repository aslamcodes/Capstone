import axios from "axios";
import React, { FC, useEffect, useState } from "react";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useQuestion from "../../hooks/fetchers/useQuestions";
import Loader from "../common/Loader";
import QuestionBox from "./QuestionDrop";
import { BiError } from "react-icons/bi";
import { Question } from "../../interfaces/course";
import useUserProfile from "../../hooks/fetchers/useUserProfile";

const QuestionAnswer: FC<{ contentId: number }> = ({ contentId }) => {
  const [question, setQuestion] = useState("");

  const { user } = useAuthContext();
  const { user: userProfile } = useUserProfile();

  const { questions, isLoading, error } = useQuestion(contentId);

  const [questionsList, setQuestionsList] = useState<Question[]>([]);

  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (questions) setQuestionsList(questions);
  }, [questions]);

  const handleSubmit = async () => {
    try {
      setIsSubmitting(true);
      const { data } = await axios.post<Question>(
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
      setIsSubmitting(false);
      setQuestion("");
      setQuestionsList((prev) => [
        {
          ...data,
          postedBy: {
            firstName: userProfile?.firstName as string,
            lastName: userProfile?.lastName as string,
          },
        },
        ...prev,
      ]);
    } catch {}
  };

  if (!contentId)
    return (
      <div className="p-4 flex gap-2 items-center">
        <BiError />
        <p>Please select a content from sections</p>
      </div>
    );

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
        <button className="btn btn-outline" onClick={handleSubmit}>
          Submit
        </button>
        <div>
          {isLoading ? (
            <Loader type="spinner" />
          ) : error ? (
            "Error loading questions"
          ) : (
            questionsList?.map((question) => (
              <QuestionBox key={question.id} question={question} />
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default QuestionAnswer;
