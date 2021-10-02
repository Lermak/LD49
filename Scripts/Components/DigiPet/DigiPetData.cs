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
        public float CheckNeedsTimer = 0;
        public bool Feeding = false;
        public bool Playing = false;
        public bool Washing = false;
        public bool NeedsFood = false;
        public bool NeedsPlay = false;
        public bool NeedsWash = false;
        public DigiPetData(GameObject go, string name) : base(go, name)
        {

        }
    }
}
