﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class Button : WorldObject
    {
        public Button(string deselectedTex, string selectedTex, string tag, Vector2 size, Vector2 pos, byte layer, BehaviorHandler.Act onClick) : base(deselectedTex, tag, size, pos, layer)
        {
            SpriteRenderer.IsHUD = true;
            ButtonData b = (ButtonData)componentHandler.AddComponent(new ButtonData(this, 0, "ButtonData", selectedTex, deselectedTex));
            Component ad = componentHandler.AddComponent(new AnimationData(this, 0, "AnimationData", SpriteRenderer, 0));
            behaviorHandler.AddBehavior("Animate", Behaviors.RunAnimation, new Component[] { ad });
            behaviorHandler.AddBehavior("Hover", Behaviors.ButtonSwapImagesOnHover, new Component[] { Transform, b, ad });
            if(onClick != null)
                behaviorHandler.AddBehavior("OnClick", onClick, new Component[] { Transform });
        }
    }
}
