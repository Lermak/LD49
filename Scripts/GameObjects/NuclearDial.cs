using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoGame_Core.Scripts
{
    public class NuclearDial : WorldObject
    {
        public NuclearDial(string texID, string tag, Vector2 size, Vector2 pos, byte layer) : base(texID, tag, size, pos, layer) {

            Transform.Radians = MathHelper.ToRadians(135);

            BehaviorHandler.AddBehavior("increasingNuclear", Behaviors.IncreaseNuclearLevelOverTime, new Component[] {  });
            BehaviorHandler.AddBehavior("NuclearRotate", Behaviors.NuclearRotate, new Component[] { Transform });
            BehaviorHandler.AddBehavior("NuclearDeath", Behaviors.NuclearDeath, new Component[] { });
        }

    }
}
