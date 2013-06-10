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
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 0,

        /// <summary>
        /// A slope tile is one which moves the player up/down a gradient.
        /// </summary>
        Slope = 1,

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

        /// <summary>
        /// A slide is a sloped collision surface that causes the player to lose control over movement.
        /// They can jump, but not run up the slide.
        /// </summary>
        Slide = 4,

        /// <summary>
        /// A conveyor is a flat collision surface that acts similarly to a slide, causing the player to
        /// move in the given direction.
        /// </summary>
        Conveyor = 5,

        /// <summary>
        /// A Walljump collision surface allows the player to walljump
        /// </summary>
        Walljump = 6,

        /// <summary>
        /// A moving collsion surface will change between 2 waypoints.
        /// </summary>
        Moving = 7,

    }

    public class CollisionSurface : Sprite
    {
        public int Width;
        public int Height;
        public bool Visible;

        public TileCollision CollisionType;

        public CollisionSurface()
        {
            AssetName = "collision_surface";
            CollisionType = TileCollision.Impassable;
            Colour = Color.White;
            Visible = true;
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
                theSpritebatch.Draw(mSpriteTexture, BoundingBox(), Colour);
        }
    }
}
