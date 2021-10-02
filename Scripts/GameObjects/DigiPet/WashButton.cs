﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class WashButton : Button
    {
        public WashButton(SceneManager sm, List<Camera> cam, string deselectedTex, string selectedTex, string tag, Vector2 size, Vector2 pos, byte layer, DigiPetData data, AnimationData dpAnimData) : base(sm, cam, deselectedTex, selectedTex, tag, size, pos, layer, null)
        {

            behaviorHandler.AddBehavior("OnClick", DigiPetBehaviors.Wash, new Component[] { Transform, data, componentHandler.GetComponent("AnimationData"), dpAnimData });

        }
    }
}
