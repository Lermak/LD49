using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Media;

namespace MonoGame_Core.Scripts
{
    public static class Behaviors
    {
        public static void ArrowControls(float gt, Component[] c)
        {
            KeyboardState state = Keyboard.GetState();
            Vector2 v = new Vector2();
            Movement m = (Movement)c[1];
            if (CurrentWindow.inputManager.IsKeyPressed(Keys.Up))
                v.Y = -m.Speed * gt;
            else if (CurrentWindow.inputManager.IsKeyPressed(Keys.Down))
                v.Y = m.Speed * gt;
            if (CurrentWindow.inputManager.IsKeyPressed(Keys.Left))
                v.X = -m.Speed * gt;
            else if (CurrentWindow.inputManager.IsKeyPressed(Keys.Right))
                v.X = m.Speed * gt;

            ((Transform)c[0]).Move(v);
        }

        public static void WASDcontrols(float gt, Component[] c)
        {
            Movement m = (Movement)c[1];

            KeyboardState state = Keyboard.GetState();
            Vector2 v = new Vector2();
            float r = 0;
            if (state.IsKeyDown(Keys.W))
                v.Y = -(m.Speed * gt);
            else if (state.IsKeyDown(Keys.S))
                v.Y = (m.Speed * gt);
            if (state.IsKeyDown(Keys.A))
                v.X = -(m.Speed * gt);
            else if (state.IsKeyDown(Keys.D))
                v.X = (m.Speed * gt);

            if (state.IsKeyDown(Keys.Q))
                r = (1 * gt);
            else if (state.IsKeyDown(Keys.E))
                r = -(1 * gt);

            RigidBody rb = (RigidBody)c[0];

            rb.MoveVelocity = v;
            rb.AngularVelocity = r;
        }

