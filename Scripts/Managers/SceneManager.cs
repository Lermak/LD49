using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_Core.Scripts
{
    /// <summary>
    /// Manageages the current scene, scene transitions and the state of the scene
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// Determines how to handle the game loop based on what the scene is performing
        /// </summary>
        public enum State { Running, Paused, SceneOut, SceneIn };
        public Scene CurrentScene = new TestScene();
        public Scene NextScene = null;
        public State SceneState;
        public List<Camera> Cameras;
        static ContentManager cm;
        
        public void Initilize(ContentManager c, Scene s, List<Camera> cam)
        {
            Cameras = cam;
            SceneState = State.SceneIn;
            cm = c;
            CurrentScene = s;
            InitilizeCurrentScene();
            CurrentScene.OnLoad();
        }   

        public void ChangeScene(Scene s)
        {
            SceneState = State.SceneOut;
            CurrentScene.OnExit();
            NextScene = s;
            NextScene.Initilize(cm, this);
        }

        /// <summary>
        /// Run the update of the current scene, or load the next schene if it is null
        /// </summary>
        /// <param name="gt">Game Time</param>
        public void Update(float gt)
        {
            if (CurrentScene == null)
            {
                CurrentScene = NextScene;
                NextScene = null;
                CurrentScene.Initilize(cm, this);
                CurrentScene.OnLoad();
                SceneState = State.SceneIn;
            }
            else if (CurrentScene != null)
            {
                CurrentScene.Update(gt);
            }
        }

        public void InitilizeCurrentScene()
        {
            CurrentScene.Initilize(cm, this);
        }
    }
}
