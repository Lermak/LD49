using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class DigiPetData : Component
    {
        public float TimeSinceLastFeed = 0;
        public float TimeSinceLastPlay = 0;
        public float TimeSinceLastWash = 0;
        public float CheckNeedsTimer = 4;
        public float AnimationDuration = 1;
        public float TimeSinceAnimation = 0;
        public float TimeDead = 0;
        public bool Feeding = false;
        public bool Playing = false;
        public bool Washing = false;
        public bool NeedsFood = false;
        public bool NeedsPlay = false;
        public bool NeedsWash = false;
        public Queue<char> Code = new Queue<char>();
        public bool CodeAccessed = false;
        public int PrevWalkSound = 0;
        public float TimeSinceLastSound = 0;
        public AnimationData Needs;
        public DigiPetData(GameObject go, string name) : base(go, name)
        {

        }
    }
}
