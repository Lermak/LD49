using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MonoGame_Core.Scripts
{
    public class GalaxyBlasterScene : Scene
    {
        protected override void loadContent(List<Camera> c)
        {
            WindowManager.GalaxyBlasterWindow.form.KeyUp += (object sender, KeyEventArgs e) =>
            {
                if(e.KeyCode == Keys.A)
                    ShipData.aDown = false;
                if (e.KeyCode == Keys.D)
                    ShipData.dDown = false;
                if (e.KeyCode == Keys.Space)
                    ShipData.SpaceDown = false;
            };
            WindowManager.GalaxyBlasterWindow.form.KeyPress += (object sender, KeyPressEventArgs e) =>
            {
                if (e.KeyChar == 'a')
                    ShipData.aDown = true;
                if (e.KeyChar == 'd')
                    ShipData.dDown = true;
                if (e.KeyChar == ' ')
                    ShipData.SpaceDown = true;
            };
            ResourceManager.Textures["Enemy"] = Content.Load<Texture2D>("Images/Enemy");

            ResourceManager.Textures["PeaShooter"] = Content.Load<Texture2D>("Images/PeaShooter");
            ResourceManager.Textures["Bullet"] = Content.Load<Texture2D>("Images/Bullet");

            GameObjects.Add(new Player());
            Player p = (Player)GameObjects[^1];
            GameObjects.Add(new Enemy(new Vector2(-840, 400)));


            base.loadContent(c);
        }

        public override void Update(float gt)
        {
            if (WindowManager.GalaxyBlasterWindow.form.Focused)
                sceneManager.SceneState = SceneManager.State.Running;
            else
                sceneManager.SceneState = SceneManager.State.Paused;

            if (sceneManager.SceneState == SceneManager.State.Running)
                SceneRunning(gt);
        }
    }
}
