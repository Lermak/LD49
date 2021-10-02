using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MonoGame_Core.Scripts
{
    public class NoCloseForm : Form
    {
        //Disable close button
        private const int CP_DISABLE_CLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CP_DISABLE_CLOSE_BUTTON;
                return cp;
            }
        }
    }
}
