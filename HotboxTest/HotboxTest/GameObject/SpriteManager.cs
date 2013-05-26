using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HotboxTest.GameObject
{
    public abstract class SpriteManager : Sprite
    {

        protected Dictionary<string, AnimationClass> Animations = new Dictionary<string, AnimationClass>();
        protected int FrameIndex = 0;

        private string animation;
        public string Animation
        {
            get { return animation; }
            set
            {
                animation = value;
                FrameIndex = 0;
            }
        }

        protected Vector2 Origin;

        protected int Frames;
        protected int height;
        protected int width;
        private int maxAnimations;

        public SpriteManager(int NumberOfFrames, int NumberOfAnimations)
        {
            Frames = NumberOfFrames;
            maxAnimations = NumberOfAnimations;
        }

        //Load the texture for the sprite using the content pipeline
        public override void LoadContent(ContentManager theContentManager, String theAssetName)
        {
            base.LoadContent(theContentManager, theAssetName);

            width = mSpriteTexture.Width / Frames;
            height = mSpriteTexture.Height / maxAnimations;
            Origin = new Vector2(width / 2, height / 2);
        }

        public override Rectangle BoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }

        public void AddAnimation(string name, int row, int frames, AnimationClass animation)
        {
            Rectangle[] recs = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
            {
                recs[i] = new Rectangle(i * width, (row - 1) * height, width, height);
            }
            animation.Frames = frames;
            animation.Rectangles = recs;
            Animations.Add(name, animation);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, Animations[Animation].Rectangles[FrameIndex], Animations[Animation].Colour, Rotation, Origin, Scale, Animations[Animation].SpriteEffect, 0f);
        }
    }
}
