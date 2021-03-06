using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Core.Scripts
{
    public class FontRenderer : SpriteRenderer
    {
        private string text;
        float textScale = 1;
        public string Text { get { return text; } set { text = value; } }
        public float TextScale { get { return TextScale; } set { textScale = value; } }
        public override string Texture
        {
            get { return texture; }
            set
            {
                if (ResourceManager.Fonts.ContainsKey(value))
                    texture = value;
                else
                    texture = null;
            }
        }
        public FontRenderer(GameObject go, string text, string texID, Transform t, Vector2 off, Vector2 drawArea, int orderInLayer, Color clr) : base(go, texID, t, off, drawArea, orderInLayer, clr)
        {
            name = "fontRenderer";
            this.text = text;
        }

        public override void Draw(SpriteBatch sb, Camera c)
        {
            Vector2 stringSize = ResourceManager.Fonts[Texture].MeasureString(text) * 0.5f;

            if (isHUD)
            {
                sb.DrawString(ResourceManager.Fonts[Texture],
                    text,
                    ScreenPosition(c),
                    new Color(Color.R - (int)CurrentWindow.GlobalFade, Color.G - (int)CurrentWindow.GlobalFade, Color.B - (int)CurrentWindow.GlobalFade, Color.A),
                    -(Transform.Radians + addedRotation),
                    stringSize,
                    RenderingManager.WindowScale * Transform.Scale * textScale,
                    Flip,
                    1);
            }
            else
            {
                sb.DrawString(ResourceManager.Fonts[Texture],
                    text,
                    ScreenPosition(c),
                    new Color(Color.R - (int)CurrentWindow.GlobalFade, Color.G - (int)CurrentWindow.GlobalFade, Color.B - (int)CurrentWindow.GlobalFade, Color.A),
                    -(Transform.Radians + addedRotation),
                    stringSize,
                    RenderingManager.GameScale * Transform.Scale * textScale,
                    Flip,
                    (float)transform.Layer / 256);
            }
        }

    }
}
