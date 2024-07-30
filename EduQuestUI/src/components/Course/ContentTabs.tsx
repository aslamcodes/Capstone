import React, { FC, useState } from "react";
import Tabs from "../common/Tabs";
import { Tab } from "../../interfaces/common";
import CourseDescription from "./ContentDescription";
import Notes from "./Notes";
import Review from "./review";

interface ContentDescriptionProps {
  contentId: number | null;
  courseId: number | null;
}

interface ContetDescriptionTab extends Tab {
  value: "description" | "qa" | "notes" | "review";
}

const tabs: ContetDescriptionTab[] = [
  {
    label: "Description",
    value: "description",
  },
  {
    label: "Q & A",
    value: "qa",
  },
  {
    label: "Notes",
    value: "notes",
  },
  {
    label: "Review",
    value: "review",
  },
];

const ContentTabs: FC<ContentDescriptionProps> = ({ contentId, courseId }) => {
  const [activeTab, setActiveTab] =
    useState<ContetDescriptionTab["value"]>("description");

  return (
    <div>
      <Tabs
        tabs={tabs}
        activeTab={activeTab}
        onClick={(tab) => {
          setActiveTab(tab.value as ContetDescriptionTab["value"]);
        }}
        style="bordered"
      />
      {activeTab === "description" && (
        <CourseDescription courseId={courseId as number} />
      )}

      {activeTab === "qa" && <div>Q & A</div>}

      {activeTab === "notes" && <Notes contentId={contentId as number} />}

      {activeTab === "review" && <Review courseId={courseId as number} />}
    </div>
  );
};

export default ContentTabs;
