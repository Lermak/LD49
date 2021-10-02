using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGame_Core.Scripts
{
    public static class Coroutines
    {
        public static IEnumerator<bool> FadeInSceneTransision(SceneManager sm)
        {
            while (RenderingManager.GlobalFade > 0)
            {
                RenderingManager.GlobalFade -= 255 * TimeManager.DeltaTime;
                SoundManager.SetVolume(1 - (RenderingManager.GlobalFade / 256));
                if (RenderingManager.GlobalFade < 0)
                {
                    RenderingManager.GlobalFade = 0;
                    sm.SceneState = SceneManager.State.Running;
                }
                yield return false;
            }
            yield return true;
        }
        public static IEnumerator<bool> FadeOutSceneTransision(SceneManager sm)
        {
            while (RenderingManager.GlobalFade < 255)
            {
                RenderingManager.GlobalFade += 255 * TimeManager.DeltaTime;
                SoundManager.SetVolume(1 - (RenderingManager.GlobalFade / 256));
                if (RenderingManager.GlobalFade > 255)
                {
                    RenderingManager.GlobalFade = 255;
                    foreach (GameObject o in sm.CurrentScene.GameObjects)
                    {
                        o.Destroy();
                        o.OnDestroy();
                    }
                    sm.CurrentScene = null;
                }
                yield return false;
            }

            yield return true;
        }

        public static IEnumerator<bool> ScreenShake(float duration, int min, int max, Transform t)
        {
            float timeElapsed = 0;
            Vector2 origonalPos = t.Position;
            Random r = new Random();
            int dir = -1;
            while (timeElapsed < duration)
            {
                t.Place(origonalPos);
                timeElapsed += TimeManager.DeltaTime;
                t.Move(new Vector2(r.Next(min, max) * dir, 1));
                dir *= -1;
                yield return false;
            }
            t.Place(origonalPos);
            yield return true;
        }

        public static IEnumerator<bool> RunAnimation(byte animToRun, byte animAfterRun, AnimationData ad)
        {
            ad.ChangeAnimation(animToRun);
            while (ad.CurrentFrame < ad.Frames && ad.TimeSinceFrameChange < ad.AnimationSpeed)
            {
                yield return false;
            }
            ad.ChangeAnimation(animAfterRun);
            yield return true;
        }
    }
}
