import React from "react";
import Sortable from "./dnd/Sortable";
import { closestCenter, DndContext } from "@dnd-kit/core";
import {
  SortableContext,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";

const DragList = ({ items }) => {
  return (
    <DndContext collisionDetection={closestCenter}>
      <SortableContext items={items} strategy={verticalListSortingStrategy}>
        DragList
      </SortableContext>
    </DndContext>
  );
};

export default DragList;
