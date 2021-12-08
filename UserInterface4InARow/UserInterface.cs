using System.Windows.Forms;

namespace UserInterface4InARow
{
    public class UserInterface
    {
        private FormGameSetting m_GameSettingForm;
        private FormGamePlay m_GamePlayForm;

        public void Start4InARowGame()
        {
            m_GameSettingForm = new FormGameSetting();
            m_GameSettingForm.ShowDialog();
            if (m_GameSettingForm.DialogResult == DialogResult.OK)
            {
                m_GamePlayForm = new FormGamePlay(m_GameSettingForm.BoardWidth, m_GameSettingForm.BoardHeight,
                    m_GameSettingForm.GameMode, m_GameSettingForm.Player1Name, m_GameSettingForm.Player2Name);
                m_GamePlayForm.ShowDialog();
            }
        }
    }
}