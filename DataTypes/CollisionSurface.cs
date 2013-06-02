using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DataTypes
{
    public enum TileCollision
    {
        /// <summary>
        /// A slope tile is one which moves the player up/down a gradient.
        /// </summary>
        Slope = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,

        /// <summary>
        /// A bounce tile is one that give the player a jump boost when they land on top of it.
        /// </summary>
        Bounce = 3,
    }

    public class CollisionSurface : Sprite
    {
        public int Width;
        public int Height;
        public bool Visible;

        private float bottomSlopePoint = 0;
        public TileCollision CollisionType;

        public float BounceVelocityX = 0.0f;
        public float BounceVelocityY = 0.0f;

        public CollisionSurface()
        {
            AssetName = "collision_surface";
            Colour = Color.White;
            Visible = true;
        }

        public float BottomSlopePoint
        {
            get { return bottomSlopePoint; }
            set { bottomSlopePoint = value; }
        }

        public override Rectangle BoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Colour != Color.White)
                Colour = Color.White;
        }

        public override void Draw(SpriteBatch theSpritebatch)
        {
            if (Visible)
            {
                if (bottomSlopePoint == Position.X + Width)
                    theSpritebatch.Draw(mSpriteTexture, BoundingBox(), null, Colour, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                else
                    theSpritebatch.Draw(mSpriteTexture, BoundingBox(), Colour);
            }
        }
    }
}
