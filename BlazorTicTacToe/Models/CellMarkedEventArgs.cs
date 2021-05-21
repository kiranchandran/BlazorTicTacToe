using System;

namespace BlazorTicTacToe.Models
{
    public class CellMarkedEventArgs: EventArgs
    {
        public MarkTypes SelectedMarkType { get; set; }
    }
}
