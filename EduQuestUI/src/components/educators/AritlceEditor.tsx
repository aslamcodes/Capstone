import MDEditor, { title } from "@uiw/react-md-editor";
import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Loader from "../common/Loader";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { Article } from "../../interfaces/course";
import { customToast } from "../../utils/toast";
import { getErrorMessage } from "../../utils/error";
import { FaLaravel } from "react-icons/fa";

const AritlceEditor = () => {
  const { user } = useAuthContext();

  const { contentId } = useParams();

  const [isSaving, setIsSaving] = useState(false);
  const [article, setArticle] = useState<Article | null>(null);
  const navigate = useNavigate();

  if (!user) {
    navigate("/login");
    return;
  }

  useEffect(() => {
    const fetchContent = async () => {
      try {
        const { data } = await axios.get<Article>(`/api/Article/ForContent`, {
          params: { contentId },
          headers: {
            Authorization: `Bearer ${user?.token}`,
          },
        });

        setArticle(data);
      } catch {
        customToast("Failed to fetch article", { type: "error" });
      }
    };

    fetchContent();
  }, []);

  const handleSave = async () => {
    try {
      setIsSaving(true);

      await axios.put(
        "/api/Article",
        {
          id: article?.id,
          contentId: article?.contentId,
          body: article?.body,
          title: article?.title,
          description: article?.description,
        },
        {
          headers: {
            Authorization: `Bearer ${user?.token}`,
          },
        }
      );
      customToast("Saved", {
        type: "success",
      });
    } catch (error) {
      customToast(getErrorMessage(error), {
        type: "error",
      });
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="flex flex-col gap-3 h-screen">
      <div className="md:hidden">
        <p className="alert alert-warning">
          Use a larger screen to edit the article. The editor is not optimised
          for mobile.
        </p>
      </div>
      <MDEditor
        className="flex-grow max-h-[70vh]"
        value={article?.body}
        onChange={(v) => {
          setArticle((prev) => ({ ...prev, body: v } as Article));
        }}
        data-color-mode="light"
      >
        <MDEditor.Markdown
          source={article?.body}
          style={{ whiteSpace: "pre-wrap" }}
        />
      </MDEditor>
      <div className="text-right">
        <button className="btn btn-primary" onClick={handleSave}>
          {isSaving ? <Loader type="ring" size="md"></Loader> : <>Save</>}
        </button>
      </div>
    </div>
  );
};

export default AritlceEditor;
