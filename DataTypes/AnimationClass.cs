using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DataTypes
{
    public class AnimationClass
    {
        public Rectangle[] Rectangles;
        public Color Colour = Color.White;
        public Vector2 Origin;
        public float Rotation = 0f;
        public float Scale = 1f;
        public SpriteEffects SpriteEffect;
        public bool IsLooping = true;
        public int Frames;

        public AnimationClass Copy()
        {
            AnimationClass ac = new AnimationClass();
            ac.Rectangles = Rectangles;
            ac.Colour = Colour;
            ac.Origin = Origin;
            ac.Rotation = Rotation;
            ac.Scale = Scale;
            ac.SpriteEffect = SpriteEffect;
            ac.IsLooping = IsLooping;
            ac.Frames = Frames;
            return ac;
        }
    }
}
