using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Core.Scripts
{
    public static class DigiPetBehaviors
    {
        public static void Play(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            Vector2 v = CurrentWindow.inputManager.MousePos;
            DigiPetData d = ((DigiPetData)c[1]);
            AnimationData ad = ((AnimationData)c[2]);
            AnimationData dpad = ((AnimationData)c[3]);
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                if (ad.SpriteRenderer.Texture == "PlayUp")
                {
                    if (d.Code.Count > 4)
                        d.Code.Dequeue();
                    d.Code.Enqueue('p');

                    Random r = new Random();
                    SoundManager.PlaySoundEffect(Globals.ClickSounds[r.Next(0, 3)]);
                }
                ad.ChangeSpriteSheet("PlayDown", 0);

                if (!Globals.DigiPetAlive)
                {

                }
                else if (!d.NeedsPlay && dpad.SpriteRenderer.Animation < 3)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, dpad), "RejectAnimate", 0, true);
                }
                else if (!d.Playing)
                {
                    d.NeedsPlay = false;
                    d.Playing = true;
                    SoundManager.PlaySoundEffect("DigiPetSuccess");
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(3, 0, dpad), "PlayingAnimate", 0, true);
                    d.TimeSinceLastPlay = 0;
                    d.Needs.SpriteRenderer.Animation = 0;

                }
            }
            else
            {
                d.Playing = false;
                ad.ChangeSpriteSheet("PlayUp", 0);
            }
        }
        public static void Wash(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            Vector2 v = CurrentWindow.inputManager.MousePos;
            DigiPetData d = ((DigiPetData)c[1]);
            AnimationData ad = ((AnimationData)c[2]);
            AnimationData dpad = ((AnimationData)c[3]);
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                if (ad.SpriteRenderer.Texture == "WashUp")
                {
                    if (d.Code.Count > 4)
                        d.Code.Dequeue();
                    d.Code.Enqueue('w');

                    Random r = new Random();
                    SoundManager.PlaySoundEffect(Globals.ClickSounds[r.Next(0, 3)]);
                }
                ad.ChangeSpriteSheet("WashDown", 0);
                if (!Globals.DigiPetAlive)
                {

                }
                else if (!d.NeedsWash && dpad.SpriteRenderer.Animation < 3)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, dpad), "RejectAnimate", 0, true);
                }
                else if (!d.Washing)
                {
                    d.NeedsWash = false;
                    d.Washing = true;
                    SoundManager.PlaySoundEffect("DigiPetSuccess");
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(3, 0, dpad), "WashingAnimate", 0, true);
                    d.TimeSinceLastWash = 0;
                    d.Needs.SpriteRenderer.Animation = 0;

                }
            }
            else
            {
                d.Washing = false;
                ad.ChangeSpriteSheet("WashUp", 0);
            }
        }
        public static void Feed(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            Vector2 v = CurrentWindow.inputManager.MousePos;
            DigiPetData d = ((DigiPetData)c[1]);
            AnimationData ad = ((AnimationData)c[2]);
            AnimationData dpad = ((AnimationData)c[3]);
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                if (ad.SpriteRenderer.Texture == "FeedUp")
                {
                    if (d.Code.Count > 4)
                        d.Code.Dequeue();
                    d.Code.Enqueue('f');

                    Random r = new Random();
                    SoundManager.PlaySoundEffect(Globals.ClickSounds[r.Next(0, 3)]);
                }

                ad.ChangeSpriteSheet("FeedDown", 0);
                if(!Globals.DigiPetAlive)
                {

                }
                else if (!d.NeedsFood && dpad.SpriteRenderer.Animation < 3)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, dpad), "RejectAnimate", 0, true);
                }
                else if (!d.Feeding)
                {
                    d.NeedsFood = false;
                    d.Feeding = true;
                    SoundManager.PlaySoundEffect("DigiPetSuccess");
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(3, 0, dpad), "FeedingAnimate", 0, true);
                    d.TimeSinceLastFeed = 0;
                    d.Needs.SpriteRenderer.Animation = 0;
                }
            }
            else
            {
                d.Feeding = false;
                ad.ChangeSpriteSheet("FeedUp", 0);
            }
        }

        public static void Running(float gt, Component[] c)
        {
            DigiPetData d = ((DigiPetData)c[0]);
            AnimationData a = ((AnimationData)c[1]);
            if (d.NeedsFood)
                d.TimeSinceLastFeed += gt;
            if(d.NeedsPlay)
                d.TimeSinceLastPlay += gt;
            if(d.NeedsWash)
                d.TimeSinceLastWash += gt;

            List<char> code = d.Code.ToList<char>();
            List<char> sequence = new List<char>() { 'w', 'f', 'f', 'p', 'f' };
            bool flag = true;
            if (code.Count == 5)
            {
                for (int i = 0; i < 5; ++i)
                    if (code[i] != sequence[i])
                    {
                        flag = false;
                        break;
                    }
            }
            else
                flag = false;

            if (d.CodeAccessed == false && flag)
            {
                SoundManager.PlaySoundEffect("MysterySound");
                SoundManager.SoundEffects["MysterySound"].Volume = .1f;

                GameManager.plotManager.digipet_secret = true;

                d.CodeAccessed = true;
                d.NeedsFood = false;
                d.NeedsPlay = false;
                d.NeedsWash = false;
                d.TimeSinceLastFeed = 0;
                d.TimeSinceLastPlay = 0;
                d.TimeSinceLastWash = 0;
                
                CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, a), "CodeAnimate", 0, true);
                //SoundManager.PlaySoundEffect("CodeConfirmation");
                string fileName = @".\OverrideCommands.txt";

                try
                {
                    // Check if file already exists. If yes, delete it.     
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    // Create a new file     
                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.WriteLine("OVERRIDE CODE");
                    }
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.ToString());
                }

            }
            else if(!d.NeedsFood && !d.NeedsPlay && !d.NeedsWash)
            {
                d.CheckNeedsTimer += gt;
                if (d.CheckNeedsTimer > 20)
                {
                    d.CheckNeedsTimer = 0;
                    Random r = new Random();
                    int i = r.Next(0, 100);
                    if (i < 10)
                    {
                        d.Needs.SpriteRenderer.Animation = 1;
                        d.NeedsFood = true;
                    }
                    else if (i < 20)
                    {
                        d.Needs.SpriteRenderer.Animation = 2;
                        d.NeedsPlay = true;
                    }
                    else if (i < 30)
                    {
                        d.Needs.SpriteRenderer.Animation = 3;
                        d.NeedsWash = true;
                    }
                    if(i < 30)
                    {
                        SoundManager.PlaySoundEffect("DigiPetWant");
                        SoundManager.SoundEffects["DigiPetWant"].Volume = .3f;

                    }

                }
            }

            if ((d.TimeSinceLastPlay > 30 ||
                d.TimeSinceLastFeed > 30 ||
                d.TimeSinceLastWash > 30) &&
                Globals.DigiPetAlive == true)
            {
                Globals.DigiPetAlive = false;
                d.Needs.SpriteRenderer.Animation = 0;
                a.ChangeAnimation(4);
            }
            WorldObject w = ((WorldObject)a.GameObject);

            if (Globals.DigiPetAlive)
            {
                if (a.SpriteRenderer.Animation < 3)
                {
                    d.TimeSinceAnimation += gt;
                    if (d.TimeSinceAnimation > d.AnimationDuration)
                    {
                        d.TimeSinceAnimation = 0;
                        Random r = new Random();
                        int i = r.Next(0, 100);
                        if (i < 40)
                        {
                            if (w.Transform.Position.X <= -900)
                                w.SpriteRenderer.Animation = 1;
                            else if (w.Transform.Position.X >= -540)
                                w.SpriteRenderer.Animation = 2;
                            else if (i < 20)
                                w.SpriteRenderer.Animation = 1;
                            else
                                w.SpriteRenderer.Animation = 2;
                        }
                        else if (w.SpriteRenderer.Animation < 3)
                            w.SpriteRenderer.Animation = 0;
                    }

                    if (w.Transform.Position.X < -900 && w.SpriteRenderer.Animation == 2)
                    {
                        a.SpriteRenderer.Animation = 0;
                    }

                    if (w.Transform.Position.X > -540 && w.SpriteRenderer.Animation == 1)
                    {
                        a.SpriteRenderer.Animation = 0;
                    }
                }
            }
            else
            {
                d.TimeDead += gt;
                if (d.TimeDead > 40 && w.SpriteRenderer.Animation < 6)
                {
                    foreach (GameObject g in WindowManager.DigiPetWindow.sceneManager.CurrentScene.GameObjects.Where(f => f.Tag == "Fly"))
                        g.Destroy();
                    w.SpriteRenderer.Animation = 6;
                }
                else if (d.TimeDead > 20 && w.SpriteRenderer.Animation < 5)
                {
                    Vector2 pos = ((WorldObject)d.GameObject).Transform.Position;
                    WindowManager.DigiPetWindow.sceneManager.CurrentScene.ToAdd.Add(
                        new Fly(pos + new Vector2(-30, 60),
                        new List<Vector2>()
                        {
                            pos + new Vector2(-15, 75),
                            pos + new Vector2(-20, 55),
                            pos + new Vector2(-30, 60)
                        }));
                    WindowManager.DigiPetWindow.sceneManager.CurrentScene.ToAdd.Add(
                        new Fly(pos + new Vector2(20, 80),
                        new List<Vector2>()
                        {
                            pos + new Vector2(10, 70),
                            pos + new Vector2(5, 75),
                            pos + new Vector2(20, 80)
                        }));

                    w.SpriteRenderer.Animation = 5;
                }

            }

            if (w.SpriteRenderer.Animation == 1)
                w.RigidBody.MoveVelocity = new Vector2(1, 0);
            else if (w.SpriteRenderer.Animation == 2)
                w.RigidBody.MoveVelocity = new Vector2(-1, 0);
            else
                w.RigidBody.MoveVelocity = new Vector2(0, 0);

            if(w.RigidBody.MoveVelocity != new Vector2())
            {
                if (!SoundManager.SoundEffects.ContainsKey("Walk") || SoundManager.SoundEffects["Walk"].State != Microsoft.Xna.Framework.Audio.SoundState.Playing)
                {
                    d.TimeSinceLastSound += gt;
                    if (d.TimeSinceLastSound > .5f)
                    {
                        d.TimeSinceLastSound = 0;
                        string[] sounds = { "DigiPetWalk1", "DigiPetWalk2", "DigiPetWalk3", "DigiPetWalk4" };
                        int i = d.PrevWalkSound + 1;
                        if (i >= 4)
                        {
                            i = 0;
                        }
                        SoundManager.PlaySoundEffect(sounds[i]);
                        SoundManager.SoundEffects[sounds[i]].Volume = .1f;
                        d.PrevWalkSound = i;
                    }
                }
            }
        }

        public static void Animate(float gt, Component[] c)
        {
            ((AnimationData)c[0]).Animate(gt);
        }

        public static void Fly(float gt, Component[] c)
        {
            FlyData f = (FlyData)c[0];
            Transform t = (Transform)c[1];
            f.TimeSinceLastMove += gt;
            if(f.TimeSinceLastMove > 2)
            {
                f.TimeSinceLastMove = 0;

                f.PositionIndex++;
                if (f.PositionIndex >= f.Positions.Count)
                    f.PositionIndex = 0;
                t.Place(f.Positions[f.PositionIndex]);
            }    
        }
    }
}
