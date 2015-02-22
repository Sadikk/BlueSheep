using System;
using System.Drawing;

namespace BlueSheep.Interface.Text
{
    class GeneralTextInformation : TextInformation
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
            get { return Color.Black; }
        }

        public override string Category
        {
            get { return "Général"; }
            set { throw new NotImplementedException(); }
        }

        public override string Account
        {
            get { return "Général"; }
            set { throw new NotImplementedException(); }
        }
        #endregion

        #region Constructeurs
        public GeneralTextInformation(string text)
        {
            m_Text = text;
        }
        #endregion
    }
}
