import useSWR from "swr";
import { fetcherWithToken } from "../../utils/fetcher";
import { useAuthContext } from "../../contexts/auth/authReducer";
import { Link } from "react-router-dom";
import axios from "axios";
import { useEffect, useState } from "react";

export interface Course {
  id: number;
  name: string;
  description: string;
  educatorId: number;
  level: "Begginer" | "Intermediate" | "Advanced";
  price: number;
}

const MyCourses = () => {
  const { user } = useAuthContext();
  const [courses, setCourses] = useState<Course[]>([]);
  if (!user?.token) {
    return <div>You need to be logged in to view this page.</div>;
  }

  // const { data: courses, isLoading } = useSWR<Course[], any>(
  //   ["/api/Course/Student-Courses", user?.token],
  //   ([url, token]) => fetcherWithToken(url, token as string)
  // );

  useEffect(() => {
    let fetch = async () => {
      const { data } = await axios.get("/api/Course/Student-Courses", {
        headers: {
          Authorization: `Bearer ${user?.token}`,
        },
      });

      setCourses(data);
    };
    fetch();
  }, []);

  // if (isLoading && !courses) {
  //   return <div>Loading...</div>;
  // }

  return (
    <div className="container mx-auto p-4">
      <div className="flex flex-wrap gap-4">
        {courses?.map((course) => (
          <div className="card bg-base-100 w-72 shadow-xl">
            <figure>
              <img
                src="https://img.daisyui.com/images/stock/photo-1606107557195-0e29a4b5b4aa.webp"
                alt="Shoes"
              />
            </figure>
            <div className="card-body">
              <h2 className="card-title">{course.name}</h2>
              <p>{course.description}</p>
              <div className="card-actions justify-end">
                <Link to={`/myCourses/${course.id}`}>
                  <button className="btn btn-primary">Go to course</button>
                </Link>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default MyCourses;
