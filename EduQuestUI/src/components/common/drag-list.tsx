import { closestCenter, DndContext } from "@dnd-kit/core";
import {
  SortableContext,
  verticalListSortingStrategy,
} from "@dnd-kit/sortable";

const DragList = ({ items }: { items: any[] }) => {
  return (
    <DndContext collisionDetection={closestCenter}>
      <SortableContext items={items} strategy={verticalListSortingStrategy}>
        DragList
      </SortableContext>
    </DndContext>
  );
};

export default DragList;
