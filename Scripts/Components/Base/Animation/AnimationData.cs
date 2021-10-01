using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class AnimationData : Component
    {
        private SpriteRenderer spriteRenderer;
        private int currentFrame = 0;
        private float animationSpeed = 0;
        private float timeSinceFrameChange = 0;
        private int frames;

        public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }

        public AnimationData(GameObject go, int uo, string name, SpriteRenderer sr, float animSpeed) : base(go, uo, name)
        {
            spriteRenderer = sr;
            animationSpeed = animSpeed;
            frames = (SceneManager.CurrentScene.Textures[spriteRenderer.Texture].Width / (int)spriteRenderer.DrawArea.X)-1;
        }

        public void ChangeSpriteSheet(string texID, float animSpeed, byte animation = 0, int frame = 0)
        {
            if (SceneManager.CurrentScene.Textures.ContainsKey(texID))
            {
                currentFrame = frame;
                spriteRenderer.Animation = animation;
                spriteRenderer.Texture = texID;
                animationSpeed = animSpeed;
                frames = (SceneManager.CurrentScene.Textures[spriteRenderer.Texture].Width / (int)spriteRenderer.DrawArea.X) - 1;
            }
        }

        public void ChangeAnimation(byte anim)
        {
            if(anim >= 0 &&
                anim < SceneManager.CurrentScene.Textures[spriteRenderer.Texture].Height / (int)spriteRenderer.DrawArea.Y)
            {
                spriteRenderer.Animation = anim;
                spriteRenderer.CurrentFrame = 0;
                timeSinceFrameChange = 0;
            }
        }

        public void ChangeAnimationSpeed(float speed)
        {
            animationSpeed = speed;
        }

        public void Animate(float gt)
        {
            timeSinceFrameChange += gt;
            if(timeSinceFrameChange >= animationSpeed)
            {
                timeSinceFrameChange = 0;
                currentFrame++;
                if (currentFrame > frames)
                    currentFrame = 0;

                spriteRenderer.CurrentFrame = currentFrame;
            }
        }
    }
}
