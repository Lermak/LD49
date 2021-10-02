using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace MonoGame_Core.Scripts
{
    public delegate void TextBoxEvent(TextBox sender);
    public class TextBox : GameObject, IKeyboardSubscriber
    {
        Texture2D _textBoxTexture;
        Texture2D _caretTexture;

        SpriteFont _font;

        public bool Highlighted { get; set; }

        public bool PasswordBox { get; set; }

        public event TextBoxEvent Clicked;

        public Transform Transform { get { return (Transform)componentHandler.GetComponent("transform"); } }

        string _text = "";
        public String Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                if (_text == null)
                    _text = "";

                if (_text != "")
                {
                    //if you attempt to display a character that is not in your font
                    //you will get an exception, so we filter the characters
                    //remove the filtering if you're using a default character in your spritefont
                    String filtered = "";
                    foreach (char c in value)
                    {
                        if (_font.Characters.Contains(c))
                            filtered += c;
                    }

                    _text = filtered;
                }
            }
        }

        public TextBox(string textBoxTexture, string font, string tag, Vector2 size, Vector2 pos, byte layer) : base(tag)
        {
            _font = ResourceManager.Fonts[font];

            componentHandler.AddComponent(new Transform(this, pos, size.X, size.Y, 0, layer));
            componentHandler.AddComponent(new TextBoxRenderer(this,
                                            Text,
                                            textBoxTexture,
                                            font,
                                            Transform,
                                            new Vector2(0, 0),
                                            size,
                                            0, Color.White));

            _previousMouse = Mouse.GetState();

            behaviorHandler.AddBehavior("Update", OnUpdate, new Component[] {});

            CurrentWindow.keyboardDispatcher.Subscriber = this;
        }

        MouseState _previousMouse;
        public void OnUpdate(float gt, Component[] c)
        {
            float X = Transform.Position.X;
            float Y = Transform.Position.Y;
            float Width = Transform.Width;
            float Height = Transform.Height;

            MouseState mouse = Mouse.GetState();
            Point mousePoint = new Point(mouse.X, mouse.Y);

            if (Transform.ContainsPoint(new Vector2(mouse.X, mouse.Y)))
            {
                Highlighted = true;
                if (_previousMouse.LeftButton == ButtonState.Released && mouse.LeftButton == ButtonState.Pressed)
                {
                    if (Clicked != null)
                        Clicked(this);
                }
            }
            else
            {
                Highlighted = false;
            }

            TextBoxRenderer r = (TextBoxRenderer)componentHandler.GetComponent("textboxrenderer");
            r.Text = Text;
        }

        public void RecieveTextInput(char inputChar)
        {
            Text = Text + inputChar;
        }
        public void RecieveTextInput(string text)
        {
            Text = Text + text;
        }
        public void RecieveCommandInput(char command)
        {
            switch (command)
            {
                case '\b': //backspace
                    if (Text.Length > 0)
                        Text = Text.Substring(0, Text.Length - 1);
                    break;
                case '\r': //return
                    if (OnEnterPressed != null)
                        OnEnterPressed(this);
                    break;
                case '\t': //tab
                    if (OnTabPressed != null)
                        OnTabPressed(this);
                    break;
                default:
                    break;
            }
        }
        public void RecieveSpecialInput(Keys key)
        {

        }

        public event TextBoxEvent OnEnterPressed;
        public event TextBoxEvent OnTabPressed;

        public bool Selected
        {
            get;
            set;
        }
    }
}