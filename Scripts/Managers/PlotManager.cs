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
        bool supervisorTutotial = false;
        IEnumerator<bool> supervisorCo = null;

        bool firstLockout = false;
        IEnumerator firstLockoutCo = null;

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

            if(christopher_strangeCo == null)
            {
                christopher_strangeCo = ChritopherStrangeCo();
                coroutines.AddCoroutine(christopher_strangeCo, "christopher_strangeCo", 0, true);
            }

            if (digipet_initialCo == null)
            {
                digipet_initialCo = DigipenInitialCo();
                coroutines.AddCoroutine(digipet_initialCo, "digipet_initialCo", 0, true);
            }
        }

        public void SendEvent(string ev)
        {
            if(supervisorTutotial == false && ev == "Delores_intro_chat")
            {
                supervisorTutotial = true;
            }

            if (ev == "Christopher_security_check")
            {
                christopher_strange = true;
            }

            if (ev == "Kailee_digipet_initial")
            {
                digipet_initial = true;
            }
        }


        public IEnumerator<bool> SupervisorCo()
        {
            GameManager.chatWindow.runChat("Delores", "intro_chat", true);
            while(supervisorTutotial == false) yield return false;
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
            while(false)
            {
                if(percent >= r.NextDouble())
                {
                    break;
                }
                percent += 0.1f;
                Coroutines.WaitTime(5);
            }

            GameManager.chatWindow.runChat("Christopher", "security_check", false);

            yield return true;
        }

        public IEnumerator DigipenInitialCo()
        {
            yield return Coroutines.WaitTime(10);

            Random r = new Random();

            float percent = 0.5f;
            while (false)
            {
                if (percent >= r.NextDouble())
                {
                    break;
                }
                percent += 0.1f;
                Coroutines.WaitTime(5);
            }

            GameManager.chatWindow.runChat("Kailee", "digipet_initial", false);

            yield return true;
        }
    }
}
