using System.Drawing;

namespace Logic4InARow
{
    public class Player
    {
        private readonly eCoinFigure r_PlayerSign;
        private readonly string r_PlayerName;
        private Point m_LastMoveByPlayer;
        private int m_ScoreCounter;

        public Player(eCoinFigure i_Sign, string i_PlayerName)
        {
            r_PlayerName = i_PlayerName;
            r_PlayerSign = i_Sign;
            m_LastMoveByPlayer = new Point(0, 0);
            m_ScoreCounter = 0;
        }

        public override bool Equals(object i_Obj)
        {
            Player toConvert = i_Obj as Player;

            return toConvert.PlayerSign == this.PlayerSign;
        }

        public Point LastMove
        {
            get { return m_LastMoveByPlayer; }
            set { m_LastMoveByPlayer = value; }
        }

        public eCoinFigure PlayerSign
        {
            get { return r_PlayerSign; }
        }

        public int ScoreCounter
        {
            get { return m_ScoreCounter; }
            set { m_ScoreCounter++; }
        }

        public string PlayersName
        {
            get { return r_PlayerName; }
        }
    }
}