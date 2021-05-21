using System;

namespace BlazorTicTacToe.Models
{
    public class Cell
    {
        public Board Parent { get; private set; }
        public MarkTypes MarkType { get; private set; }

        public EventHandler<CellMarkedEventArgs> OnAfterCellMarked;

        /// <summary>
        /// The method to check if the cell is already marked or not.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return MarkType == MarkTypes.NotMarked;
            }
        }

        public Cell(Board parent)
        {
            this.Parent = parent;
        }

        public void MarkCell(MarkTypes selectedMarkType)
        {
            if (MarkType == MarkTypes.NotMarked)
            {
                MarkType = selectedMarkType;
                if (OnAfterCellMarked != null)
                {
                    OnAfterCellMarked.Invoke(this, new CellMarkedEventArgs { SelectedMarkType = selectedMarkType });
                }                
            }
        }

    }
}
