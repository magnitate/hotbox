using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DataTypes
{
    public enum PickupVersion
    {
        /// <summary>
        /// A lifeblood pickup is one that
        /// </summary>
        Lifeblood = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Health = 1,
    }

    public class Pickup : Sprite
    {
        public bool Active;
        public PickupVersion PickupType;

        public int Value;

        private float jitterAmount = 0.5f;
        private Random rand = new Random();
        private Vector2 PositionAdjustment;

        public Pickup()
        {
            AssetName = "lifeblood";
            Colour = Color.White;
            Active = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            //TODO bob up and down
            if (Active)
            {
                PositionAdjustment = Position;
                PositionAdjustment.X += rand.Next(-100, 100) * jitterAmount * (float)gameTime.ElapsedGameTime.TotalSeconds;
                PositionAdjustment.Y += rand.Next(-100, 100) * jitterAmount * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public override void Draw(SpriteBatch theSpritebatch)
        {
            if (Active)
                theSpritebatch.Draw(mSpriteTexture, new Rectangle((int)PositionAdjustment.X, (int)PositionAdjustment.Y, Size.Width, Size.Height), Colour);
        }
    }
}