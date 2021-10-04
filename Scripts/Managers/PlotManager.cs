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

        public bool christopher_strange = false;
        IEnumerator christopher_strangeCo = null;

        public bool digipet_initial = false;
        IEnumerator digipet_initialCo = null;

        public bool morse_code = false;
        IEnumerator christopher_morsecode = null;

        bool jude_money = false;

        bool supervisor_angry = false;
        IEnumerator supervisor_angryCo = null;

        bool meet_stranger = false;
        IEnumerator meetStrangerCo = null;

        public bool digipet_secret = false;

        IEnumerator christopher_aida = null;

        public bool remove_overlay = false;

        IEnumerator christopher_oldone = null;
        IEnumerator stranger_endgame = null;
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

            if(christopher_strange == false && christopher_strangeCo == null)
            {
                christopher_strangeCo = ChritopherStrangeCo();
                coroutines.AddCoroutine(christopher_strangeCo, "christopher_strangeCo", 0, true);
            }

            if (digipet_initial == false && digipet_initialCo == null)
            {
                digipet_initialCo = DigipetInitialCo();
                coroutines.AddCoroutine(digipet_initialCo, "digipet_initialCo", 0, true);
            }

            if(digipet_initial && WindowManager.DigiPetWindow == null)
            {
                WindowManager.AddWindow(new NoCloseForm(), "DigiPetWindow", new DigiPetScene(), new Vector2(480, 330));
            }

            if(morse_code == false && christopher_morsecode == null)
            {
                christopher_morsecode = ChritopherMorseCodeCo();
                coroutines.AddCoroutine(christopher_morsecode, "christopher_morsecode", 0, true);
            }

            if(morse_code)
            {
                coroutines.Stop("christopher_morsecode");
                christopher_morsecode = null;
            }

            if(jude_money && supervisor_angry == false && supervisor_angryCo == null)
            {
                supervisor_angryCo = SupervisorAngryCo();
                coroutines.AddCoroutine(supervisor_angryCo, "supervisor_angryCo", 0, true);
            }

            if(supervisor_angry && meet_stranger == false && meetStrangerCo == null)
            {
                meetStrangerCo = MeetStrangerCo();
                coroutines.AddCoroutine(meetStrangerCo, "meetStrangerCo", 0, true);
            }

            if(meet_stranger)
            {
                if(christopher_aida == null)
                {
                    christopher_aida = ChritopherAidaCo();
                    coroutines.AddCoroutine(christopher_aida, "christopher_aida", 0, true);
                }
            }

            if(remove_overlay)
            {
                if (christopher_oldone == null)
                {
                    christopher_oldone = ChritopherOldOneCo();
                    coroutines.AddCoroutine(christopher_oldone, "christopher_oldone", 0, true);
                }


                if (stranger_endgame == null)
                {
                    stranger_endgame = StrangerEndgameCo();
                    coroutines.AddCoroutine(stranger_endgame, "stranger_endgame", 0, true);
                }
            }
        }

        public void SendEvent(string ev)
        {
            if (ev == "Delores_intro_chat")
            {
                deloresChat = true;
            }

            if (ev == "Christopher_security_check")
            {
                christopher_strange = true;
            }

            if (ev == "Kailee_digipal_chat")
            {
                digipet_initial = true;
            }

            if (ev == "jude_money")
            {
                jude_money = true;
            }

            if (ev == "Delores_salary_chat")
            {
                supervisor_angry = true;
            }

            if (ev == "met_stranger")
            {
                meet_stranger = true;
            }

            if (ev == "remove_overlay")
            {
                remove_overlay = true;
            }

            if (ev == "Stranger_great_old_one")
            {
                //Globals.
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

            GameManager.chatWindow.runChat("Kailee", "digipal_chat", false);

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
                int value = r.Next(0, 10);

                if (value < 3 && WindowManager.ResetKeysWindow == null)
                {
                    WindowManager.AddWindow(new NoCloseForm(), "ResetKeysWindow", new ResetKeysScene(), new Vector2(600, 200));
                    found = true;
                }
                else if (value < 6 && WindowManager.SecurityCheckWindow == null)
                {
                    WindowManager.AddWindow(new NoCloseForm(), "SecurityCheckWindow", new SecurityCheckScene(), new Vector2(600, 240));
                    found = true;
                }
                else if (value < 7 && WindowManager.ITHelp == null)
                {
                    WindowManager.AddWindow(new NoCloseForm(), "ITHelp", new AskITScene(), new Vector2(600, 200));
                    found = true;
                }
                else if (value < 10 && WindowManager.UpdateWindow == null)
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

        public IEnumerator ChritopherMorseCodeCo()
        {
            yield return Coroutines.WaitTime(13);

            Random r = new Random();

            float percent = 0.1f;
            while (true)
            {
                if (percent >= r.NextDouble())
                {
                    break;
                }
                percent += 0.02f;
                yield return Coroutines.WaitTime(6);
            }

            GameManager.chatWindow.runChat("Christopher", "morse_code", false);

            yield return true;
        }

        public IEnumerator SupervisorAngryCo()
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
                percent += 0.02f;
                yield return Coroutines.WaitTime(6);
            }

            GameManager.chatWindow.runChat("Delores", "salary_chat", false);

            yield return true;
        }

        public IEnumerator MeetStrangerCo()
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
                percent += 0.02f;
                yield return Coroutines.WaitTime(6);
            }

            GameManager.chatWindow.sendEvent("meet_stranger");

            yield return true;
        }

        public IEnumerator ChritopherAidaCo()
        {
            yield return Coroutines.WaitTime(13);

            Random r = new Random();

            float percent = 0.1f;
            while (true)
            {
                if (percent >= r.NextDouble())
                {
                    break;
                }
                percent += 0.02f;
                yield return Coroutines.WaitTime(6);
            }

            GameManager.chatWindow.runChat("Christopher", "coworker_bot", false);

            yield return true;
        }

        public IEnumerator ChritopherOldOneCo()
        {
            yield return Coroutines.WaitTime(13);

            Random r = new Random();

            float percent = 0.1f;
            while (true)
            {
                if (percent >= r.NextDouble())
                {
                    break;
                }
                percent += 0.02f;
                yield return Coroutines.WaitTime(6);
            }

            GameManager.chatWindow.runChat("Christopher", "great_old_one", false);

            yield return true;
        }

        public IEnumerator StrangerEndgameCo()
        {
            yield return Coroutines.WaitTime(13);

            Random r = new Random();

            float percent = 0.1f;
            while (true)
            {
                if (percent >= r.NextDouble())
                {
                    break;
                }
                percent += 0.02f;
                yield return Coroutines.WaitTime(6);
            }

            GameManager.chatWindow.runChat("Stranger", "great_old_one", false);

            yield return true;
        }


        
    }
}
