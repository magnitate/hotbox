using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataTypes
{
    public class CollisionConveyor : CollisionSurface
    {
        public CollisionConveyor()
            : base()
        {
            CollisionType = TileCollision.Conveyor;
        }

        public float SlideBoost = 0.0f;
        public float SlideDirection = 0.0f;
    }
}
