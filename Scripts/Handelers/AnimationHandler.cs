using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class AnimationHandler
    {
        private GameObject gameObject;
        private SpriteRenderer spriteRenderer;
        private int currentFrame = 0;
        private float animationSpeed = 0;
        private float timeSinceFrameChange = 0;
        private int frames;

        public SpriteRenderer SpriteRenderer { get { return spriteRenderer; } }
        public GameObject GameObject { get { return gameObject; } }

        public AnimationHandler(GameObject go, SpriteRenderer sr, float animSpeed)
        {
            gameObject = go;
            spriteRenderer = sr;
            animationSpeed = animSpeed;
            frames = SceneManager.CurrentScene.Textures[spriteRenderer.Texture].Width / (int)spriteRenderer.DrawArea.X;
        }

        public void Update(float gt)
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
