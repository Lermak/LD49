using Microsoft.Xna.Framework;

namespace MonoGame_Core.Scripts
{
    public class EmptyObject : GameObject
    {
        public EmptyObject(SceneManager sm, string tag) : base(tag, sm)
        {

        }

        public override void Initilize()
        {
            base.Initilize();
        }

        public override void Update(float gt)
        {
            base.Update(gt);
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}
