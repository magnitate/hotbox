using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;

namespace HotboxTest.GameObject
{
    public class SpriteAnimation : SpriteManager
    {
        public SpriteAnimation(int frames, int animations)
            : base(frames, animations)
        {
        }

        private float timeElapsed;
        public bool IsLooping = false;

        // default to 20 frames per second
        private float timeToUpdate = 0.05f;
        public int FramesPerSecond
        {
            set { timeToUpdate = (1f / value); }
        }

        public virtual void Update(GameTime gameTime)
        {
            timeElapsed += (float)
                gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;

                if (FrameIndex < Animations[Animation].Frames - 1)
                {
                    FrameIndex++;
                }
                else if (Animations[Animation].IsLooping)
                {
                    FrameIndex = 0;
                }
            }
        }
    }
}