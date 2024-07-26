import React, { FC } from "react";

export interface Section {
  name: string;
  description: string;
}

type SectionProps = Section;

const SectionDrop: FC<SectionProps> = ({ name: title, description }) => {
  return (
    <div className="collapse collapse-plus bg-base-200">
      <input type="radio" name="my-accordion-3" defaultChecked />
      <div className="collapse-title text-xl font-medium">{title}</div>
      <div className="collapse-content">
        <p>{description}</p>
      </div>
    </div>
  );
};

export default SectionDrop;
