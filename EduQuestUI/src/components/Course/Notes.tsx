import React, { FC, useCallback, useEffect, useState } from "react";
import useNotes from "../../hooks/fetchers/useNotes";
import { debounce } from "lodash";
import axios from "axios";
import { useAuthContext } from "../../contexts/auth/authReducer";

const Notes: FC<{ contentId: number }> = ({ contentId }) => {
  const { user } = useAuthContext();
  const { notes, isLoading, error } = useNotes(contentId);
  const [localNote, setLocalNote] = useState<string>();

  const debouncedSaveNote = useCallback(
    debounce(async (updatedNote: string) => {
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

  if (error) {
    return <div>{error.response.data?.message}</div>;
  }

  if (!notes) {
    return <div>No Notes</div>;
  }

  return (
    <div className="gap-4 p-4 flex flex-col">
      <div className="text-xl font-bold">Notes</div>
      <textarea
        className="textarea textarea-bordered w-full"
        value={localNote}
        onChange={handleNoteChange}
      ></textarea>
    </div>
  );
};

export default Notes;
