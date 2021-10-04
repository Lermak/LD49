using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    /// <summary>
    /// Class to manage the run cycles of coroutines
    /// </summary>
    public class CoroutineManager
    {
        /// <summary>
        /// Determines if a coroutine should progress or wait
        /// </summary>
        public enum CoroutineState { Paused, Running }


        public class Coroutine
        {
            public string Name;
            public CoroutineState State;
            public float TimeSinceLast;
            public float TimeBetweenSteps;
            public Stack<IEnumerator> Routines = new Stack<IEnumerator>();

            /// <summary>
            /// Create a new coroutine
            /// </summary>
            /// <param name="routine">The enumerator function to iterate through</param>
            /// <param name="name">The coroutine name</param>
            /// <param name="timeBetween">Delay in seconds between iterations</param>
            /// <param name="start">Start immediately</param>
            public Coroutine(IEnumerator routine, string name, float timeBetween, bool start)
           {
                if (start)
                    State = CoroutineState.Running;
                else
                    State = CoroutineState.Paused;
                
              Routines.Push(routine);
              Name = name;
               TimeBetweenSteps = timeBetween;
                TimeSinceLast = 0;
            }
        }

        List<string> keys = new List<string>();
        Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();
        bool doingCoroutines = false;

        /// <summary>
        /// Remove all coroutines from the list
        /// </summary>
        public void Clear()
        {
            coroutines.Clear();
        }

        public void AddCoroutine(IEnumerator coroutine, string name, float timeBetween, bool start)
        {
            if (!coroutines.ContainsKey(name))
            {
               coroutines[name] = new Coroutine(coroutine, name, timeBetween, start);
            }
        }

        /// <summary>
        /// Check if a coroutine is currently running
        /// </summary>
        /// <param name="coroutine">The name of the coroutine</param>
        /// <returns>true if the coroutine' CoroutineStat is Running</returns>
        public bool IsRunning(string coroutine)
        {
            return coroutines.ContainsKey(coroutine) && coroutines[coroutine].State == CoroutineState.Running;
        }

        /// <summary>
        /// Changes the named coroutine's CoroutineState to Paused
        /// </summary>
        /// <param name="coroutine">The coroutine's name</param>
        public void Pause(string coroutine)
        {
            if (coroutines.ContainsKey(coroutine))
            {
                Coroutine c = coroutines[coroutine];
                c.State = CoroutineState.Paused;
                coroutines[coroutine] = c;
            }
        }

        /// <summary>
        /// Changes the named coroutine's CoroutineState to Running
        /// </summary>
        /// <param name="coroutine">The coroutine's name</param>
        public void Start(string coroutine)
        {
            if (coroutines.ContainsKey(coroutine))
            {
                Coroutine c = coroutines[coroutine];

                c.State = CoroutineState.Running;

                coroutines[coroutine] = c;
            }
        }

        /// <summary>
        /// Removes a coroutine from the list of coroutines
        /// </summary>
        /// <param name="coroutine">The coroutine's name</param>
        /// 

        List<string> toRemove = new List<string>();
        public void Stop(string coroutine)
        {
            if (coroutines.ContainsKey(coroutine))
            {
                if (doingCoroutines)
                {
                    toRemove.Add(coroutine);
                }
                else
                {
                    coroutines.Remove(coroutine);
                }
            }
        }

        public CoroutineManager()
        {
            coroutines = new Dictionary<string, Coroutine>();
        }

        /// <summary>
        /// Iterate through all coroutines that are currently running by one loop, provided there has been enough delay
        /// If a coroutine is finished, remove it from the list
        /// </summary>
        /// <param name="gt">Game Time</param>
        public void Update(float gt)
        {
            List<string> k = new List<string>();
            k.AddRange(coroutines.Keys);
            doingCoroutines = true;

            for (int i = 0; i < k.Count; ++i)
            {
                Coroutine c = coroutines[k[i]];
                
                foreach(string name in toRemove)
                {
                    if(c.Name == name)
                    {
                        continue;
                    }
                }

                if (c.State == CoroutineState.Running)
                {
                    c.TimeSinceLast += gt;
                    if (c.TimeSinceLast > c.TimeBetweenSteps)
                    {
                        c.Routines.Peek().MoveNext();
                        if (c.Routines.Peek().Current is bool status)
                        {
                            if(status == true)
                            {
                                c.Routines.Pop();
                                if (c.Routines.Count == 0)
                                {
                                    toRemove.Add(c.Name);
                                }
                            }
                        }
                        else if (c.Routines.Peek().Current is IEnumerator enumerator)
                        {
                            c.Routines.Push(enumerator);
                        }
                    }
                }

                coroutines[k[i]] = c;
            }
            doingCoroutines = false;

            for (int i = 0; i < toRemove.Count; ++i)
            {
                coroutines.Remove(toRemove[i]);
            }
            toRemove.Clear();
        }
    }
}
