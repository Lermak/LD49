﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

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
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                if(CurrentWindow.inputManager.IsMouseTriggered(InputManager.MouseKeys.LeftButton))
                {
                    if (d.Code.Count > 4)
                        d.Code.Dequeue();
                    d.Code.Enqueue('p');
                }

                ad.ChangeSpriteSheet("PlayDown", 0);

                if (!Globals.DigiPetAlive)
                {

                }
                else if (!d.NeedsPlay && ad.SpriteRenderer.Animation < 3)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "RejectAnimate", 0, true);
                }
                else if (!d.Playing)
                {
                    d.NeedsPlay = false;
                    d.Playing = true;
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(3, 0, ad), "PlayingAnimate", 0, true);
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
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {

                if (CurrentWindow.inputManager.IsMouseTriggered(InputManager.MouseKeys.LeftButton))
                {
                    if (d.Code.Count > 4)
                        d.Code.Dequeue();
                    d.Code.Enqueue('w');
                }

                ad.ChangeSpriteSheet("WashDown", 0);
                if (!Globals.DigiPetAlive)
                {

                }
                else if (!d.NeedsWash && ad.SpriteRenderer.Animation < 3)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "RejectAnimate", 0, true);
                }
                else if (!d.Washing)
                {
                    d.NeedsWash = false;
                    d.Washing = true;
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(3, 0, ad), "WashingAnimate", 0, true);
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
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                if (CurrentWindow.inputManager.IsMouseTriggered(InputManager.MouseKeys.LeftButton))
                {
                    if (d.Code.Count > 4)
                        d.Code.Dequeue();
                    d.Code.Enqueue('f');
                }

                ad.ChangeSpriteSheet("FeedDown", 0);
                if(!Globals.DigiPetAlive)
                {

                }
                else if (!d.NeedsFood && ad.SpriteRenderer.Animation < 3)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "RejectAnimate", 0, true);
                }
                else if (!d.Feeding)
                {
                    d.NeedsFood = false;
                    d.Feeding = true;
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(3, 0, ad), "FeedingAnimate", 0, true);
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
                        if (i < 20 && w.Transform.Position.X > -900)
                            w.SpriteRenderer.Animation = 1;
                        else if (i < 40 && w.Transform.Position.X < -720)
                            w.SpriteRenderer.Animation = 2;
                        else
                            w.SpriteRenderer.Animation = 0;
                    }

                    if (w.Transform.Position.X < -900 && w.SpriteRenderer.Animation == 2)
                    {
                        a.SpriteRenderer.Animation = 0;
                    }

                    if (w.Transform.Position.X > -720 && w.SpriteRenderer.Animation == 1)
                    {
                        a.SpriteRenderer.Animation = 0;
                    }
                }
            }
            else
            {
                d.TimeDead += gt;
                if (d.TimeDead > 40)
                    w.SpriteRenderer.Animation = 6;
                else if (d.TimeDead > 20)
                    w.SpriteRenderer.Animation = 5;

            }

            if (w.SpriteRenderer.Animation == 1)
                w.RigidBody.MoveVelocity = new Vector2(1, 0);
            else if (w.SpriteRenderer.Animation == 2)
                w.RigidBody.MoveVelocity = new Vector2(-1, 0);
            else
                w.RigidBody.MoveVelocity = new Vector2(0, 0);
        }

        public static void Animate(float gt, Component[] c)
        {
            ((AnimationData)c[0]).Animate(gt);
        }
    }
}
