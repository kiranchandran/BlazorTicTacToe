using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTicTacToe.Models
{
    public class Board
    {
        protected const int rowLength = 3;
        protected const int columnLength = 3;
        private MarkTypes currentPlayerMark = MarkTypes.Cross;

        public Cell[,] Cells { get; private set; } = new Cell[rowLength, columnLength];

        public EventHandler OnWinningGame;
        public EventHandler OnGameDraw;

        public Board()
        {
            ResetCellsToInitialState();
            InitializeEvents();
        }

        public MarkTypes GetCurrentPlayerMark()
        {
            return currentPlayerMark;
        }

        protected void SetNextPlayerMark()
        {
            if (currentPlayerMark == MarkTypes.Cross)
            {
                currentPlayerMark = MarkTypes.Oval;
            }
            else
            {
                currentPlayerMark = MarkTypes.Cross;
            }
        }

        protected void InitializeEvents()
        {
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < columnLength; j++)
                {
                    Cells[i, j].OnAfterCellMarked += OnAfterCellMarkedEventHandler;
                }
            }
        }

        protected void OnAfterCellMarkedEventHandler(object sender, CellMarkedEventArgs args)
        {
            this.SetNextPlayerMark();
            var gameResult = CheckGameStatus();
            if (gameResult.HasWinner)
            {
                if(OnWinningGame !=null)
                {
                    OnWinningGame.Invoke(this, null);
                }
            }
            if (gameResult.IsDraw)
            {
                if (OnGameDraw != null)
                {
                    OnGameDraw.Invoke(this, null);
                }
            }
        }

        protected void ResetCellsToInitialState()
        {
            Cells = new Cell[rowLength, columnLength];

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < columnLength; j++)
                {
                    Cells[i, j] = new Cell(this);
                }
            }
        }

        private IEnumerable<Cell> GetAllCells()
        {
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < columnLength; j++)
                {
                    yield return Cells[i, j];
                }
            }
        }

        protected GameStatus CheckGameStatus()
        {
            GameStatus result = new GameStatus();

            foreach (var tupleItem in GetPossibleSequences())
            {
                bool isSequenceAreSame = tupleItem.Item1.MarkType != MarkTypes.NotMarked && (tupleItem.Item1.MarkType == tupleItem.Item2.MarkType) && (tupleItem.Item1.MarkType == tupleItem.Item3.MarkType);

                if (isSequenceAreSame)
                {
                    result.IsGameCompleted = true;
                    result.HasWinner = true;
                    result.Winner = tupleItem.Item1.MarkType;

                    return result;
                }
            }

            result.IsGameCompleted = !HasAnyUnMarkedCells;
            result.IsDraw = !IsAnyChanceToWinGame();

            return result;
        }

        protected bool IsAnyChanceToWinGame()
        {
            foreach (Tuple<Cell, Cell, Cell> tupleItem in GetPossibleSequences())
            {
                bool isAllCellsEmpty = tupleItem.Item1.IsEmpty && tupleItem.Item2.IsEmpty && tupleItem.Item3.IsEmpty;
                if (isAllCellsEmpty == true)
                {
                    return true;
                }

                bool isTwoCellsEmpty = (tupleItem.Item1.IsEmpty && tupleItem.Item2.IsEmpty) || (tupleItem.Item1.IsEmpty && tupleItem.Item3.IsEmpty) || (tupleItem.Item2.IsEmpty && tupleItem.Item3.IsEmpty);
                if (isTwoCellsEmpty)
                {
                    return true;
                }

                bool isAnyCellEmpty = tupleItem.Item1.IsEmpty || tupleItem.Item2.IsEmpty || tupleItem.Item3.IsEmpty;

                bool isOneEmptyCellAndRestSameMarkTypes = isAnyCellEmpty && ((tupleItem.Item1.MarkType == tupleItem.Item2.MarkType) || (tupleItem.Item1.MarkType == tupleItem.Item3.MarkType) || (tupleItem.Item2.MarkType == tupleItem.Item3.MarkType));

                if (isOneEmptyCellAndRestSameMarkTypes)
                {
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// This will be the vertical, horizontal and diagonal cells. There will be a total of 8 sets in the board.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Tuple<Cell, Cell, Cell>> GetPossibleSequences()
        {
            for (int i = 0; i < rowLength; i++)
            {
                yield return new Tuple<Cell, Cell, Cell>(Cells[i, 0], Cells[i, 1], Cells[i, 2]);
                yield return new Tuple<Cell, Cell, Cell>(Cells[0, i], Cells[1, i], Cells[2, i]);
            }

            yield return new Tuple<Cell, Cell, Cell>(Cells[0, 0], Cells[1,1], Cells[2, 2]);
            yield return new Tuple<Cell, Cell, Cell>(Cells[0, 2], Cells[1, 1], Cells[2, 0]);
        }

        protected bool HasAnyUnMarkedCells
        {
            get
            {
                return GetAllCells().Any(s => s.IsEmpty);
            }
        }
    }
}
