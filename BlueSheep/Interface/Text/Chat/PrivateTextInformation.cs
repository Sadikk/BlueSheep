using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSheep.Interface.Text.Chat
{
    class PrivateTextInformation : TextInformation
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
                get { return ColorTranslator.FromHtml("#0096AE"); }
            }

            public override string Category
            {
                get { return "Privé"; }
                set { throw new NotImplementedException(); }
            }

            public override string Account
            {
                get { return "Privé"; }
                set { throw new NotImplementedException(); }
            }
            #endregion

        #region Constructeurs
            public PrivateTextInformation(string text)
            {
                m_Text = text;
            }
            #endregion
    }
}
