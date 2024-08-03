import React, { useState, useEffect, useCallback } from "react";
import axios from "axios";
import debounce from "lodash/debounce";
import { Link } from "react-router-dom";
import { Course } from "../../interfaces/course";
import Loader from "./Loader";
import { IoClose, IoCloseCircle, IoCloseCircleOutline } from "react-icons/io5";

const SearchBar = ({ onClose = () => {} }: { onClose?: () => void }) => {
  const [query, setQuery] = useState("");
  const [results, setResults] = useState<Course[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showResults, setShowResult] = useState(false);

  const fetchResults = useCallback(
    debounce(async (searchQuery) => {
      if (!searchQuery) {
        setResults([]);
        return;
      }

      setIsLoading(true);
      setError(null);

      try {
        const response = await axios.get("/api/Course/search", {
          params: { query: searchQuery },
        });
        setResults(response.data);
      } catch (err) {
        setError("An error occurred while fetching results.");
        console.error("Search error:", err);
      } finally {
        setIsLoading(false);
      }
    }, 300),
    []
  );

  useEffect(() => {
    fetchResults(query);
    // Cleanup function to cancel any pending debounced calls when the component unmounts
    return () => {
      fetchResults.cancel();
    };
  }, [query, fetchResults]);

  const handleInputChange = (e: any) => {
    setQuery(e.target.value);
  };

  return (
    <div
      className="w-full max-w-xl mx-auto relative"
      onFocus={() => setShowResult(true)}
    >
      <div className="relative">
        <input
          type="text"
          placeholder="Search courses"
          value={query}
          onChange={handleInputChange}
          className="input input-bordered w-full pr-10"
        />
        <IoCloseCircle
          className="md:hidden absolute right-2 top-1/2 transform -translate-y-1/2 cursor-pointer"
          size={22}
          onClick={onClose}
        />
        {isLoading && <Loader />}
      </div>

      {showResults && (
        <div
          onBlur={() => setShowResult(false)}
          className="absolute max-h-[60vh] min-w-full mt-4 overflow-y-scroll no-scrollbar bg-base-100 rounded-box"
          onClick={(e) => e.preventDefault()}
        >
          {error && (
            <div className="text-error mt-2 p-4">
              <p>{error}</p>
            </div>
          )}

          {results.length > 0 && (
            <ul className="mt-4 bg-base-100 shadow-xl  ">
              {results.map((course) => (
                <li
                  className="p-4 hover:bg-base-200 cursor-pointer max-w-80 text-ellipsis overflow-hidden"
                  key={course.id}
                >
                  <Link
                    to={`/course-description/${course.id}`}
                    className="w-full"
                  >
                    <h3 className="font-bold">{course.name}</h3>
                    <p className="text-sm text-base-content text-opacity-70">
                      {course.description.length > 50
                        ? course.description.slice(0, 20) + "..."
                        : course.description}
                    </p>
                  </Link>
                </li>
              ))}
            </ul>
          )}

          {query && results.length === 0 && !isLoading && (
            <p className="mt-4 text-center p-4">No results found.</p>
          )}
        </div>
      )}
    </div>
  );
};

export default SearchBar;
