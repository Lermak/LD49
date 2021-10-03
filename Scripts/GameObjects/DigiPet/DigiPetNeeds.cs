using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGame_Core.Scripts
{
    public class DigiPetNeeds : WorldObject
    {
        public DigiPetNeeds(string texID, string tag, Vector2 size, Vector2 pos, byte layer) : base(texID, tag, size, pos, layer)
        {
            Component c = componentHandler.AddComponent(new DigiPetData(this, "DigiPetData"));
            Component anim = componentHandler.AddComponent(new AnimationData(this, "AnimationData", SpriteRenderer, .6f));
        }
    }
}
