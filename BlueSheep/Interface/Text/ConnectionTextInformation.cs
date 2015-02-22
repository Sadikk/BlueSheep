using System;
using System.Drawing;

namespace BlueSheep.Interface.Text
{
    class ConnectionTextInformation : TextInformation
    {
        #region Fields
        private string m_Text;
        #endregion

        #region Properties
        public override string Text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        public override Color Color
        {
            get { return Color.Green; }
        }

        public override string Category
        {
            get { return "Connexion"; }
            set { throw new NotImplementedException(); }
        }

        public override string Account { get; set; }
        #endregion

        #region Constructeurs
        public ConnectionTextInformation(string text)
        {
            m_Text = text;
        }
        #endregion
    }
}
