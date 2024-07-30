import React, { FC } from "react";
import { Tab } from "../../interfaces/common";

type TabStyle = "boxed" | "lifted" | "bordered";

const Tabs: FC<{
  tabs: Tab[];
  activeTab: Tab["value"];
  onClick: (tab: Tab) => void;
  style?: TabStyle;
}> = ({ tabs, activeTab, onClick, style = "lifted" }) => {
  return (
    <div
      role="tablist"
      className={`tabs tabs-${style} tabs-lg sticky top-16 bg-base-100 z-50`}
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
