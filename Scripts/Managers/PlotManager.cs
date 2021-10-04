using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts.Managers
{

    public class PlotManager
    {
        CoroutineManager coroutines = new CoroutineManager();

        // Plot flags (false is uncompleted, true is completed)
        bool deloresChat = false;
        bool supervisorTutotial = false;
        IEnumerator<bool> supervisorCo = null;

        bool firstLockout = false;
        IEnumerator firstLockoutCo = null;

        IEnumerator generalLockOutCo = null;

        bool christopher_strange = false;
        IEnumerator christopher_strangeCo = null;

        bool digipet_initial = false;
        IEnumerator digipet_initialCo = null;
        //

        public PlotManager()
        {

        }

        public void Update(float dt)
        {
            coroutines.Update(dt);

            //We always need to do the supervisor tutorial
            if (supervisorTutotial == false) {
                if (supervisorCo == null && GameManager.chatWindow.ready)
                {
                    supervisorCo = SupervisorCo();
                    coroutines.AddCoroutine(supervisorCo, "SupervisorCo", 0, true);
                }
                return;
            }

            //Then, we always need to do the first lockout
            if (firstLockout == false)
            {
                if (firstLockoutCo == null)
                {
                    firstLockoutCo = FirstLockoutCo();
                    coroutines.AddCoroutine(firstLockoutCo, "firstLockoutCo", 0, true);
                }
                return;
            }

            if(generalLockOutCo == null)
            {
                generalLockOutCo = GeneralLockOutCo();
                coroutines.AddCoroutine(generalLockOutCo, "generalLockOutCo", 0, true);
            }

            if(christopher_strangeCo == null)
            {
                christopher_strangeCo = ChritopherStrangeCo();
                coroutines.AddCoroutine(christopher_strangeCo, "christopher_strangeCo", 0, true);
            }

            if (digipet_initialCo == null)
            {
                digipet_initialCo = DigipetInitialCo();
                coroutines.AddCoroutine(digipet_initialCo, "digipet_initialCo", 0, true);
            }
        }

        public void SendEvent(string ev)
        {
            if(ev == "Delores_intro_chat")
            {
                deloresChat = true;
            }

            if (ev == "Christopher_security_check")
            {
                christopher_strange = true;
            }

            if (ev == "Kailee_digipet_chat")
            {
                digipet_initial = true;
            }
        }


        public IEnumerator<bool> SupervisorCo()
        {
            GameManager.chatWindow.runChat("Delores", "intro_chat", true);
            while(deloresChat == false || NuclearLevel.started == false) yield return false;
            supervisorTutotial = true;
            yield return true;
        }

        public IEnumerator FirstLockoutCo()
        {
            yield return Coroutines.WaitTime(7);

            WindowManager.AddWindow(new NoCloseForm(), "SecruityCheckScene", new SecurityCheckScene(), new Vector2(600, 240));

            while (NuclearLevel.Locked == true) yield return false;
            firstLockout = true;

            yield return true;
        }

        public IEnumerator ChritopherStrangeCo()
        {
            yield return Coroutines.WaitTime(7);

            Random r = new Random();

            float percent = 0.1f;
            while(true)
            {
                if(percent >= r.NextDouble())
                {
                    break;
                }
                percent += 0.05f;
                yield return Coroutines.WaitTime(5);
            }

            GameManager.chatWindow.runChat("Christopher", "security_check", false);

            yield return true;
        }

        public IEnumerator DigipetInitialCo()
        {
            yield return Coroutines.WaitTime(10);

            Random r = new Random();

            float percent = 0.1f;
            while (true)
            {
                if (percent >= r.NextDouble())
                {
                    break;
                }
                percent += 0.05f;
                yield return Coroutines.WaitTime(5);
            }

            GameManager.chatWindow.runChat("Kailee", "digipet_chat", false);

            yield return true;
        }

        public void SpawnRandomLockOut()
        {
            if(WindowManager.ResetKeysWindow != null ||
               WindowManager.SecurityCheckWindow != null ||
               WindowManager.ITHelp != null ||
               WindowManager.UpdateWindow != null)
            {
                return;
            }

            bool found = false;
            Random r = new Random();

            int iter = 0;
            while (iter < 100 && !found)
            {
                ++iter;
                int value = r.Next(0, 4);

                if (value == 0 && WindowManager.ResetKeysWindow == null)
                {
                    WindowManager.AddWindow(new NoCloseForm(), "ResetKeysWindow", new ResetKeysScene(), new Vector2(600, 200));
                    found = true;
                }
                else if (value == 1 && WindowManager.SecurityCheckWindow == null)
                {
                    WindowManager.AddWindow(new NoCloseForm(), "SecurityCheckWindow", new SecurityCheckScene(), new Vector2(600, 240));
                    found = true;
                }
                else if (value == 2 && WindowManager.ITHelp == null)
                {
                    WindowManager.AddWindow(new NoCloseForm(), "ITHelp", new AskITScene(), new Vector2(600, 200));
                    found = true;
                }
                else if (value == 3 && WindowManager.UpdateWindow == null)
                {
                    WindowManager.AddWindow(new NoCloseForm(), "UpdateWindow", new UpdateRequiredScene(), new Vector2(600, 200));
                    found = true;
                }
            }
        }

        public IEnumerator GeneralLockOutCo()
        {
            yield return Coroutines.WaitTime(3);

            float percent = 0.1f;

            Random r = new Random();
            while (true)
            {
                if (percent >= r.NextDouble())
                {
                    SpawnRandomLockOut();
                    percent = 0;
                    continue;
                }
                percent += 0.05f;
                yield return Coroutines.WaitTime(5);
            }
        }
    }
}
