using Microsoft.Xna.Framework;

namespace MonoGame_Core.Scripts
{
    public class NuclearDial : WorldObject
    {
        public static int dialSize = 128;
        public NuclearDial(string texID, string tag) : base(texID, tag, new Vector2(dialSize, dialSize), new Vector2(0, 0), 1) {

            BehaviorHandler.AddBehavior("increasingNuclear", Behaviors.IncreaseNuclearLevelOverTime, new Component[] {  });
            BehaviorHandler.AddBehavior("NuclearRotate", Behaviors.NuclearRotate, new Component[] { Transform });
            BehaviorHandler.AddBehavior("NuclearDeath", Behaviors.NuclearDeath, new Component[] { });
        }

    }
}
