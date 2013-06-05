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
        public Sprite[] Background =
            {
                new Sprite { AssetName = "Tutorial/back", Position = new Vector2(1024, 1430) }       
            };
        
        public Sprite[] CloudOne =
            {
                new Sprite { AssetName = "Layer6-3", Position = new Vector2(60, 1740) },
                new Sprite { AssetName = "Layer6-3", Position = new Vector2(1960, 1240) },
                new Sprite { AssetName = "Layer6-3", Position = new Vector2(3360, 1800) }
            };

        public Sprite[] CloudTwo = 
            {
                new Sprite { AssetName = "Layer6-2", Position = new Vector2(960, 1240) },
                new Sprite { AssetName = "Layer6-2", Position = new Vector2(1560, 1300) },
                new Sprite { AssetName = "Layer6-2", Position = new Vector2(2560, 1400) },
                new Sprite { AssetName = "Layer6-2", Position = new Vector2(3260, 1900) }
            };

        public Sprite[] CloudThree = 
            {
                new Sprite { AssetName = "Layer6-1", Position = new Vector2(460, 1540) },
                new Sprite { AssetName = "Layer6-1", Position = new Vector2(1860, 1240) },
                new Sprite { AssetName = "Layer6-1", Position = new Vector2(2860, 1340) },
                new Sprite { AssetName = "Layer6-1", Position = new Vector2(2360, 1640) },
                new Sprite { AssetName = "Layer6-1", Position = new Vector2(3360, 2040) }
            };

        public Sprite[] Horizon = 
            {
                new Sprite { AssetName = "Layer5-3", Position = new Vector2(960, 1700) },
                new Sprite { AssetName = "Layer5-3", Position = new Vector2(2260, 1500) },
                new Sprite { AssetName = "Layer5-3", Position = new Vector2(3860, 1700) },
                new Sprite { AssetName = "Layer5-3", Position = new Vector2(4860, 1900) }
            };

        public Sprite[] Distant = 
            {
                new Sprite { AssetName = "Layer5-2", Position = new Vector2(500, 1700) },
                new Sprite { AssetName = "Layer5-2", Position = new Vector2(2500, 1600) },
                new Sprite { AssetName = "Layer5-2", Position = new Vector2(3500, 1700) },
                new Sprite { AssetName = "Layer5-2", Position = new Vector2(4500, 1800) }
            };

        public Sprite[] Far = 
            {
                new Sprite { AssetName = "Layer5-1", Position = new Vector2(600, 1700) },
                new Sprite { AssetName = "Layer5-1", Position = new Vector2(2300, 1400) },
                new Sprite { AssetName = "Layer5-1", Position = new Vector2(2900, 1700) },
                new Sprite { AssetName = "Layer5-1", Position = new Vector2(4900, 1700) }
            };

        public Sprite[] Rear = 
            {
                new Sprite { AssetName = "Tutorial/entrance", Position = new Vector2(300, 1380) },
                new Sprite { AssetName = "Tutorial/bush", Position = new Vector2(900, 1700) },
                new Sprite { AssetName = "Tutorial/house", Position = new Vector2(1800, 1380) },
                new Sprite { AssetName = "Tutorial/tree1", Position = new Vector2(2500, 1425) },
                new Sprite { AssetName = "Tutorial/windmill", Position = new Vector2(3420, 800) },
                new Sprite { AssetName = "Tutorial/windmill_fence", Position = new Vector2(4150, 1400) }
            };

        public Sprite[] Mid = 
            {
                new Sprite { AssetName = "Tutorial/mid1", Position = new Vector2(2048, 1938) },
                new Sprite { AssetName = "Tutorial/mid2", Position = new Vector2(6144, 1938) },
                new Sprite { AssetName = "Tutorial/mid3", Position = new Vector2(10240, 1938) },
                new Sprite { AssetName = "Tutorial/mid4", Position = new Vector2(14336, 1938) },
                new Sprite { AssetName = "Tutorial/jumping_post1", Position = new Vector2(1350, 1820) },
                new Sprite { AssetName = "Tutorial/jumping_post2", Position = new Vector2(2250, 1800) },
                new Sprite { AssetName = "Tutorial/cart", Position = new Vector2(4670, 1950) }
            };

        public Sprite[] Collision = 
        {   
                //bush slope
                new CollisionSurface(){ Position = new Vector2(900, 1860), Width = 500, Height = 40, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                
                //house slope
                new CollisionSurface(){ Position = new Vector2(2100, 1840), Width = 300, Height = 20, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },

                //windmill hill

                    //second row small
                new CollisionSurface(){ Position = new Vector2(2460, 1780), Width = 200, Height = 50, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },

                    //massive one
                new CollisionSurface(){ Position = new Vector2(2600, 1400), Width = 720, Height = 440, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },//this is big

                    //original row merged into bigger
                new CollisionSurface(){ Position = new Vector2(2400, 1790), Width = 200, Height = 50, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                new CollisionSurface(){ Position = new Vector2(2600, 1640), Width = 300, Height = 150, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                new CollisionSurface(){ Position = new Vector2(2900, 1590), Width = 80, Height = 50, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                new CollisionSurface(){ Position = new Vector2(2980, 1490), Width = 140, Height = 100, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                new CollisionSurface(){ Position = new Vector2(3120, 1440), Width = 100, Height = 50, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                new CollisionSurface(){ Position = new Vector2(3220, 1400), Width = 90, Height = 40, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                new CollisionSurface(){ Position = new Vector2(3310, 1380), Width = 340, Height = 20, BottomSlopePoint = 0, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                new CollisionSurface(){ Position = new Vector2(3650, 1380), Width = 300, Height = 50, BottomSlopePoint = 300, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },

                //cart collision
                new CollisionSurface(){ Position = new Vector2(4730, 1980), Width = 220, Height = 100, BottomSlopePoint = 220, CollisionType = TileCollision.Slope, AssetName = "collision_slope" },
                              


                //FLAT COLLISION
                new CollisionSurface(){ Position = new Vector2(50, 1900), Width = 150, Height = 1400, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(50, 900), Width = 150, Height = 1000, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(450, 1100), Width = 150, Height = 500, CollisionType = TileCollision.Impassable },

                new CollisionSurface(){ Position = new Vector2(1330, 1780), Width = 50, Height = 100, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(2230, 1740), Width = 50, Height = 100, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(2400, 1840), Width = 920, Height = 150, CollisionType = TileCollision.Impassable },
                
                new CollisionSurface(){ Position = new Vector2(3310, 1400), Width = 340, Height = 150, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(3650, 1430), Width = 300, Height = 150, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(1400, 1860), Width = 1000, Height = 150, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(3900, 1430), Width = 600, Height = 150, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(4400, 1550), Width = 100, Height = 600, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(4500, 2070), Width = 800, Height = 150, CollisionType = TileCollision.Impassable },
                new CollisionSurface(){ Position = new Vector2(5300, 2070), Width = 15000, Height = 150, CollisionType = TileCollision.Impassable },

                //BOUNCE
                new CollisionSurface(){ Position = new Vector2(4470, 1870), Width = 280, Height = 230, BounceVelocityX = -500.0f, BounceVelocityY = -8500.0f, CollisionType = TileCollision.Bounce },
                new CollisionSurface(){ Position = new Vector2(4970, 2000), Width = 280, Height = 230, BounceVelocityX = -900.0f, BounceVelocityY = -4000.0f, CollisionType = TileCollision.Bounce },

        };
    }
}