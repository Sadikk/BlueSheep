using System;
using System.Drawing;

namespace BlueSheep.Interface.Text
{
    class ErrorTextInformation : TextInformation
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
            get { return Color.Red; }
        }

        public override string Category
        {
            get { return "Erreur"; }
            set { throw new NotImplementedException(); }
        }

        public override string Account
        {
            get { return "Erreur"; }
            set { throw new NotImplementedException(); }
        }
        #endregion

        #region Constructeurs
        public ErrorTextInformation(string text)
        {
            m_Text = text;
        }
        #endregion
    }
}
