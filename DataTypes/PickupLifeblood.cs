using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DataTypes
{
    public class PickupLifeblood : Pickup
    {
        public int Value;

        public PickupLifeblood()
        {
            AssetName = "lifeblood";
            PickupType = PickupItem.Lifeblood;
            Value = 1;
        }

        public override Rectangle BoundingBox()
        {
            return new Rectangle((int)PositionAdjustment.X, (int)PositionAdjustment.Y, (Size.Width / 2), (Size.Height / 2));
        }
    }
}
