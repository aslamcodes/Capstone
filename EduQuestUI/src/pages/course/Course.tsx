import React from "react";
import { useLocation, useParams } from "react-router-dom";
import SectionDrop, { Section } from "../../components/common/SectionDrop";
import useSWR from "swr";
import { fetcher, fetcherWithToken } from "../../utils/fetcher";
import { useAuthContext } from "../../contexts/auth/authReducer";

const Course = () => {
  const { courseId } = useParams();
  const { user } = useAuthContext();

  // const { data } = useSWR(
  //   ["/api/course/section", user?.token as string],
  //   ([url, token]) => fetcherWithToken(url, token)
  // );
  const { data: sections, isLoading } = useSWR<Section[]>(
    `/api/course/sections?courseId=${courseId}`,
    fetcher
  );

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (!sections) {
    return <div>No sections found</div>;
  }

  return (
    <div>
      {sections.map((section) => (
        <SectionDrop name={section.name} description={section.description} />
      ))}
    </div>
  );
};

export default Course;
