using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class Button : WorldObject
    {
        public delegate void OnClickAction();

        public Button(string deselectedTex, string selectedTex, string tag, Vector2 size, Vector2 pos, byte layer, OnClickAction onClick) : base(deselectedTex, tag, size, pos, layer)
        {
            SpriteRenderer.IsHUD = true;
            ButtonData b = (ButtonData)componentHandler.AddComponent(new ButtonData(this, "ButtonData", selectedTex, deselectedTex));
            Component ad = componentHandler.AddComponent(new AnimationData(this, "AnimationData", SpriteRenderer, 0));
            behaviorHandler.AddBehavior("Animate", Behaviors.RunAnimation, new Component[] { ad });
            behaviorHandler.AddBehavior("Hover", Behaviors.ButtonSwapImagesOnHover, new Component[] { Transform, b, ad });
            if (onClick != null)
            {
                behaviorHandler.AddBehavior("OnClick", (float gt, Component[] c) =>
                {
                    Transform t = (Transform)c[0];
                    Vector2 v = CurrentWindow.inputManager.MousePos;
                    if (CurrentWindow.inputManager.IsMouseTriggered(InputManager.MouseKeys.LeftButton) && t.ContainsPoint(v))
                    {
                        onClick();
                    }
                }, new Component[] { Transform });
            }
        }
    }
}