        public static void ManualScale(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            if (CurrentWindow.inputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Add) && t.Scale.X < 5)
            { t.SetScale(t.Scale.X + .1f, t.Scale.Y + .1f); }
            if (CurrentWindow.inputManager.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Subtract) && t.Scale.X > 0)
            { t.SetScale(t.Scale.X - .1f, t.Scale.Y - .1f); }

        }

        public static void ScreenShake(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];

            if (CurrentWindow.inputManager.IsKeyTriggered(Microsoft.Xna.Framework.Input.Keys.Space))
                CurrentWindow.coroutineManager.AddCoroutine(Coroutines.ScreenShake(.1f, -10, 10, t), "screenShake", 0, true);
        }

        public static void PointAtMouse(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            t.Radians = hf_Math.getAngle(CurrentWindow.inputManager.MousePos, t.Position) + 90 * (float)Math.PI / 180;
            if (t.Parent != null)
                t.Rotate(t.Radians - t.Parent.Radians);
        }

        public static void FaceTransform(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            Transform t2 = (Transform)c[1];
            RigidBody rb = (RigidBody)c[2];


            float newRot = (hf_Math.getAngle(t.Position - t2.Position, new Vector2(1, 0))) - 90 * (float)Math.PI / 180;
            rb.AngularVelocity = (newRot - t.Radians) / 1 * gt;
        }

        public static void MoveTowardRotation(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            RigidBody rb = (RigidBody)c[1];

            rb.MoveVelocity = hf_Math.RadiansToUnitVector(t.Radians + 90 * (float)Math.PI / 180) * gt * 100;
        }

        public static void ScreenShakeOnClick(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            Vector2 v = CurrentWindow.inputManager.MousePos;
            if (CurrentWindow.inputManager.IsMouseTriggered(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
                CurrentWindow.coroutineManager.AddCoroutine(Coroutines.ScreenShake(.1f, 10, 10, CameraManager.Cameras[0].Transform), "ClickShake", 0, true);
        }

        public static void ButtonSwapImagesOnHover(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            ButtonData b = (ButtonData)c[1];
            AnimationData ad = (AnimationData)c[2];
            Vector2 v = CurrentWindow.inputManager.MousePos;

            if (t.ContainsPoint(v))
                ad.ChangeSpriteSheet(b.SelectedTexID, 0);//((WorldObject)b.GameObject).SpriteRenderer.Texture = b.SelectedTexID;
            else
                ad.ChangeSpriteSheet(b.DeselectedTexID, 0);//((WorldObject)b.GameObject).SpriteRenderer.Texture = b.DeselectedTexID;
        }

        public static void ButtonSwapImagesOnClick(float gt, Component[] c)
        {
            if (!ChalkData.Held)
            {
                Transform t = (Transform)c[0];
                ButtonData b = (ButtonData)c[1];
                AnimationData ad = (AnimationData)c[2];
                Vector2 v = CurrentWindow.inputManager.MousePos;
                if (NuclearLevel.Locked)
                    return;

                if (t.ContainsPoint(v) && CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton))
                    ad.ChangeSpriteSheet(b.SelectedTexID, 0);//((WorldObject)b.GameObject).SpriteRenderer.Texture = b.SelectedTexID;
                else
                    ad.ChangeSpriteSheet(b.DeselectedTexID, 0);//((WorldObject)b.GameObject).SpriteRenderer.Texture = b.DeselectedTexID;
            }
        }

        public static void RunAnimation(float gt, Component[] c)
        {
            //if(SceneManager.SceneState == SceneManager.State.Running)
            ((AnimationData)c[0]).Animate(gt);
        }

        public static void IncreaseNuclearLevelOverTime(float gt, Component[] c)
        {
            if (Globals.FinalButtonPush) return;

            if (NuclearLevel.started && !Globals.FirstLockout)
            {
                if (NuclearLevel.buttonHit)
                {
                    NuclearLevel.currentStopTime += gt;
                    if (NuclearLevel.currentStopTime >= NuclearLevel.ButtonHitStopTime)
                    {
                        NuclearLevel.currentStopTime = 0;
                        NuclearLevel.buttonHit = false;
                    }

                    return;
                }

                NuclearLevel.level = NuclearLevel.level + gt * NuclearLevel.speed;
            }
        }
        public static void NuclearRotate(float gt, Component[] c)
        {
            Random r = new Random();
            Transform t = (Transform)c[0];
            //start at -135 degrees from straight up, then rotate to the right until +135 degrees
            float rot_start = MathHelper.ToRadians(70);
            float rot_end = MathHelper.ToRadians(-70);
            t.Radians = rot_start - MathHelper.Clamp(NuclearLevel.level, 0, 1) * (rot_start - rot_end);
            float intensity = MathHelper.Clamp((NuclearLevel.level - 0.5f) * 2, 0, 1);
            float intensityScale = 5f;
            if (intensity > 0f && NuclearLevel.level < 1.1)
            {
                SoundManager.PlaySoundEffect("alert");
                SoundManager.SoundEffects["alert"].Volume = intensity * 0.15f;
            }

            float r1 = intensity * intensityScale * ((float)r.NextDouble() - 0.5f);
            float r2 = intensity * intensityScale * ((float)r.NextDouble() - 0.5f);
            t.Place(new Vector2(r1, r2));
        }

        public static void NuclearDeath(float gt, Component[] c)
        {
            if (NuclearLevel.level >= 1.1 && !Globals.OverheatGameOver) {
                WindowManager.MainWindow.coroutineManager.AddCoroutine(Coroutines.GameOver((SpriteRenderer)c[0]), "GameOver", 0, true);
            }
            //GameManager.Quit(); //TODO() this is bad, make it into a game over screen
        }

        public static void isButtonHeld(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            Vector2 v = CurrentWindow.inputManager.MousePos;
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                NuclearLevel.ButtonHoldTime += gt;
            }
            else
            {
                if (NuclearLevel.ButtonHoldTime > 0)
                {
                    string code = "...----..-..";
                    if (NuclearLevel.MorseCode.Count > 11)
                        NuclearLevel.MorseCode.Dequeue();
                    if (NuclearLevel.ButtonHoldTime < .5f)
                        NuclearLevel.MorseCode.Enqueue('.');
                    else
                        NuclearLevel.MorseCode.Enqueue('-');
                    if (NuclearLevel.MorseCode.Count == 12)
                    {
                        bool flag = true;
                        List<char> str = NuclearLevel.MorseCode.ToList<char>();
                        for (int i = 0; i < 12; ++i)
                        {
                            if (str[i] != code[i])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            GameManager.plotManager.morse_code = true;
                            WindowManager.MainWindow.coroutineManager.AddCoroutine(Coroutines.PaylogDownload(), "PayLog", 0, true);
                            SoundManager.PlaySoundEffect("MysterySound");
                            SoundManager.SoundEffects["MysterySound"].Volume = .1f;
                            Globals.CreateFile("PayLog", "    STAFF SALARIES FY22\n---------------------------\n    Delores H - $ 90,000\n      Danni B - $ 75,000\n        Tim G - $ 40,000\n      Quinn R - $ 40,000\n     Kailee M - $ 40,000\nChristopher C - $ 40,000\n      Janey L - $ 40,000\n       Jude N - $ 150,000\n       Aida F - $ 40,000\n     Adrian B - $ 60,000\n     Gerald B - $ 60,000\n      Jamie Z - $ 60,000\n     New Hire - $ 40,000");
                        }

                    }
                }
                NuclearLevel.ButtonHoldTime = 0.0f;
            }
        }
        public static void UpdateNuclear(float gt, Component[] c)
        {
            if (NuclearLevel.NeedsUpdate)
            {
                NuclearLevel.Updating = true;
                AnimationData ad = (AnimationData)c[0];
                CurrentWindow.coroutineManager.AddCoroutine(Coroutines.UpdateNuclear(ad), "Updating", 0, true);
            }
        }
        public static void UpdateSpinner(float gt, Component[] c)
        {
            if (NuclearLevel.Updating)
            {
                SpriteRenderer s = (SpriteRenderer)c[0];
                s.Visible = true;
                s.Transform.Rotate(-gt);
            }
            else
            {
                SpriteRenderer s = (SpriteRenderer)c[0];
                s.Visible = false;
            }

        }

        public static void LockoutUpdate(float gt, Component[] c)
        {
            SpriteRenderer sd = (SpriteRenderer)c[0];
            if (NuclearLevel.Locked)
            {
                sd.Visible = true;
            }
            else
            {
                sd.Visible = false;
            }
        }

        public static void ChalkControlls(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            ChalkData cd = (ChalkData)c[1];
            SpriteRenderer sr = (SpriteRenderer)c[2];
            Transform box = (Transform)c[3];

            Vector2 v = CurrentWindow.inputManager.MousePos;
            if (CurrentWindow.inputManager.IsMouseTriggered(InputManager.MouseKeys.LeftButton))
            {
                if (t.ContainsPoint(v) && !ChalkData.Held)
                {
                    if (!cd.FirstPickup)
                    {
                        cd.FirstPickup = true;
                        WindowManager.MainWindow.coroutineManager.AddCoroutine(Coroutines.EndTimeMusic(), "EndTimeLoop", 0, true);//SoundManager.PlaySong("EndTimes");
                    }

                    sr.Texture = "UprightChalk";
                    ChalkData.Held = true;
                    cd.Draw = false;
                }
                else if (ChalkData.Held && box.ContainsPoint(v))
                {
                    cd.Draw = false;
                    t.Place(box.Position);
                    sr.Texture = "SideChalk";
                    ChalkData.Held = false;

                    if (CurrentWindow.sceneManager.CurrentScene.GameObjects.Where(t => t.Tag == "Dust").Count() > 5)
                    {
                        if(!Globals.ReadyForEndTimes)
                        {
                            Globals.ReadyForEndTimes = true;
                            GameManager.chatWindow.runChat("Stranger", "press_the_button", false);
                        }
                    }
                }
                else
                {
                    cd.Draw = true;
                }
            }
            bool down = CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton);
            if (down)
            {
                if (ChalkData.Held && Vector2.Distance(cd.LastDrawPos, v) > 6 && cd.Draw)
                {
                    cd.LastDrawPos = v;
                    CurrentWindow.sceneManager.CurrentScene.ToAdd.Add(new Dust(v));
                }

            }

            if (ChalkData.Held)
            {
                t.Place(v);
            }
        }
        public static void SpawnChalk(float gt, Component[] c)
        {
            if (Globals.CreateChalk && !Globals.PrepareForEndTimes)
            {
                WindowManager.MainWindow.sceneManager.CurrentScene.ToAdd.Add(new Chalk(new Vector2(-835, 415) + new Vector2(-100, -100)));
                Globals.PrepareForEndTimes = true;
            }
        }

        public static void FadeFinaleMusic(float gt, Component[] c)
        {
            if(Globals.FinalButtonPush)
            {
                if(MediaPlayer.Volume > 0)
                {
                    SoundManager.volume -= 1 * gt;
                }    
                else
                {
                    MediaPlayer.Stop();
                    SoundManager.volume = 1;
                }
            }
        }
        public static void ChangeOverlay(float gt, Component[] c)
        {
            SpriteRenderer bg = (SpriteRenderer)c[0];
            SpriteRenderer title = (SpriteRenderer)c[1];
            ButtonData button = (ButtonData)c[2];
            if (GameManager.plotManager.remove_overlay && bg.Texture != "EvilBackground")
            {
                bg.Texture = "EvilBackground";
                title.Texture = "SoulsTitle";
                title.SetDrawArea(106, 32);
                title.Transform.Resize(106, 32);
                button.DeselectedTexID = "EvilButton";
                button.SelectedTexID = "EvilButtonPress";
                SoundManager.PlaySong("OminousMusic");
                SoundManager.SetVolume(0.2f);
            }
        }


    }
}
