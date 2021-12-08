using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Logic4InARow;

namespace UserInterface4InARow
{
    public class FormGamePlay : Form
    {
        private readonly Button[,] r_ButtonsBoard;
        private readonly int r_BoardHeight;
        private readonly int r_BoardWidth;
        private readonly Label r_LabelP1Name = new Label();
        private readonly Label r_LabelP2Name = new Label();
        private readonly Player r_Player1;
        private readonly Player r_Player2;
        private readonly Game r_Game;
        private readonly List<Button> r_InputKeys;
        private readonly Label r_LabelP1Score = new Label();
        private readonly Label r_LabelP2Score = new Label();
        private Player m_CurrentPlayer;
        
        public FormGamePlay(int i_BoardWidth, int i_BoardHeight, eGameMode i_GameMode, string i_P1Name, string i_P2Name)
        {
            r_InputKeys = new List<Button>();
            r_BoardHeight = i_BoardHeight;
            r_BoardWidth = i_BoardWidth;
            r_LabelP1Name.Text = i_P1Name;
            r_LabelP2Name.Text = i_P2Name;
            r_ButtonsBoard = new Button[i_BoardHeight, i_BoardWidth];
            r_InputKeys = new List<Button>();
            r_Game = new Game(r_BoardWidth, r_BoardHeight, i_GameMode);
            r_Player1 = new Player(eCoinFigure.X, i_P1Name);
            r_Player2 = new Player(eCoinFigure.O, i_P2Name);
            m_CurrentPlayer = r_Player1;
            initButtons();
            initGamePlayForm();
        }

        private void initializeComponent() // Blank lines between controls for better readability
        {
            int top = r_ButtonsBoard[r_BoardHeight - 1, 0].Bottom + 20;
            int left = 40 * r_BoardWidth / 2 - 40;

            r_LabelP1Name.Text = r_Player1.PlayersName + ":";
            r_LabelP1Name.Font = new Font("David", 10, FontStyle.Bold);
            r_LabelP1Name.Top = top;
            r_LabelP1Name.Left = left;
            r_LabelP1Name.AutoSize = true;
            this.Controls.Add(r_LabelP1Name);

            r_LabelP1Score.Text = r_Player1.ScoreCounter.ToString();
            r_LabelP1Score.Top = top;
            r_LabelP1Score.Left = r_LabelP1Name.Right + 1;
            r_LabelP1Score.AutoSize = true;
            this.Controls.Add(r_LabelP1Score);

            r_LabelP2Name.Text = r_Player2.PlayersName + ":";
            r_LabelP2Name.Font = new Font("David", 10, FontStyle.Regular);
            r_LabelP2Name.Top = top;
            r_LabelP2Name.Left = r_LabelP1Score.Right + 10;
            r_LabelP2Name.AutoSize = true;
            this.Controls.Add(r_LabelP2Name);

            r_LabelP2Score.Text = r_Player2.ScoreCounter.ToString();
            r_LabelP2Score.Top = top;
            r_LabelP2Score.Left = r_LabelP2Name.Right + 3;
            r_LabelP2Score.AutoSize = true;
            this.Controls.Add(r_LabelP2Score);
        }

        private void initButtons()
        {
            Button currentButton;
            int distanceFromTop = this.Top + 10;
            int buttonOfInputKeys = 0;

            for (int i = 0; i < r_BoardWidth; i++)
            {
                currentButton = new Button();
                currentButton.Enabled = true;
                currentButton.Size = new Size(30, 20);
                currentButton.Text = (i + 1).ToString();
                currentButton.Location = new Point(40 * (i + 1), distanceFromTop);
                currentButton.Click += new EventHandler(inputButton_Click);
                r_InputKeys.Add(currentButton);
                this.Controls.Add(currentButton);
                buttonOfInputKeys = currentButton.Bottom;
            }

            for (int i = 0; i < r_BoardHeight; i++)
            {
                for (int j = 0; j < r_BoardWidth; j++)
                {
                    currentButton = new Button();
                    currentButton.Enabled = true;
                    currentButton.Size = new Size(30, 30);
                    currentButton.Font = new Font(Label.DefaultFont, FontStyle.Bold);
                    r_ButtonsBoard[i, j] = currentButton;
                    currentButton.Location = new Point(40 * j + buttonOfInputKeys + distanceFromTop, 40 * (i + 1));
                    this.Controls.Add(currentButton);
                }
            }

            initializeComponent();
            resetButtons();
            r_Game.gameBoard.CellChanged += syncBoards;
            r_Game.gameBoard.ColumnFull += disableWhenColumnFull;
        }

