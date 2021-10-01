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

        public AnimationData(GameObject go, string name, SpriteRenderer sr, float animSpeed) : base(go, name)
        {
            spriteRenderer = sr;
            animationSpeed = animSpeed;
            frames = (SceneManager.CurrentScene.Textures[spriteRenderer.Texture].Width / (int)spriteRenderer.DrawArea.X)-1;
        }

        /// <summary>
        /// Swap to a different loaded texture from the current scene
        /// reset animation and frame
        /// </summary>
        /// <param name="texID">the new texture name</param>
        /// <param name="animSpeed">the speed in seconds between frames</param>
        /// <param name="animation">the Y index animation for the spritesheet</param>
        /// <param name="frame">the current X index for the animation on the spritesheet</param>
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

        /// <summary>
        /// Change to a different animation on the same spritesheet
        /// </summary>
        /// <param name="anim">the Y index for the spritesheet animation</param>
        public void ChangeAnimation(byte anim)
        {
            if(anim >= 0 &&
                anim < (SceneManager.CurrentScene.Textures[spriteRenderer.Texture].Height / (int)spriteRenderer.DrawArea.Y) - 1)
            {
                spriteRenderer.Animation = anim;
                spriteRenderer.CurrentFrame = 0;
                timeSinceFrameChange = 0;
            }
        }

        /// <summary>
        /// Alter speed between animation frames
        /// </summary>
        /// <param name="speed">Time between frames in seconds</param>
        public void ChangeAnimationSpeed(float speed)
        {
            animationSpeed = speed;
        }

        /// <summary>
        /// Move the animation forward by one step based on gametime
        /// </summary>
        /// <param name="gt">GameTime since last gameloop</param>
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
