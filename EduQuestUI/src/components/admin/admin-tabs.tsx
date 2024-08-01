import { FC, useState } from "react";
import Tabs from "../common/Tabs";
import { Tab } from "../../interfaces/common";
import AdminReviewTab from "./admin-review-tab";
import AdminLiveTab from "./admin-live-tab";
import AdminOutdatedTab from "./admin-outdated-tab";

interface AdminTab extends Tab {
  value: "review" | "live" | "outdated";
}

const tabs: AdminTab[] = [
  {
    label: "Course for Review",
    value: "review",
  },
  {
    label: "Live Courses",
    value: "live",
  },
  {
    label: "Outdated Courses",
    value: "outdated",
  },
];

const AdminTabs: FC = ({}) => {
  const [activeTab, setActiveTab] = useState<AdminTab["value"]>("review");

  return (
    <div>
      <Tabs
        tabs={tabs}
        activeTab={activeTab}
        onClick={(tab) => {
          setActiveTab(tab.value as AdminTab["value"]);
        }}
        style="bordered"
      />
      {activeTab === "review" && <AdminReviewTab />}

      {activeTab === "live" && <AdminLiveTab />}

      {activeTab === "outdated" && <AdminOutdatedTab />}
    </div>
  );
};

export default AdminTabs;
