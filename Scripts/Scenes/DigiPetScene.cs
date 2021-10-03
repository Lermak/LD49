using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
            ResourceManager.Textures["DigiPetNeeds"] = Content.Load<Texture2D>("Images/DigiPet/chat_sprite_sheet");
            ResourceManager.Textures["DigiPetBG"] = Content.Load<Texture2D>("Images/DigiPet/background");
            ResourceManager.Textures["FeedUp"] = Content.Load<Texture2D>("Images/DigiPet/button_feed");
            ResourceManager.Textures["FeedDown"] = Content.Load<Texture2D>("Images/DigiPet/button_feed_press");
            ResourceManager.Textures["WashUp"] = Content.Load<Texture2D>("Images/DigiPet/button_wash");
            ResourceManager.Textures["WashDown"] = Content.Load<Texture2D>("Images/DigiPet/button_wash_press");
            ResourceManager.Textures["PlayUp"] = Content.Load<Texture2D>("Images/DigiPet/button_play");
            ResourceManager.Textures["PlayDown"] = Content.Load<Texture2D>("Images/DigiPet/button_play_press");
            ResourceManager.Textures["DigiPet"] = Content.Load<Texture2D>("Images/DigiPet/pet_sprite_sheet");
            ResourceManager.SoundEffects["DigiPetWalk1"] = Content.Load<SoundEffect>("Sound/DigiPet/walk_1");
            ResourceManager.SoundEffects["DigiPetWalk2"] = Content.Load<SoundEffect>("Sound/DigiPet/walk_2");
            ResourceManager.SoundEffects["DigiPetWalk3"] = Content.Load<SoundEffect>("Sound/DigiPet/walk_3");
            ResourceManager.SoundEffects["DigiPetWalk4"] = Content.Load<SoundEffect>("Sound/DigiPet/walk_4");
            ResourceManager.SoundEffects["DigiPetWant"] = Content.Load<SoundEffect>("Sound/DigiPet/want");
            ResourceManager.SoundEffects["DigiPetSuccess"] = Content.Load<SoundEffect>("Sound/DigiPet/success");

            GameObjects.Add(new WorldObject("DigiPetBG", "DigiPetBG", new Vector2(480, 340), new Vector2(-720, 380), 0));
            
            GameObjects.Add(new DigiPet("DigiPet", "DigiPet", new Vector2(80, 80), new Vector2(-810, 370), 2));
            DigiPet d = (DigiPet)GameObjects[GameObjects.Count - 1];
            GameObjects.Add(new DigiPetNeeds("DigiPetNeeds", "Needs", new Vector2(240, 240), new Vector2(-810, 375), 1));
            DigiPetNeeds dpn = (DigiPetNeeds)GameObjects[^1];
            ((DigiPetData)d.ComponentHandler.GetComponent("DigiPetData")).Needs = (AnimationData)dpn.ComponentHandler.GetComponent("AnimationData");
            dpn.Transform.AttachToTransform(d.Transform);

            GameObjects.Add(new FeedButton("FeedUp", "FeedUp", "FeedButton", new Vector2(160, 90), new Vector2(-880, 260), 1, (DigiPetData)d.ComponentHandler.GetComponent("DigiPetData"), (AnimationData)d.ComponentHandler.GetComponent("AnimationData")));
            GameObjects.Add(new WashButton("WashUp", "WashUp", "WashButton", new Vector2(160, 90), new Vector2(-720, 260), 1, (DigiPetData)d.ComponentHandler.GetComponent("DigiPetData"), (AnimationData)d.ComponentHandler.GetComponent("AnimationData")));
            GameObjects.Add(new PlayButton("PlayUp", "PlayUp", "PlayButton", new Vector2(160, 90), new Vector2(-560, 260), 1, (DigiPetData)d.ComponentHandler.GetComponent("DigiPetData"), (AnimationData)d.ComponentHandler.GetComponent("AnimationData")));

        }
    }
}
