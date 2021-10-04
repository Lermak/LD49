﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGame_Core.Scripts
{
    public static class Coroutines
    {
        public static IEnumerator<bool> FadeInSceneTransision(SceneManager sm)
        {
            while (CurrentWindow.GlobalFade > 0)
            {
                CurrentWindow.GlobalFade -= 255 * TimeManager.DeltaTime;
                SoundManager.SetVolume(1 - (CurrentWindow.GlobalFade / 256));
                if (CurrentWindow.GlobalFade < 0)
                {
                    CurrentWindow.GlobalFade = 0;
                    sm.SceneState = SceneManager.State.Running;
                }
                yield return false;
            }
            yield return true;
        }
        public static IEnumerator<bool> FadeOutSceneTransision(SceneManager sm)
        {
            while (CurrentWindow.GlobalFade < 255)
            {
                CurrentWindow.GlobalFade += 255 * TimeManager.DeltaTime;
                SoundManager.SetVolume(1 - (CurrentWindow.GlobalFade / 256));
                if (CurrentWindow.GlobalFade > 255)
                {
                    CurrentWindow.GlobalFade = 255;
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

        public static IEnumerator<bool> UpdateNuclear(AnimationData ad)
        {
            ad.SpriteRenderer.Visible = true;
            ad.ChangeSpriteSheet("UpdateOverlay", 2);
            NuclearLevel.Locked = true;
            float timeElapsed = 0;

            while (timeElapsed < 30)
            {
                ad.Animate(TimeManager.DeltaTime);
                timeElapsed += TimeManager.DeltaTime;
                yield return false;
            }
            NuclearLevel.Updating = false;
            NuclearLevel.Updated = true;
            NuclearLevel.Locked = false;
            ad.SpriteRenderer.Visible = false;
            yield return true;
        }

        

        public static IEnumerator<bool> UpdateLater()
        {
            float timeElapsed = 0;

            while (timeElapsed < 300)
            {
                timeElapsed += TimeManager.DeltaTime;
                yield return false;
            }
            WindowManager.AddWindow(new NoCloseForm(), "UpdateWindow", new UpdateRequiredScene(), new Vector2(600, 200));

            yield return true;
        }

        public static IEnumerator<bool> ConnectToServer(FontRenderer fr)
        {
            float timeElapsed = 0;

            while (timeElapsed < 30)
            {
                fr.Text = "Connecting";
                for (int i = 0; i < timeElapsed / 10; ++i)
                    fr.Text += ".";
                timeElapsed += TimeManager.DeltaTime;
                yield return false;
            }
            NuclearLevel.Locked = false;
            WindowManager.KillBadConnection = true;
            yield return true;
        }

    }
}
