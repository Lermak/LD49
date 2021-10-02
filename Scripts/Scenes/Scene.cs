using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGame_Core.Scripts
{
    public class Scene
    {
        protected SceneManager sceneManager;
        protected Vector2 size;//Scene size must never be smaller than the rendering size
        protected ContentManager Content;
        protected List<Camera> Cameras;
        public Vector2 Size { get { return size; } }

        public List<GameObject> ToAdd = new List<GameObject>();
        public List<GameObject> GameObjects = new List<GameObject>();


        public virtual void Initilize(ContentManager c, SceneManager sm)
        {
            sceneManager = sm;
            size = new Vector2(RenderingManager.WIDTH, RenderingManager.HEIGHT);
            Content = c;
            loadContent(sceneManager.Cameras);
        }

        protected virtual void loadContent(List<Camera> c)
        {
            Cameras = c;
            foreach (GameObject go in GameObjects)
            {
                go.Initilize();
            }
        }

        public virtual void OnLoad()
        {
            CoroutineManager.AddCoroutine(Coroutines.FadeInSceneTransision(sceneManager), "FadeIn", 0, true);
        }

        public virtual void OnExit()
        {
            CoroutineManager.AddCoroutine(Coroutines.FadeOutSceneTransision(sceneManager), "FadeOut", 0, true);
        }

        public virtual void Update(float gt)
        {
            if (InputManager.IsKeyTriggered(Microsoft.Xna.Framework.Input.Keys.Escape))
                if (sceneManager.SceneState == SceneManager.State.Running)
                    sceneManager.SceneState = SceneManager.State.Paused;
                else
                    sceneManager.SceneState = SceneManager.State.Running;

            if (sceneManager.SceneState == SceneManager.State.Running)
                SceneRunning(gt);
            else if (sceneManager.SceneState == SceneManager.State.Paused)
                ScenePaused(gt);
        }

        public virtual void SceneRunning(float gt)
        {
            List<GameObject> destroy = new List<GameObject>();
            foreach (GameObject go in GameObjects)
            {
                go.Update(gt);
                if (go.ToDestroy)
                    destroy.Add(go);
            }
            foreach (GameObject go in destroy)
            {
                go.OnDestroy();
                GameObjects.Remove(go);
            }
            foreach (GameObject go in ToAdd)
            {
                go.Initilize();
                GameObjects.Add(go);
            }
            ToAdd.Clear();
        }

        public virtual void ScenePaused(float gt)
        {
            List<GameObject> destroy = new List<GameObject>();
            foreach (GameObject go in GameObjects)
            {
                go.Update(gt);
                if (go.ToDestroy)
                    destroy.Add(go);
            }
            foreach (GameObject go in destroy)
            {
                go.OnDestroy();
                GameObjects.Remove(go);
            }
            foreach (GameObject go in ToAdd)
            {
                go.Initilize();
                GameObjects.Add(go);
            }
            ToAdd.Clear();
        }
    }
}
