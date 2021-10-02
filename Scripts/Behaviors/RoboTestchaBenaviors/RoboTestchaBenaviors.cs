using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public static class RoboTestchaBenaviors
    {
        public static void LimitCharCount(float gt, Component[] c)
        {
            TextBoxRenderer t = ((TextBoxRenderer)c[0]);
            if (t.Text.Length >= 10)
            {
                ((TextBox)c[0].GameObject).Text = t.Text.Substring(0, 10);
                t.Text = t.Text.Substring(0, 10);
            }
        }
    }
}
