import React, { FC, useState } from "react";
import Tabs from "../common/Tabs";
import { Tab } from "../../interfaces/common";
import CourseDescription from "./ContentDescription";
import Notes from "./Notes";

interface ContentDescriptionProps {
  contentId: number | null;
  courseId: number | null;
}

interface ContetDescriptionTab extends Tab {
  value: "description" | "qa" | "notes";
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
    </div>
  );
};

export default ContentTabs;
