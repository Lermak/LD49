using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
                if (e.KeyCode == Keys.A)
                    ShipData.aDown = false;
                if (e.KeyCode == Keys.D)
                    ShipData.dDown = false;
                if (e.KeyCode == Keys.Space)
                    ShipData.SpaceDown = false;
            };
            WindowManager.GalaxyBlasterWindow.form.KeyPress += (object sender, KeyPressEventArgs e) =>
            {
                if (e.KeyChar == 'a' || e.KeyChar == 'A')
                    ShipData.aDown = true;
                if (e.KeyChar == 'd' || e.KeyChar == 'D')
                    ShipData.dDown = true;
                if (e.KeyChar == ' ')
                    ShipData.SpaceDown = true;
            };
            ResourceManager.Textures["Enemy"] = Content.Load<Texture2D>("Images/Enemy");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");
            ResourceManager.SoundEffects["Error"] = Content.Load<SoundEffect>(@"Sound/error");

            ResourceManager.Textures["PeaShooter"] = Content.Load<Texture2D>("Images/PeaShooter");
            ResourceManager.Textures["Bullet"] = Content.Load<Texture2D>("Images/Bullet");

            GameObjects.Add(new Player());
            Player p = (Player)GameObjects[^1];

            GameObjects.Add(new GameObject("Score"));
            GameObject g = GameObjects[^1];
            Transform t = (Transform)g.ComponentHandler.AddComponent(new Transform(g, new Vector2(-660, 490), 600, 100, 0, 5));
            FontRenderer fr = (FontRenderer)g.ComponentHandler.AddComponent(new FontRenderer(g, "Score: 0", "TestFont", t, new Vector2(), new Vector2(600, 100), 5, Color.White));
            g.BehaviorHandler.AddBehavior("ShowScore", GalaxyBlasterBehaviors.ShowScore, new Component[] { fr });
            SpawnHorde();
            base.loadContent(c);
        }

        public void SpawnHorde()
        {
            for (int y = 0; y < 3; ++y)
                for (int x = 0; x < 8; ++x)
                {
                    ToAdd.Add(new Enemy(new Vector2(-860 + x * 60, 450 - y * 60)));
                }
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
