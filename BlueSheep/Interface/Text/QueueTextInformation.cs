using System;
using System.Drawing;

namespace BlueSheep.Interface.Text
{
    class QueueTextInformation : TextInformation
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
            get { return "File d'attente"; }
            set { throw new NotImplementedException(); }
        }

        public override string Account { get; set; }
        #endregion
        
        #region Constructeurs
        public QueueTextInformation(string text)
        {
            m_Text = text;
        }
        #endregion
    }
}
