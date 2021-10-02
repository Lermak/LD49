using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class PlayButton : Button
    {
        public PlayButton(string deselectedTex, string selectedTex, string tag, Vector2 size, Vector2 pos, byte layer, DigiPetData data, AnimationData dpAnimData) : base(deselectedTex, selectedTex, tag, size, pos, layer, null)
        {

            behaviorHandler.AddBehavior("OnClick", DigiPetBehaviors.Play, new Component[] { Transform, data, componentHandler.GetComponent("AnimationData"), dpAnimData });

        }
    }
}
