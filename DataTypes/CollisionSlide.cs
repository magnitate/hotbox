using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DataTypes
{
    public class CollisionSlide : CollisionSlope
    {
        public float SlideBoost = 0.0f;
        public float SlideDirection = 0.0f;

        public CollisionSlide()
            : base()
        {
            CollisionType = TileCollision.Slide;
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
