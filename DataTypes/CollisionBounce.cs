using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTypes
{
    public class CollisionBounce : CollisionSurface
    {
        public CollisionBounce()
            : base()
        {
            CollisionType = TileCollision.Bounce;
        }

        public float BounceVelocityX = 0.0f;
        public float BounceVelocityY = 0.0f;
    }
}
