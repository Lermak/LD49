 using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGame_Core.Scripts
{
    public class TestStaticObject : WorldObject
    {
        public TestStaticObject(SceneManager sm, List<Camera> cam, string texID, byte layer) : base(sm, cam, texID, "StaticTest", new Vector2(40,40), new Vector2(100, 100), layer)
        {
            ComponentHandler.AddComponent(new CollisionBox(this, "myBox", true));
            //SpriteRenderer.Shader = "BlueShader";
        }
    }
}
