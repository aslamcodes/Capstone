import React, { useState, useEffect, useCallback } from "react";
import axios from "axios";
import debounce from "lodash/debounce";
import { Link } from "react-router-dom";
import { Course } from "../../interfaces/course";

const SearchBar = () => {
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
      onBlur={() => setShowResult(false)}
      onFocus={() => setShowResult(true)}
    >
      <div className="">
        <input
          type="text"
          placeholder="Search courses"
          value={query}
          onChange={handleInputChange}
          className="input input-bordered w-full pr-10"
        />
        {isLoading && (
          <span className="absolute right-3 top-3">
            <svg
              className="animate-spin h-5 w-5 text-gray-400"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
            >
              <circle
                className="opacity-25"
                cx="12"
                cy="12"
                r="10"
                stroke="currentColor"
                strokeWidth="4"
              ></circle>
              <path
                className="opacity-75"
                fill="currentColor"
                d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
              ></path>
            </svg>
          </span>
        )}
      </div>

      {showResults && (
        <div
          className="absolute max-h-[60vh] min-w-full mt-4 overflow-y-scroll no-scrollbar bg-base-100 rounded-box"
          onClick={(e) => e.stopPropagation()}
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
                  className="p-4 hover:bg-base-200 cursor-pointer"
                  key={course.id}
                >
                  <Link to={`/course-description/${course.id}`}>
                    <h3 className="font-bold">{course.name}</h3>
                    <p className="text-sm text-base-content text-opacity-70">
                      {course.description}
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
