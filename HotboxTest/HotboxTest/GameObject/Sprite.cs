using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HotboxTest.GameObject
{
    public class Sprite
    {
        //The current position of the sprite
        public Vector2 Position = new Vector2(0, 0);

        public Color Colour = Color.White;

        //the texture of the sprite
        protected Texture2D mSpriteTexture;
        public float Rotation;

        //The assetname for the sprite
        public string AssetName;
        //the size of the sprite
        public Rectangle Size;
        //Used to size the sprite up or down from the original image
        private float mScale = 1.0f;

        //The rectangular area from the original image that defines the sprite
        Rectangle mSource;

        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }

        //When the scale is modified through the property, the Size of the sprite is recalculated with the new scale
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the size of the sprite with the new scale
                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }

        public virtual Rectangle BoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Size.Width, Size.Height);
        }

        //Load the texture for the sprite using the content pipeline
        public virtual void LoadContent(ContentManager theContentManager, String theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Source = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * Scale), (int)(mSpriteTexture.Height * Scale));
            Rotation = 0.0f;
        }

        //Update the sprite's position based on it's speed, direction and elapsed gametime
        public virtual void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theSpeed * theDirection * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position, Source, Colour, Rotation, new Vector2(Size.Width / 2, Size.Height / 2), Scale, SpriteEffects.None, 0);
        }
    }
}
