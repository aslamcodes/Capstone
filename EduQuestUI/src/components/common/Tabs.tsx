import React, { FC } from "react";
import { Tab } from "../../interfaces/common";

const Tabs: FC<{
  tabs: Tab[];
  activeTab: Tab["value"];
  onClick: (tab: Tab) => void;
}> = ({ tabs, activeTab, onClick }) => {
  return (
    <div
      role="tablist"
      className="tabs tabs-bordered tabs-sm sticky top-16 bg-white"
    >
      {tabs.map((tab) => (
        <a
          onClick={() => {
            onClick(tab);
          }}
          role="tab"
          className={`tab hover:font-bold transition-all ${
            tab.value === activeTab && "tab-active"
          }`}
        >
          {tab.label}
        </a>
      ))}
    </div>
  );
};

export default Tabs;
