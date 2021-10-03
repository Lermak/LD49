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
            ResourceManager.Textures["DigiPet"] = Content.Load<Texture2D>("Images/DigiPet/pet_sprite_sheet");
            GameObjects.Add(new WorldObject("DigiPetBG", "Background", new Vector2(400, 600), new Vector2(-760, 240), 0));
            GameObjects.Add(new DigiPet("DigiPet", "DigiPet", new Vector2(80, 80), new Vector2(-810, 450), 1));
            DigiPet d = (DigiPet)GameObjects[GameObjects.Count - 1];
            GameObjects.Add(new DigiPetNeeds("DigiPetNeeds", "Needs", new Vector2(240, 240), new Vector2(), 1));
            DigiPetNeeds dpn = (DigiPetNeeds)GameObjects[^1];
            ((DigiPetData)d.ComponentHandler.GetComponent("DigiPetData")).Needs = (AnimationData)dpn.ComponentHandler.GetComponent("AnimationData");
            dpn.Transform.AttachToTransform(d.Transform);
            dpn.Transform.Place(new Vector2());
            GameObjects.Add(new FeedButton("ButtonUp", "ButtonUp", "FeedButton", new Vector2(75, 75), new Vector2(-860, 60), 1, (DigiPetData)d.ComponentHandler.GetComponent("DigiPetData"), (AnimationData)d.ComponentHandler.GetComponent("AnimationData")));
            GameObjects.Add(new WashButton("ButtonUp", "ButtonUp", "WashButton", new Vector2(75, 75), new Vector2(-760, 60), 1, (DigiPetData)d.ComponentHandler.GetComponent("DigiPetData"), (AnimationData)d.ComponentHandler.GetComponent("AnimationData")));
            GameObjects.Add(new PlayButton("ButtonUp", "ButtonUp", "PlayButton", new Vector2(75, 75), new Vector2(-660, 60), 1, (DigiPetData)d.ComponentHandler.GetComponent("DigiPetData"), (AnimationData)d.ComponentHandler.GetComponent("AnimationData")));

        }
    }
}
