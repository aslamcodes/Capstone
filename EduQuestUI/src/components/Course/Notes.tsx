import React, { FC, useCallback, useEffect, useState } from "react";
import useNotes from "../../hooks/fetchers/useNotes";
import { debounce } from "lodash";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { BiError } from "react-icons/bi";
import Loader from "../common/Loader";

const Notes: FC<{ contentId: number }> = ({ contentId }) => {
  const { user } = useAuthContext();
  const { notes, isLoading, error } = useNotes(contentId);
  const [localNote, setLocalNote] = useState<string>();
  const [isSaving, setIsSaving] = useState(false);

  const debouncedSaveNote = useCallback(
    debounce(async (updatedNote: string) => {
      setIsSaving(true);
      await axios.put(
        `/api/Notes/`,
        {
          ...notes,
          noteContent: updatedNote,
        },
        {
          headers: { Authorization: `Bearer ${user?.token}` },
        }
      );
      setIsSaving(false);
    }, 1000),
    [notes, user?.token]
  );

  useEffect(() => {
    setLocalNote(notes?.noteContent);
  }, [notes?.noteContent]);

  const handleNoteChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    const newValue = e.target.value;
    setLocalNote(newValue);
    debouncedSaveNote(newValue);
  };

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!contentId)
    return (
      <div className="p-4 flex gap-2 items-center">
        <BiError />
        <p>Please select a content from sections</p>
      </div>
    );

  if (error) {
    return <div>{error.response.data?.message}</div>;
  }

  if (!notes) {
    return <div>No Notes</div>;
  }

  return (
    <div className="gap-4 p-4 flex flex-col">
      <div className="text-xl font-bold">Notes</div>
      <div className="relative">
        <textarea
          className="textarea textarea-bordered w-full"
          value={localNote}
          onChange={handleNoteChange}
        ></textarea>
        {isSaving && (
          <div className="absolute top-0 right-0 p-2 flex items-center gap-2 text-base-500">
            <Loader /> Saving...
          </div>
        )}
      </div>
    </div>
  );
};

export default Notes;
