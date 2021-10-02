using Microsoft.Xna.Framework;
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
                    if (d.Code.Count > 5)
                        d.Code.Dequeue();
                    d.Code.Enqueue('p');
                }

                if (Globals.DigiPetAlive == false)
                {
                    ad.ChangeSpriteSheet("ButtonDown", 0);
                }
                else if (!d.NeedsPlay && ad.SpriteRenderer.Animation != 0)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "RejectAnimate", 0, true);
                    ad.ChangeSpriteSheet("ButtonDown", 0);
                }
                else if (!d.Playing)
                {
                    d.NeedsPlay = false;
                    d.Playing = true;
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "PlayingAnimate", 0, true);
                    ad.ChangeSpriteSheet("ButtonDown", 0);
                    d.TimeSinceLastPlay = 0;
                }
            }
            else
            {
                d.Playing = false;
                ad.ChangeSpriteSheet("ButtonUp", 0);
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
                    if (d.Code.Count > 5)
                        d.Code.Dequeue();
                    d.Code.Enqueue('w');
                }
                if (Globals.DigiPetAlive == false)
                {
                    ad.ChangeSpriteSheet("ButtonDown", 0);
                }
                else if (!d.NeedsWash && ad.SpriteRenderer.Animation != 0)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "RejectAnimate", 0, true);
                    ad.ChangeSpriteSheet("ButtonDown", 0);
                }
                else if (!d.Washing)
                {
                    d.NeedsWash = false;
                    d.Washing = true;
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "WashingAnimate", 0, true);
                    ad.ChangeSpriteSheet("ButtonDown", 0);
                    d.TimeSinceLastWash = 0;
                }
            }
            else
            {
                d.Washing = false;
                ad.ChangeSpriteSheet("ButtonUp", 0);
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
                    if (d.Code.Count > 5)
                        d.Code.Dequeue();
                    d.Code.Enqueue('f');
                }

                if (Globals.DigiPetAlive == false)
                {
                    ad.ChangeSpriteSheet("ButtonDown", 0);
                }
                else if (!d.NeedsFood && ad.SpriteRenderer.Animation != 0)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "RejectAnimate", 0, true);
                    ad.ChangeSpriteSheet("ButtonDown", 0);
                }
                else if (!d.Feeding)
                {
                    d.NeedsFood = false;
                    d.Feeding = true;
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "FeedingAnimate", 0, true);
                    ad.ChangeSpriteSheet("ButtonDown", 0);
                    d.TimeSinceLastFeed = 0;
                }
            }
            else
            {
                d.Feeding = false;
                ad.ChangeSpriteSheet("ButtonUp", 0);
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

            if(d.CodeAccessed == false && d.Code.ToList<char>() == new List<char>() { 'w','f','f','p','f' })
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
                if (d.CheckNeedsTimer > 5)
                {
                    d.CheckNeedsTimer = 0;
                    Random r = new Random();
                    int i = r.Next(0, 100);
                    if (i < 10)
                        d.NeedsFood = true;
                    else if (i < 20)
                        d.NeedsPlay = true;
                    else if (i < 30)
                        d.NeedsWash = true;
                }
            }

            if ((d.TimeSinceLastPlay > 15 ||
                d.TimeSinceLastFeed > 15 ||
                d.TimeSinceLastWash > 15) &&
                Globals.DigiPetAlive == true)
            {
                Globals.DigiPetAlive = false;              
                a.ChangeAnimation(1);
            }
        }

        public static void Animate(float gt, Component[] c)
        {
            ((AnimationData)c[0]).Animate(gt);
        }
    }
}
