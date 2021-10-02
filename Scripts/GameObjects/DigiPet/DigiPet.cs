using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGame_Core.Scripts
{
    public class DigiPet : WorldObject
    {
        public DigiPet(SceneManager sm, List<Camera> cam, string texID, string tag, Vector2 size, Vector2 pos, byte layer) : base(sm, cam, texID, tag, size, pos, layer)
        {
            Component c = componentHandler.AddComponent(new DigiPetData(this, "DigiPetData"));
            Component anim = componentHandler.AddComponent(new AnimationData(this, "AnimationData", SpriteRenderer, 2));

            behaviorHandler.AddBehavior("Animate", DigiPetBehaviors.Animate, new Component[] { anim });
            behaviorHandler.AddBehavior("RunDigiPet", DigiPetBehaviors.Running, new Component[] { c, anim });
        }
    }
}
