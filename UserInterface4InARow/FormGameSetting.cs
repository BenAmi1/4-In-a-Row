using System;
using System.Windows.Forms;
using System.Drawing;
using Logic4InARow;

namespace UserInterface4InARow
{
    public class FormGameSetting : Form
    {
        private readonly Label r_LabelPlayers = new Label();
        private readonly Label r_LabelPlayer1 = new Label();
        private readonly Label r_LabelBoardSize = new Label();
        private readonly Label r_LabelRows = new Label();
        private readonly Label r_LabelCols = new Label();
        private readonly CheckBox r_CheckBoxVsComputer = new CheckBox();
        private readonly TextBox r_TextBoxPlayer1Name = new TextBox();
        private readonly TextBox r_TextBoxPlayer2Name = new TextBox();
        private readonly NumericUpDown r_NumericUpDownSetRows = new NumericUpDown();
        private readonly NumericUpDown r_NumericUpDownSetCols = new NumericUpDown();
        private readonly Button r_ButtonStart = new Button();
        private const int k_MinimumBoardDimension = 4;
        private const int k_MaximumBoardDimension = 10;
        private const int k_GameSettingFormWidth = 300;
        private const int k_GameSettingFormHeight = 300;

        public string Player1Name
        {
            get { return r_TextBoxPlayer1Name.Text; }
        }

        public string Player2Name
        {
            get { return r_TextBoxPlayer2Name.Text; }
        }

        public int BoardHeight
        {
            get { return (int)r_NumericUpDownSetRows.Value; }
        }

        public int BoardWidth
        {
            get { return (int)r_NumericUpDownSetCols.Value; }
        }

        public eGameMode GameMode
        { 
            get { return getGameMode(); }
        }

        public FormGameSetting()
        {
            this.Text = "Game Settings";
            this.Size = new Size(k_GameSettingFormHeight, k_GameSettingFormWidth);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            initControls();
        }

