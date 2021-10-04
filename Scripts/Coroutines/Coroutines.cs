using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

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
            SoundManager.PlaySoundEffect("Lockout");
            SoundManager.SoundEffects["Lockout"].Volume = .3f;
            float timeElapsed = 0;

            while (timeElapsed < 30)
            {
                ad.Animate(TimeManager.DeltaTime);
                timeElapsed += TimeManager.DeltaTime;
                yield return false;
            }
            NuclearLevel.NeedsUpdate = false;
            NuclearLevel.Updating = false;
            NuclearLevel.Updated = true;
            NuclearLevel.Locked = false;
            SoundManager.PlaySoundEffect("Unlock");
            SoundManager.SoundEffects["Unlock"].Volume = .5f;
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

            while (timeElapsed < 4)
            {
                fr.Text = "Connecting";
                for (int i = 0; i < timeElapsed; ++i)
                    fr.Text += ".";
                timeElapsed += TimeManager.DeltaTime;
                yield return false;
            }
            NuclearLevel.Locked = false;
            SoundManager.PlaySoundEffect("Unlock");
            SoundManager.SoundEffects["Unlock"].Volume = .5f;
            WindowManager.KillBadConnection = true;
            yield return true;
        }

        public static IEnumerator WaitTime(float time)
        {
            float t = 0;

            while(t <= time)
            {
                t += TimeManager.DeltaTime;
                yield return false;
            }

            yield return true;
        }

        public static IEnumerator<bool> BootUp(SpriteRenderer sr)
        {
            float timeElapsed = 0;
            SoundManager.PlaySoundEffect("Boot");
            Random r = new Random();
            while (timeElapsed < 4)
            {
                timeElapsed += TimeManager.DeltaTime;
                if (timeElapsed % 1f < .5f)
                {
                    if(r.Next(0,100) < 50)
                    {
                        if(sr.Texture == "DialBack")
                            sr.Texture = "DialBackDark";
                        else
                            sr.Texture = "DialBack";

                    }
                }

                yield return false;
            }
            sr.Texture = "DialBack";
            NuclearLevel.started = true;
            yield return true;
        }

        public static IEnumerator GameOver(SpriteRenderer sr)
        {
            Globals.ButtonNotCool = true;
            float timeElapsed = 0;
            sr.Texture = "DialBackDark";
            SoundManager.PlaySoundEffect("Shutdown");
            while(SoundManager.SoundEffects["Shutdown"].State == SoundState.Playing)
            {
                yield return false;
            }
            while (timeElapsed <= .5f)
            {
                timeElapsed += TimeManager.DeltaTime;

                yield return false;
            }
            SoundManager.PlaySoundEffect("Explosion");
            while (SoundManager.SoundEffects["Explosion"].State == SoundState.Playing)
            {
                yield return false;
            }

            Globals.OverheatGameOver = true;

            yield return true;
        }

        public static IEnumerator EndTimeMusic()
        {
            SoundManager.PlaySong("EndTimesOpening");
            MediaPlayer.IsRepeating = false;
            while (MediaPlayer.State == MediaState.Playing)
            {
                yield return false;
            }
            SoundManager.PlaySong("EndTimes");

            yield return true;
        }

        public static IEnumerator PaylogDownload()
        {
            float timeElapsed = 0;
            GameObject go = new GameObject("Message");
            Transform t = (Transform)go.ComponentHandler.AddComponent(new Transform(go, new Vector2(-835, 315), 250, 50, 0, 8));
            FontRenderer fr = (FontRenderer)go.ComponentHandler.AddComponent(new FontRenderer(go, "Paylog.txt\nDownloaded", "TestFont", t, new Vector2(), new Vector2(250, 50), 0, Color.Black));
            fr.TextScale = .5f;
            WindowManager.MainWindow.sceneManager.CurrentScene.ToAdd.Add(go);
            while (timeElapsed < 10)
            {
                timeElapsed += TimeManager.DeltaTime;
                yield return false;
            }
            go.Destroy();

            yield return true;
        }

        public static IEnumerator OverrideCommandsDownload()
        {
            float timeElapsed = 0;
            GameObject go = new GameObject("Message");
            Transform t = (Transform)go.ComponentHandler.AddComponent(new Transform(go, new Vector2(-740, 510), 250, 50, 0, 8));
            FontRenderer fr = (FontRenderer)go.ComponentHandler.AddComponent(new FontRenderer(go, "OverrideCommands.txt\nDownloaded", "TestFont", t, new Vector2(), new Vector2(250, 50), 0, Color.Black));
            fr.TextScale = .5f;
            WindowManager.MainWindow.sceneManager.CurrentScene.ToAdd.Add(go);
            while (timeElapsed < 10)
            {
                timeElapsed += TimeManager.DeltaTime;
                yield return false;
            }
            go.Destroy();

            yield return true;
        }
    }
}
