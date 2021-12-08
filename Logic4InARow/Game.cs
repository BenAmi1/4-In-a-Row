using System;
using System.Drawing;

namespace Logic4InARow
{
    public class Game
    {
        private readonly Board r_GameBoard;
        private readonly int r_BoardWidth;
        private readonly eGameMode r_GameMode;
        private readonly Random r_RandomMove = new Random();
        public const int k_WinningSequence = 4;
        private const int k_UnDefinedMove = -1;
        private const int k_SequenceOfThreeCharacters = 3;
        private const int k_SequenceOfTwoCharacters = 2;
        public const bool v_TestMode = true;

        public Game(int i_BoardWidth, int i_BoardHeight, eGameMode i_GameMode)
        {
            r_BoardWidth = i_BoardWidth;
            r_GameMode = i_GameMode;
            r_GameBoard = new Board(i_BoardHeight, r_BoardWidth);
        }

        public Board gameBoard
        {
            get { return r_GameBoard; }
        }

        public eGameMode gameMode
        {
            get { return r_GameMode; }
        }

        public void MakeMove(Player i_CurrentPlayer, int i_UserInput, bool i_TestingComputedMoveMode)
        {
            int rowOfPlay = r_GameBoard.GetRowOfPlay(i_UserInput);

            r_GameBoard.UpdateBoardWithMove(rowOfPlay, i_UserInput, (char)i_CurrentPlayer.PlayerSign, i_TestingComputedMoveMode);
            i_CurrentPlayer.LastMove = new Point(rowOfPlay, i_UserInput);
        }

        public int GetComputerMove(Player i_Player1, Player i_Player2)
        {
            int selectedMove = k_UnDefinedMove;
            int currentBestMove = k_UnDefinedMove;
            int currentMoveToMake = k_UnDefinedMove;
            bool ableToBlock = false;
            bool sequenceOfThree = false;
            bool winningPossible = false;

            for(int i = 0; i < r_BoardWidth; i++)
            {
                if(r_GameBoard.CellsArray[0, i].CellIsEmpty())
                {
                    MakeMove(i_Player2, i, v_TestMode);
                    if (r_GameBoard.ThereIsWinning(i_Player2, k_WinningSequence)) // check if computer can win
                    {
                        selectedMove = i;
                        unDoLastMove(i_Player2, i);
                        winningPossible = true;
                        break;
                    }

                    unDoLastMove(i_Player2, i);
                }
            }

            if(!winningPossible)
            {
                for(int i = 0; i < r_BoardWidth; i++)
                {
                    if(r_GameBoard.CellsArray[0, i].CellIsEmpty())
                    {
                        currentMoveToMake = canBlockOpponentWinning(i, i_Player1);
                        if(currentMoveToMake != k_UnDefinedMove)
                        {
                            ableToBlock = true;
                        }

                        MakeMove(i_Player2, i, v_TestMode);
                        if(ableToBlock)
                        {
                            selectedMove = currentMoveToMake;
                            unDoLastMove(i_Player2, i);
                            break;
                        }

                        if(r_GameBoard.CellsArray[0, i].CellIsEmpty())
                        {
                            MakeMove(i_Player1, i, v_TestMode);
                            if(!r_GameBoard.ThereIsWinning(i_Player1, k_WinningSequence)) //check if the move of computer will let opponent win
                            {
                                unDoLastMove(i_Player1, i);
                                if(r_GameBoard.ThereIsWinning(i_Player2, k_SequenceOfThreeCharacters))
                                {
                                    selectedMove = i;
                                    currentBestMove = i;
                                    sequenceOfThree = true;
                                }
                                else if(!sequenceOfThree && r_GameBoard.ThereIsWinning(i_Player2, k_SequenceOfTwoCharacters))
                                {
                                    currentBestMove = i;
                                }
                            }
                            else
                            {
                                unDoLastMove(i_Player1, i);
                            }
                        }
                        
                        unDoLastMove(i_Player2, i);
                    }
                }
            }

            if(selectedMove == k_UnDefinedMove)
            {
                selectedMove = currentBestMove;
            }

            while(k_UnDefinedMove == selectedMove || !r_GameBoard.CellsArray[0, selectedMove].CellIsEmpty())
            {
                selectedMove = r_RandomMove.Next(0, r_BoardWidth);
            }

            return selectedMove;
        }

        private void unDoLastMove(Player i_CurrentPlayer, int i_Column)
        {
            Point coordinateToDelete = i_CurrentPlayer.LastMove;

            r_GameBoard.CellsArray[coordinateToDelete.X, coordinateToDelete.Y].SingleCell = Cell.k_Blank;
        }

        private int canBlockOpponentWinning(int i_Column, Player i_Player1)
        {
            int ableToBlockInColumn = k_UnDefinedMove;

            MakeMove(i_Player1, i_Column, v_TestMode);
            if(r_GameBoard.ThereIsWinning(i_Player1, k_WinningSequence))
            {
                ableToBlockInColumn = i_Column;
            }

            unDoLastMove(i_Player1, i_Column);

            return ableToBlockInColumn;
        }
    }
}