        private void initControls() // Blank lines between controls for better readability
        {
            r_LabelPlayers.AutoSize = true;
            r_LabelPlayers.Text = "Players:";
            r_LabelPlayers.Location = new Point(20, 18);

            r_LabelPlayer1.AutoSize = true;
            r_LabelPlayer1.Width = 50;
            r_LabelPlayer1.Text = "Player 1:";
            r_LabelPlayer1.Top = r_LabelPlayers.Bottom + 10;
            r_LabelPlayer1.Left = r_LabelPlayers.Left + 10;

            r_TextBoxPlayer1Name.Left = r_LabelPlayer1.Right + 28;
            r_TextBoxPlayer1Name.Top = r_LabelPlayer1.Top;

            r_CheckBoxVsComputer.AutoSize = true;
            r_CheckBoxVsComputer.Width = 50;
            r_CheckBoxVsComputer.Text = "Player 2:";
            r_CheckBoxVsComputer.Top = r_LabelPlayer1.Bottom + 10;
            r_CheckBoxVsComputer.Left = r_LabelPlayer1.Left + 2;

            r_TextBoxPlayer2Name.Text = "[Computer]";
            r_TextBoxPlayer2Name.Top = r_CheckBoxVsComputer.Top;
            r_TextBoxPlayer2Name.Left = r_CheckBoxVsComputer.Right + 28;
            r_TextBoxPlayer2Name.Enabled = false;

            r_LabelBoardSize.AutoSize = true;
            r_LabelBoardSize.Text = "Board Size:";
            r_LabelBoardSize.Left = r_LabelPlayers.Left;
            r_LabelBoardSize.Top = r_CheckBoxVsComputer.Bottom + 30;

            r_LabelRows.AutoSize = true;
            r_LabelRows.Text = "Rows:";
            r_LabelRows.Width = 35;
            r_LabelRows.Left = r_LabelBoardSize.Left + 10;
            r_LabelRows.Top = r_LabelBoardSize.Bottom + 5;

            r_NumericUpDownSetRows.AutoSize = true;
            r_NumericUpDownSetRows.Top = r_LabelRows.Top + (r_LabelRows.Height / 2) - (r_NumericUpDownSetRows.Height / 2) - 3;
            r_NumericUpDownSetRows.Left = r_LabelRows.Right + 4;
            r_NumericUpDownSetRows.Width = 40;
            r_NumericUpDownSetRows.Minimum = k_MinimumBoardDimension;
            r_NumericUpDownSetRows.Maximum = k_MaximumBoardDimension;

            r_LabelCols.AutoSize = true;
            r_LabelCols.Text = "Cols:";
            r_LabelCols.Width = 35;
            r_LabelCols.Left = r_NumericUpDownSetRows.Right + 30;
            r_LabelCols.Top = r_LabelBoardSize.Bottom + 5;

            r_NumericUpDownSetCols.AutoSize = true;
            r_NumericUpDownSetCols.Width = 40;
            r_NumericUpDownSetCols.Left = r_LabelCols.Right;
            r_NumericUpDownSetCols.Top = r_NumericUpDownSetRows.Top;
            r_NumericUpDownSetCols.Minimum = k_MinimumBoardDimension;
            r_NumericUpDownSetCols.Maximum = k_MaximumBoardDimension;

            r_ButtonStart.Text = "Start!";
            r_ButtonStart.Width = k_GameSettingFormWidth - 50;
            r_ButtonStart.Top = (this.ClientSize.Height - r_ButtonStart.Height) - 20;
            r_ButtonStart.Left = (this.ClientSize.Width - r_ButtonStart.Width) / 2;

            this.Controls.AddRange(
                new Control[]
                    {
                        r_LabelPlayers, r_LabelPlayer1, r_LabelBoardSize, r_LabelRows, r_LabelCols,
                        r_CheckBoxVsComputer, r_TextBoxPlayer1Name, r_TextBoxPlayer2Name, r_NumericUpDownSetRows,
                        r_NumericUpDownSetCols, r_ButtonStart
                    });
            
            r_ButtonStart.Click += new EventHandler(m_ButtonStart_Click);
            r_CheckBoxVsComputer.CheckedChanged += new EventHandler(m_CheckBoxVsComputer_Checked);
            r_NumericUpDownSetRows.ValueChanged += new EventHandler(m_NumericUpDownSetRows_ValueChanged);
            r_NumericUpDownSetCols.ValueChanged += new EventHandler(m_NumericUpDownSetCols_ValueChanged);
        }

        private void m_NumericUpDownSetRows_ValueChanged(object i_Sender, EventArgs i_E)
        {
            NumericUpDown rows = i_Sender as NumericUpDown;

            r_NumericUpDownSetRows.Value = rows.Value;
        }

        private void m_NumericUpDownSetCols_ValueChanged(object i_Sender, EventArgs i_E)
        {
            NumericUpDown cols = i_Sender as NumericUpDown;

            r_NumericUpDownSetCols.Value = cols.Value;
        }

        private void m_CheckBoxVsComputer_Checked(object i_Sender, EventArgs i_E)
        {
            r_TextBoxPlayer2Name.Enabled = (i_Sender as CheckBox).CheckState == CheckState.Checked;
            if(r_TextBoxPlayer2Name.Enabled)
            {
                r_TextBoxPlayer2Name.Text = null;
            }
            else
            {
                r_TextBoxPlayer2Name.Text = "[Computer]";
            }
        }

        private void m_ButtonStart_Click(object i_Sender, EventArgs i_E)
        {
            if (r_TextBoxPlayer1Name.Text == string.Empty)
            {
                r_TextBoxPlayer1Name.Text = "Player 1";
            }

            if (r_TextBoxPlayer2Name.Text == string.Empty)
            {
                r_TextBoxPlayer2Name.Text = "Player 2";
            }
           
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private eGameMode getGameMode()
        {
            eGameMode gameMode;

            if(r_CheckBoxVsComputer.Checked)
            {
                gameMode = eGameMode.VsFriend;
            }
            else
            {
                gameMode = eGameMode.VsComputer;
            }

            return gameMode;
        }
    }
}