import { FC, useState } from "react";
import { Answer, Question } from "../../interfaces/course";
import { GoCommentDiscussion } from "react-icons/go";
import useAnswersForQuestion from "../../hooks/fetchers/useAnswers";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import useUserProfile from "../../hooks/fetchers/useUserProfile";
import { customToast } from "../../utils/toast";
import axiosInstance from "../../utils/fetcher";

const Answers: FC<{ questionId: number }> = ({ questionId }) => {
  const { answers, isLoading, error } = useAnswersForQuestion(questionId);
  const [localAnswers, setLocalAnswers] = useState<Answer[]>([]);
  const [answer, setAnswer] = useState<string>("");
  const [isAnswering, setIsAnswering] = useState(false);
  const { user } = useAuthContext();
  const { user: userProfile } = useUserProfile();

  const handleAnswer = async () => {
    try {
      setIsAnswering(true);
      const { data } = await axiosInstance.post<Answer>(
        "/api/Answers/For-Question",
        {
          answerText: answer,
          questionId: questionId,
          answeredById: user?.id,
        },
        {
          headers: {
            Authorization: `Bearer ${user?.token}`,
          },
        }
      );

      setLocalAnswers([
        {
          ...data,
          answeredBy: {
            firstName: userProfile?.firstName as string,
            lastName: userProfile?.lastName as string,
          },
        },
        ...localAnswers,
      ]);
    } catch (error) {
      customToast("Failed to answer question", { type: "error" });
    } finally {
      setIsAnswering(false);
    }
  };

  if (isLoading) return <div>Loading...</div>;

  if (error || !answers) return <div>Error loading answers</div>;

  return (
    <div className="">
      <div className="space-y-2 mb-2">
        <div>
          <textarea
            className="textarea textarea-bordered w-full"
            placeholder="Write your Answer"
            value={answer}
            onChange={(e) => setAnswer(e.target.value)}
          ></textarea>
          <button
            className="btn btn-outline"
            onClick={handleAnswer}
            disabled={isAnswering}
          >
            Answer
          </button>
        </div>
        {[...localAnswers, ...(answers as Answer[])].map((answer) => (
          <div className="bg-base-300 rounded-md p-2 flex gap-3">
            <div className="avatar  rounded-full bg-base-400 w-8 h-8">
              <p className="placeholder font-semibold">
                {answer.answeredBy.firstName[0]}
                {answer.answeredBy.lastName[0]}
              </p>
            </div>
            <div>
              <div className="flex items-center justify-center gap-2">
                <p>
                  {answer.answeredBy.firstName} {answer.answeredBy.lastName}
                </p>
              </div>
              <p>{answer.answerText}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

const QuestionBox: FC<{ question: Question }> = ({ question }) => {
  const [showAnswers, setShowAnswers] = useState(false);

  return (
    <div className="p-4 bg-base-200">
      <div className="flex justify-between">
        <div className="flex  gap-4">
          <div className="flex items-center justify-center gap-2 rounded-full bg-base-300 w-8 h-8">
            <p className="font-semibold ">
              {question.postedBy.firstName[0]}
              {question.postedBy.lastName[0]}
            </p>
          </div>
          <div>
            <p className="font-semibold">
              {question.postedBy.firstName} {question.postedBy.lastName}
            </p>
            <div>{question.questionText}</div>
          </div>
        </div>
        <div
          className="p-2 justify-self-end"
          onClick={() => setShowAnswers(true)}
        >
          <GoCommentDiscussion />
        </div>
      </div>
      <div className="p-2 ml-12 mt-4 flex flex-col gap-2">
        {showAnswers && <Answers questionId={question.id} />}
      </div>
    </div>
  );
};

export default QuestionBox;
