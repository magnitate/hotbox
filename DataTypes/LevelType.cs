using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DataTypes
{
    public class LevelType
    {
        public int elf = 23;
        public string hello = "Hello World";

        public string[] s = { "hi", "yo", "sup" };


        public Layers Layers = new Layers();
    }

    public class Layers
    {
        public Sprite[] LayerOne =
            {
                new Sprite { AssetName = "Tutorial/back", Position = new Vector2(1024, 1430) }       
            };
        
        public Sprite[] LayerTwo =
            {
                new Sprite { AssetName = "Layer6-3", Position = new Vector2(60, 1740) },
                new Sprite { AssetName = "Layer6-3", Position = new Vector2(1960, 1240) },
                new Sprite { AssetName = "Layer6-3", Position = new Vector2(3360, 1800) }
            };

        public Sprite[] LayerThree;

        public Sprite[] Collision = 
        {   
                //bush slope
                new CollisionSurface(){ Position = new Vector2(900, 1860), Height = 500, Width = 40, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                /*
                //house slope
                new CollisionSurface(new Vector2(2100, 1840), 300, 20, 0, TileCollision.Slope) { AssetName = "collision_slope" },

                //windmill hill

                    //second row small
                new CollisionSurface(new Vector2(2460, 1780), 200, 50, 0, TileCollision.Slope) { AssetName = "collision_slope" },

                    //massive one
                new CollisionSurface(new Vector2(2600, 1400), 720, 440, 0, TileCollision.Slope) { AssetName = "collision_slope" },//this is big

                    //original row merged into bigger
                new CollisionSurface(new Vector2(2400, 1790), 200, 50, 0, TileCollision.Slope) { AssetName = "collision_slope" },
                new CollisionSurface(new Vector2(2600, 1640), 300, 150, 0, TileCollision.Slope) { AssetName = "collision_slope" },
                new CollisionSurface(new Vector2(2900, 1590), 80, 50, 0, TileCollision.Slope) { AssetName = "collision_slope" },
                new CollisionSurface(new Vector2(2980, 1490), 140, 100, 0, TileCollision.Slope) { AssetName = "collision_slope" },
                new CollisionSurface(new Vector2(3120, 1440), 100, 50, 0, TileCollision.Slope) { AssetName = "collision_slope" },
                new CollisionSurface(new Vector2(3220, 1400), 90, 40, 0, TileCollision.Slope) { AssetName = "collision_slope" },
                new CollisionSurface(new Vector2(3310, 1380), 340, 20, 0, TileCollision.Slope) { AssetName = "collision_slope" },
                new CollisionSurface(new Vector2(3650, 1380), 300, 50, 300, TileCollision.Slope) { AssetName = "collision_slope" },

                //cart collision
                //new CollisionSurface(new Vector2(4470, 1820), 250, 50, 250, TileCollision.Slope) { AssetName = "collision_slope" },
                new CollisionSurface(new Vector2(4730, 1980), 220, 100, 220, TileCollision.Slope) { AssetName = "collision_slope" },
                */


                //FLAT COLLISION
                new CollisionSurface(){ Position = new Vector2(50, 1900), Height = 1400, Width = 150, BottomSlopePoint = 0, CollisionType = TileCollision.Impassable },
                /*new CollisionSurface(new Vector2(50, 900), 150, 1000, 0, TileCollision.Impassable),
                new CollisionSurface(new Vector2(450, 1100), 150, 500, 0, TileCollision.Impassable),

                new CollisionSurface(new Vector2(1330, 1780), 50, 100, 0, TileCollision.Impassable),
                new CollisionSurface(new Vector2(2230, 1740), 50, 100, 0, TileCollision.Impassable),
                new CollisionSurface(new Vector2(2400, 1840), 920, 150, 0, TileCollision.Impassable),

                new CollisionSurface(new Vector2(3310, 1400), 340, 150, 0, TileCollision.Impassable),
                new CollisionSurface(new Vector2(3650, 1430), 300, 150, 0, TileCollision.Impassable),
                new CollisionSurface(new Vector2(1400, 1860), 1000, 150, 0, TileCollision.Impassable),
                new CollisionSurface(new Vector2(3900, 1430), 600, 150, 0, TileCollision.Impassable),
                new CollisionSurface(new Vector2(4400, 1550), 100, 600, 0, TileCollision.Impassable),
                new CollisionSurface(new Vector2(4500, 2070), 800, 150, 0, TileCollision.Impassable),
                new CollisionSurface(new Vector2(4470, 1870), 280, 230, 0, TileCollision.Bounce) { BounceVelocityX = -500.0f, BounceVelocityY = -8500.0f },
                new CollisionSurface(new Vector2(4970, 2000), 280, 230, 0, TileCollision.Bounce) { BounceVelocityX = -900.0f, BounceVelocityY = -4000.0f },
                new CollisionSurface(new Vector2(5300, 2070), 15000, 150, 0, TileCollision.Impassable)*/

        };
    }
}