        private void inputButton_Click(object i_Sender, EventArgs i_E)
        {
            int userInput = int.Parse((i_Sender as Button).Text) - 1;

            handleTurn(userInput);
        }

        private void handleTurn(int i_UserInput)
        {
            handleMove(i_UserInput);
            if (!handleIfRoundIsFinished() && r_Game.gameMode == eGameMode.VsComputer)
            {
                handleMove(r_Game.GetComputerMove(r_Player1, r_Player2));
                handleIfRoundIsFinished();
            }
        }

        private bool handleIfRoundIsFinished()
        {
            bool isGameOver;

            if (r_Game.gameBoard.ThereIsWinning(m_CurrentPlayer, Game.k_WinningSequence))
            {
                handleEndOfRound(eRoundOverStatus.Winning);
                isGameOver = true;
            }
            else if(r_Game.gameBoard.CheckIfBoardIfFull())
            {
                handleEndOfRound(eRoundOverStatus.Tie);
                isGameOver = true;
            }
            else
            {
                shiftBetweenPlayers();
                isGameOver = false;
            }

            return isGameOver;
        }

        private void handleMove(int i_UserInput)
        {
            int rowOfPlay = r_Game.gameBoard.GetRowOfPlay(i_UserInput);

            r_Game.MakeMove(m_CurrentPlayer, i_UserInput, !Game.v_TestMode);
        }

        private void disableWhenColumnFull(int i_ColumnOfPlay)
        {
            r_InputKeys[i_ColumnOfPlay].Enabled = false;
        }

        private void scoreUpdate()
        {
            if (m_CurrentPlayer.Equals(r_Player1)) 
            {
                r_Player1.ScoreCounter++;
                r_LabelP1Score.Text = r_Player1.ScoreCounter.ToString();
            }
            else
            {
                r_Player2.ScoreCounter++;
                r_LabelP2Score.Text = r_Player2.ScoreCounter.ToString();
            }
        }

        private void handleEndOfRound(eRoundOverStatus i_Status)
        {
            DialogResult result;

            if (i_Status == eRoundOverStatus.Winning)
            {
                result = MessageBox.Show(m_CurrentPlayer.PlayersName + " won!" + Environment.NewLine + "Another Round?",
                                "A win!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                scoreUpdate();
            }
            else
            {
                result = MessageBox.Show("Tie!" + Environment.NewLine + "Another Round?",
                                "It's a Tie!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (result == DialogResult.Yes) // another round
            {
                resetRound();
            }
            else
            {
                this.Close();
            }
        }

        private void resetRound()
        {
            r_Game.gameBoard.InitGameBoard();
            resetButtons();
            m_CurrentPlayer = r_Player1;
            r_LabelP2Name.Font = new Font("David", 10, FontStyle.Regular);
            r_LabelP1Name.Font = new Font("David", 10, FontStyle.Bold);
        }

        private void resetButtons()
        {
            foreach(Button button in r_ButtonsBoard)
            {
                button.Text = Cell.k_Blank.ToString();
            }

            foreach (Button key in r_InputKeys)
            {
                key.Enabled = true;
            }
        }

        private void syncBoards(int i_X, int i_Y)
        {
            r_ButtonsBoard[i_Y, i_X].Text = r_Game.gameBoard.CellsArray[i_Y, i_X].SingleCell.ToString();
        }

        private void shiftBetweenPlayers()
        {
            if (m_CurrentPlayer.Equals(r_Player1))
            {
                r_LabelP1Name.Font = new Font("David", 10, FontStyle.Regular);
                r_LabelP2Name.Font = new Font("David", 10, FontStyle.Bold);
                m_CurrentPlayer = r_Player2;
            }
            else
            {
                r_LabelP2Name.Font = new Font("David", 10, FontStyle.Regular);
                r_LabelP1Name.Font = new Font("David", 10, FontStyle.Bold);
                m_CurrentPlayer = r_Player1;
            }
        }

        private void initGamePlayForm()
        {
            this.Text = "4 in a Row!";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoSize = true;
            this.Width = r_InputKeys[r_BoardWidth - 1].Right + 60;
        }
    }
}