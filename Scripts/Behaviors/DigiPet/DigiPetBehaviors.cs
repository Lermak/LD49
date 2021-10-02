using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
                if (Globals.DigiPetAlive == true)
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
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "RejectAnimate", 0, true);
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
                if (Globals.DigiPetAlive == true)
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
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.RunAnimation(0, 0, ad), "RejectAnimate", 0, true);
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
                if (Globals.DigiPetAlive == true)
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
            if(d.NeedsFood)
                d.TimeSinceLastFeed += gt;
            if(d.NeedsPlay)
                d.TimeSinceLastPlay += gt;
            if(d.NeedsWash)
                d.TimeSinceLastWash += gt;

            if(!d.NeedsFood && !d.NeedsPlay && !d.NeedsWash)
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
                AnimationData a = ((AnimationData)c[1]);
                a.ChangeAnimation(1);
            }
        }

        public static void Animate(float gt, Component[] c)
        {
            ((AnimationData)c[0]).Animate(gt);
        }
    }
}
