using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Core.Scripts
{
    public class TextBoxRenderer : SpriteRenderer
    {
        private string text;
        float textScale = 5;
        public string Text { get { return text; } set { text = value; } }
        public bool Highlighted = false;
        public bool PasswordBox = false;
        public bool Selected = false;

        public float timePasssed;

        string fontId;

        public override string Texture
        {
            get { return texture; }
            set
            {
                if (ResourceManager.Textures.ContainsKey(value))
                    texture = value;
                else
                    texture = null;
            }
        }
        public TextBoxRenderer(GameObject go, string text, string texID, string fontId, Transform t, Vector2 off, Vector2 drawArea, int orderInLayer, Color clr) : base(go, texID, t, off, drawArea, orderInLayer, clr)
        {
            name = "fontRenderer";
            this.fontId = fontId;
            this.text = text;
            this.name = "textboxrenderer";
        }

        public override void Draw(SpriteBatch spriteBatch, Camera c)
        {
            var font = ResourceManager.Fonts[fontId];
            var caretTexture = ResourceManager.Textures["CarretTexture"];
            var textboxTexture = ResourceManager.Textures[Texture];

            bool caretVisible = true;

            timePasssed += TimeManager.DeltaTime;

            if ((timePasssed % 1000) < 500)
                caretVisible = false;
            else
                caretVisible = true;

            String toDraw = Text;

            if (PasswordBox)
            {
                toDraw = "";
                for (int i = 0; i < Text.Length; i++)
                    toDraw += (char)0x2022; //bullet character (make sure you include it in the font!!!!)
            }

            float X = Transform.Position.X;
            float Y = Transform.Position.Y;
            float Width = Transform.Width;
            float Height = Transform.Height;

            //my texture was split vertically in 2 parts, upper was unhighlighted, lower was highlighted version of the box
            spriteBatch.Draw(textboxTexture, new Rectangle((int)X, (int)Y, (int)Width, (int)Height), new Rectangle(0, Highlighted ? (textboxTexture.Height / 2) : 0, textboxTexture.Width, textboxTexture.Height / 2), Color.White);

            Vector2 size = font.MeasureString(toDraw);

            if (caretVisible && Selected)
                spriteBatch.Draw(caretTexture, new Vector2(X + (int)size.X + 2, Y + 2), Color.White); //my caret texture was a simple vertical line, 4 pixels smaller than font size.Y

            //shadow first, then the actual text
            spriteBatch.DrawString(font, toDraw, new Vector2(X, Y) + Vector2.One, Color.Black);
            spriteBatch.DrawString(font, toDraw, new Vector2(X, Y), Color.White);
        }

    }
}
