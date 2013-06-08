using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DataTypes
{
    public class CollisionSlope : CollisionSurface
    {
        protected float bottomSlopePoint = 0;

        public CollisionSlope()
            : base()
        {
            AssetName = "collision_slope";
            CollisionType = TileCollision.Slope;
        }

        public float BottomSlopePoint
        {
            get { return bottomSlopePoint; }
            set { bottomSlopePoint = value; }
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
