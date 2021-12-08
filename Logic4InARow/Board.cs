using System;

namespace Logic4InARow
{
    public class Board
    {
        private readonly int r_WidthOfGameBoard;
        private readonly int r_HeightOfGameBoard;
        private readonly Cell[,] r_CellsArray;

        public event Action<int, int> CellChanged;

        public event Action<int> ColumnFull;
        
        public Board(int i_HeightOfGameBoard, int i_WidthOfGameBoard)
        {
            r_WidthOfGameBoard = i_WidthOfGameBoard;
            r_HeightOfGameBoard = i_HeightOfGameBoard;
            r_CellsArray = new Cell[i_HeightOfGameBoard, i_WidthOfGameBoard];
            InitGameBoard();
        }

        public int WidthOfGameBoard
        {
            get { return r_WidthOfGameBoard; }
        }

        public int HeightOfGameBoard
        {
            get { return r_HeightOfGameBoard; }
        }

        public Cell[,] CellsArray 
        {
            get { return r_CellsArray; }
        }

        public void InitGameBoard()
        {
            for(int i = 0; i < r_HeightOfGameBoard; i++)
            {
                for(int j = 0; j < r_WidthOfGameBoard; j++)
                {
                    r_CellsArray[i, j] = new Cell();
                }
            }
        }

        public int GetRowOfPlay(int i_UserInput) // getting the row of the last move, to get the exact coordinate of the move
        {
            int currentRowPlayed = r_HeightOfGameBoard - 1;

            while(!r_CellsArray[currentRowPlayed, i_UserInput].CellIsEmpty())
            {
                currentRowPlayed--;
            }

            return currentRowPlayed;
        }

        public void IsColumnFilledUp(int i_RowOfPlay, int i_ColumnOfPlay)
        {
            if(i_RowOfPlay == 0) // columnFilledUp
            {
                OnColumnFull(i_ColumnOfPlay);
            }
        }

        protected virtual void OnColumnFull(int i_ColumnOfPlay)
        {
            if(ColumnFull != null)
            {
                ColumnFull.Invoke(i_ColumnOfPlay);
            }
        }

        public void UpdateBoardWithMove(int i_Row, int i_Column, char i_PlayerSign, bool i_TestingAiMode)
        {
            r_CellsArray[i_Row, i_Column].SingleCell = i_PlayerSign;
            if(!i_TestingAiMode)
            {
                OnCellChanged(i_Row, i_Column);
                IsColumnFilledUp(i_Row, i_Column);
            }
        }

        protected virtual void OnCellChanged(int i_Row, int i_Column)
        {
            if (CellChanged != null)
            {
                CellChanged.Invoke(i_Column, i_Row);
            }
        }

        public bool ThereIsWinning(Player i_CurrentPlayer, int i_SizeOfNeededSequence)
        {
            return verticalWinning(i_CurrentPlayer, i_SizeOfNeededSequence) || horizontalWinning(i_CurrentPlayer, i_SizeOfNeededSequence)
                                                                            || descendingDiagonalWinning(i_CurrentPlayer, i_SizeOfNeededSequence)
                                                                            || ascendingDiagonalWinning(i_CurrentPlayer, i_SizeOfNeededSequence);
        }

        private bool verticalWinning(Player i_CurrentPlayer, int i_SizeOfNeededSequence)
        {
            bool isWinning = false;
            int counter = 0;
            int playedColumn = i_CurrentPlayer.LastMove.Y;
            int playerRow = i_CurrentPlayer.LastMove.X;
            int highestCellInColumnInRangeOfFourCells = playerRow - 3;
            int lowestCellInColumnInRangeOfFourCells = playerRow + 3;

            if (highestCellInColumnInRangeOfFourCells < 0)
            {
                highestCellInColumnInRangeOfFourCells = 0;
            }

            if(r_HeightOfGameBoard - 1 < lowestCellInColumnInRangeOfFourCells)
            {
                lowestCellInColumnInRangeOfFourCells = r_HeightOfGameBoard - 1;
            }

            for(int i = highestCellInColumnInRangeOfFourCells; i <= lowestCellInColumnInRangeOfFourCells; i++)
            {
                if((eCoinFigure)r_CellsArray[i, playedColumn].SingleCell == i_CurrentPlayer.PlayerSign)
                {
                    counter++;
                    if(counter == i_SizeOfNeededSequence)
                    {
                        isWinning = true;
                        break;
                    }
                }
                else
                {
                    counter = 0;
                }
            }

            return isWinning;
        }

        private bool horizontalWinning(Player i_CurrentPlayer, int i_SizeOfNeededSequence)
        {
            bool isWinning = false;
            int playedColumn = i_CurrentPlayer.LastMove.Y;
            int playerRow = i_CurrentPlayer.LastMove.X;
            int counter = 0;
            int mostRightCellInColumnInRangeOfFourCells = playedColumn + 3;
            int mostLeftCellInColumnInRangeOfFourCells = playedColumn - 3;

            if (r_WidthOfGameBoard - 1 < mostRightCellInColumnInRangeOfFourCells)
            {
                mostRightCellInColumnInRangeOfFourCells = r_WidthOfGameBoard - 1;
            }

            if (0 > mostLeftCellInColumnInRangeOfFourCells)
            {
                mostLeftCellInColumnInRangeOfFourCells = 0;
            }

            for (int i = mostLeftCellInColumnInRangeOfFourCells; i <= mostRightCellInColumnInRangeOfFourCells; i++)
            {
                if ((eCoinFigure)r_CellsArray[playerRow, i].SingleCell == i_CurrentPlayer.PlayerSign)
                {
                    counter++;
                    if (counter == i_SizeOfNeededSequence)
                    {
                        isWinning = true;
                        break;
                    }
                }
                else
                {
                    counter = 0;
                }
            }

            return isWinning;
        }

