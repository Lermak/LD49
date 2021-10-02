using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MonoGame_Core.Scripts
{
    public class WorldObject : GameObject
    {
        public RigidBody RigidBody { get { return (RigidBody)componentHandler.GetComponent("rigidBody"); } }
        public Transform Transform { get { return (Transform)componentHandler.GetComponent("transform"); } }
        public SpriteRenderer SpriteRenderer{ get { return (SpriteRenderer)componentHandler.GetComponent("spriteRenderer"); } }
        public CollisionHandler CollisionHandler { get { return (CollisionHandler)componentHandler.GetComponent("collisionHandler"); } }
        public WorldObject(SceneManager sm, List<Camera> cam, string texID, string tag, Vector2 size, Vector2 pos, byte layer) : base(tag, sm)
        {           
            componentHandler.AddComponent(new CollisionHandler(this));           
            componentHandler.AddComponent(new Transform(this, pos, size.X, size.Y, 0, layer));
            componentHandler.AddComponent(new RigidBody(this, RigidBody.RigidBodyType.Static));
            componentHandler.AddComponent(new SpriteRenderer(this, 
                                            cam,
                                            texID,
                                            Transform,
                                            new Vector2(0, 0),
                                            size,
                                            0));
        }
    }
}
