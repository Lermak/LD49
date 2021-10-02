using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    class DigiPetScene : Scene
    {
        protected override void loadContent(List<Camera> c)
        {
            ResourceManager.Textures["DigiPetBG"] = Content.Load<Texture2D>("Images/DigiPet/DigiPetBackground");
            ResourceManager.Textures["ButtonDown"] = Content.Load<Texture2D>("Images/DigiPet/ButtonDown");
            ResourceManager.Textures["ButtonUp"] = Content.Load<Texture2D>("Images/DigiPet/ButtonUp");
            ResourceManager.Textures["DigiPet"] = Content.Load<Texture2D>("Images/DigiPet/DigiPet");
            GameObjects.Add(new WorldObject(sceneManager, sceneManager.Cameras, "DigiPetBG", "Background", new Vector2(400, 600), new Vector2(-760, 240), 0));
            GameObjects.Add(new DigiPet(sceneManager, sceneManager.Cameras, "DigiPet", "DigiPet", new Vector2(300, 300), new Vector2(-760, 390), 1));
            DigiPet d = (DigiPet)GameObjects[GameObjects.Count - 1];
            GameObjects.Add(new FeedButton(sceneManager, sceneManager.Cameras, "ButtonUp", "ButtonUp", "FeedButton", new Vector2(75, 75), new Vector2(-860, 60), 1, (DigiPetData)d.ComponentHandler.GetComponent("DigiPetData"), (AnimationData)d.ComponentHandler.GetComponent("AnimationData")));
            GameObjects.Add(new WashButton(sceneManager, sceneManager.Cameras, "ButtonUp", "ButtonUp", "FeedButton", new Vector2(75, 75), new Vector2(-760, 60), 1, (DigiPetData)d.ComponentHandler.GetComponent("DigiPetData"), (AnimationData)d.ComponentHandler.GetComponent("AnimationData")));
            GameObjects.Add(new PlayButton(sceneManager, sceneManager.Cameras, "ButtonUp", "ButtonUp", "FeedButton", new Vector2(75, 75), new Vector2(-660, 60), 1, (DigiPetData)d.ComponentHandler.GetComponent("DigiPetData"), (AnimationData)d.ComponentHandler.GetComponent("AnimationData")));

        }
    }
}