        private bool descendingDiagonalWinning(Player i_CurrentPlayer, int i_SizeOfNeededSequence)
        {
            bool isWinning = false;
            int counter = 0;
            int playedColumn = i_CurrentPlayer.LastMove.Y;
            int playerRow = i_CurrentPlayer.LastMove.X;
            int distanceFromMostLeftCellOfDiagonal = Math.Min(playedColumn, playerRow);
            int rowToStartCheck = 0;
            int columnToStartCheck = 0;
            int distanceFromBottomLine = r_HeightOfGameBoard - (playerRow + 1);
            int distanceFromMostRightCell = r_WidthOfGameBoard - (playedColumn + 1);
            int distanceFromRightCellOfDiagonal = 0;
            int rowToEndCheck = 0;
            int columnToEndCheck = 0;
            int i = 0;

            if (3 < distanceFromMostLeftCellOfDiagonal)
            {
                distanceFromMostLeftCellOfDiagonal = 3;
            }

            rowToStartCheck = playerRow - distanceFromMostLeftCellOfDiagonal;
            columnToStartCheck = playedColumn - distanceFromMostLeftCellOfDiagonal;
            distanceFromRightCellOfDiagonal = Math.Min(distanceFromBottomLine, distanceFromMostRightCell);
            if(3 < distanceFromRightCellOfDiagonal)
            {
                distanceFromRightCellOfDiagonal = 3;
            }

            rowToEndCheck = playerRow + distanceFromRightCellOfDiagonal;
            columnToEndCheck = playedColumn + distanceFromRightCellOfDiagonal;
            while(rowToStartCheck + i != rowToEndCheck + 1)
            {
                if((eCoinFigure)r_CellsArray[rowToStartCheck + i, columnToStartCheck + i].SingleCell == i_CurrentPlayer.PlayerSign)
                {
                    counter++;
                    if(counter == i_SizeOfNeededSequence)
                    {
                        isWinning = true;
                        break;
                    }
                }
                else
                {
                    counter = 0;
                }

                i++;
            }

            return isWinning;
        }

        private bool ascendingDiagonalWinning(Player i_CurrentPlayer, int i_SizeOfNeededSequence)
        {
            bool isWinning = false;
            int playedColumn = i_CurrentPlayer.LastMove.Y;
            int playerRow = i_CurrentPlayer.LastMove.X;
            int counter = 0;
            int distanceFromMostRightCell = r_WidthOfGameBoard - (playedColumn + 1);
            int distanceFromBottomLine = r_HeightOfGameBoard - (playerRow + 1);
            int distanceFromMostLeftCell = playedColumn;
            int distanceFromMostLeftCellOfDiagonal = Math.Min(distanceFromMostLeftCell, distanceFromBottomLine);
            int rowToStartCheck = 0;
            int columnToStartCheck = 0;
            int distanceFromRightCellOfDiagonal = 0;
            int rowToEndCheck = 0;
            int columnToEndCheck = 0;
            int i = 0;

            if (3 < distanceFromMostLeftCellOfDiagonal)
            {
                distanceFromMostLeftCellOfDiagonal = 3;
            }

            rowToStartCheck = playerRow + distanceFromMostLeftCellOfDiagonal;
            columnToStartCheck = playedColumn - distanceFromMostLeftCellOfDiagonal;
            distanceFromRightCellOfDiagonal = Math.Min(playerRow, distanceFromMostRightCell);
            if (3 < distanceFromRightCellOfDiagonal)
            {
                distanceFromRightCellOfDiagonal = 3;
            }

            rowToEndCheck = playerRow - distanceFromRightCellOfDiagonal;
            columnToEndCheck = playedColumn + distanceFromRightCellOfDiagonal;
            while (rowToStartCheck - i >= rowToEndCheck)
            {
                if ((eCoinFigure)r_CellsArray[rowToStartCheck - i, columnToStartCheck + i].SingleCell == i_CurrentPlayer.PlayerSign)
                {
                    counter++;
                    if (counter == i_SizeOfNeededSequence)
                    {
                        isWinning = true;
                        break;
                    }
                }
                else
                {
                    counter = 0;
                }

                i++;
            }

            return isWinning;
        }

        public bool CheckIfBoardIfFull()
        {
            bool boardIfFull = true;

            for(int i = 0; i < r_HeightOfGameBoard; i++)
            {
                for(int j = 0; j < r_WidthOfGameBoard; j++)
                {
                    if(r_CellsArray[i, j].SingleCell == Cell.k_Blank)
                    {
                        boardIfFull = false;
                        break;
                    }
                }

                if(!boardIfFull)
                {
                    break;
                }
            }

            return boardIfFull;
        }
    }
}