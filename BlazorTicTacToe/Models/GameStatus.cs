namespace BlazorTicTacToe.Models
{
    public class GameStatus
    {
        public bool IsGameCompleted { get; set; } = false;

        public bool HasWinner { get; set; } = false;

        public MarkTypes Winner { get; set; } = MarkTypes.NotMarked;

        public bool IsDraw { get; set; } = false;
    }
}
