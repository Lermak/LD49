using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{ 
    public class Chalk : WorldObject
    {
        public Chalk(Vector2 pos) : base("SideChalk", "Chalk", new Vector2(80,80), pos, 10)
        {
            CurrentWindow.sceneManager.CurrentScene.ToAdd.Add(new WorldObject("ChalkBox", "ChalkBox", new Vector2(60,30), pos, 5));
            WorldObject wo = (WorldObject)CurrentWindow.sceneManager.CurrentScene.ToAdd[^1];
            ChalkData cd = (ChalkData)componentHandler.AddComponent(new ChalkData(this, "ChalkData"));
            behaviorHandler.AddBehavior("ChalkControlls", Behaviors.ChalkControlls, new Component[] { Transform, cd, SpriteRenderer, wo.Transform });
        }
    }

    public class Dust : WorldObject
    {
        public Dust(Vector2 pos) : base("Dust", "Dust", new Vector2(20,20), pos, 6)
        {

        }
    }
}
