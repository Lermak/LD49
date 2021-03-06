using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MonoGame_Core.Scripts
{
    public class SpriteRenderer : Component
    {
        protected string texture;
        protected int orderInLayer;
        protected Color color;
        protected Transform transform;
        protected Vector2 offSet;
        protected Vector2 drawArea;
        protected string shader = "";
        protected List<Camera> cameras;
        protected SpriteEffects flip = SpriteEffects.None;
        protected bool isHUD = false;
        protected bool visible = true;
        protected float addedRotation = 0;
        protected int currentFrame = 0;
        protected byte animation = 0;

        public virtual string Texture { get { return texture; } 
            set {
                if (ResourceManager.Textures.ContainsKey(value))
                    texture = value;
                else
                    texture = null;
            } 
        }
        public int OrderInLayer { get { return orderInLayer; } }
        public Color Color { get { return color; } }
        public Transform Transform { get { return transform; } }
        public Vector2 Offset { get { return offSet; } }
        public Vector2 DrawArea { get { return drawArea; } }
        public string Shader { get { return shader; } 
            set {
                if (ResourceManager.Effects.ContainsKey(value))
                    shader = value;
                else
                    shader = "";
            } 
        }
        public SpriteEffects Flip { get { return flip; } set { flip = value; } }
        public bool IsHUD { get { return isHUD; } set { isHUD = value; } }
        public bool Visible { get { return visible; } set { visible = value; } }
        public float AddedRotation { get { return addedRotation; } set { addedRotation = value; } }
        public List<Camera> Cameras { get { return cameras; } }
        public byte Animation { get { return animation; } set { animation = value; } }
        public int CurrentFrame { get { return currentFrame; } set { currentFrame = value; } }

        public SpriteRenderer(GameObject go, string texID, Transform t, Vector2 off, Vector2 drawArea, int orderInLayer, Color clr) : base(go, "spriteRenderer")
        {
            cameras = new List<Camera>() { CurrentWindow.sceneManager.Cameras[0] };
            Texture = texID;
            transform = t;
            offSet = off;
            this.orderInLayer = orderInLayer;
            this.drawArea = drawArea;
            color = clr;

            RenderingManager.Sprites.Add(this);
        }
        public SpriteRenderer(GameObject go, string texID, Transform t, Vector2 off, Vector2 drawArea, int orderInLayer) : base(go, "spriteRenderer")
        {
            cameras = new List<Camera>() { CurrentWindow.sceneManager.Cameras[0] };
            Texture = texID;
            transform = t;
            offSet = off;
            this.orderInLayer = orderInLayer;
            this.drawArea = drawArea;
            color = Color.White;

            RenderingManager.Sprites.Add(this);
        }

        public Rectangle DrawRect()
        {
            return new Rectangle(currentFrame * (int)DrawArea.X, animation * (int)DrawArea.Y, (int)DrawArea.X, (int)DrawArea.Y);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            RenderingManager.Sprites.Remove(this);
        }

        public void SetDrawArea(float width, float height)
        {
            drawArea.X = width;
            drawArea.Y = height;
        }

        public virtual void Draw(SpriteBatch sb, Camera c)
        {
            if (isHUD)
            {
                sb.Draw(ResourceManager.Textures[Texture],
                    ScreenPosition(c),
                    DrawRect(),
                    new Color(Color.R - (int)CurrentWindow.GlobalFade, Color.G - (int)CurrentWindow.GlobalFade, Color.B - (int)CurrentWindow.GlobalFade, Color.A),
                    -(Transform.Radians + addedRotation),
                    new Vector2(Transform.Width / 2, Transform.Height / 2),
                    RenderingManager.WindowScale * Transform.Scale,
                    Flip,
                    1);
            }
            else
            {
                sb.Draw(ResourceManager.Textures[Texture],
                    ScreenPosition(c),
                    DrawRect(),
                    new Color(Color.R - (int)CurrentWindow.GlobalFade, Color.G - (int)CurrentWindow.GlobalFade, Color.B - (int)CurrentWindow.GlobalFade, Color.A),
                    -(Transform.Radians + addedRotation),
                    new Vector2(Transform.Width / 2, Transform.Height / 2),
                    RenderingManager.GameScale * Transform.Scale,
                    Flip,
                    (float)transform.Layer / 256f);
            }
        }

        protected Vector2 ScreenPosition(Camera camera)
        {
            if (isHUD)
                return Transform.WorldPosition(offSet) + (new Vector2(RenderingManager.WIDTH / 2, RenderingManager.HEIGHT / 2) * RenderingManager.WindowScale);

            else
                return (Transform.WorldPosition(offSet) - camera.Position + (new Vector2(RenderingManager.WIDTH / 2, RenderingManager.HEIGHT / 2) * RenderingManager.WindowScale));
        }
    }
}
