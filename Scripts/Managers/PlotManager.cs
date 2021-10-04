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
        }

        public void SendEvent(string ev)
        {
            if(supervisorTutotial == false && ev == "Delores_intro_chat")
            {
                supervisorTutotial = true;
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
            yield return Coroutines.WaitTime(1);
            while (firstLockout == false) yield return false;
            yield return true;
        }
    }
}
