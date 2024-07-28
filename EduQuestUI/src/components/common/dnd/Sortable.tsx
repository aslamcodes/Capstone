import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import React, { FC, PropsWithChildren } from "react";

const Sortable: FC<PropsWithChildren<{ id: number }>> = ({ id, children }) => {
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    transition,
    isDragging,
  } = useSortable({ id });

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
  };

  return (
    <div
      ref={setNodeRef}
      style={style}
      className={`z-40 cursor-grab ${isDragging && "cursor-grabbing z-50"}`}
      {...attributes}
      {...listeners}
    >
      {children}
    </div>
  );
};

export default Sortable;
