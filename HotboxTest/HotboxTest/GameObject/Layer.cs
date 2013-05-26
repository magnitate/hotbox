using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HotboxTest.GameObject
{
    public class Layer
    {
        public Layer(Camera2D camera)
        {
            _camera = camera;
            Parallax = Vector2.One;
            Sprites = new List<Sprite>();
        }

        public Vector2 Parallax { get; set; }
        public List<Sprite> Sprites { get; private set; }

        private readonly Camera2D _camera;

        public void Draw(SpriteBatch spriteBatch, Matrix ResolutionScaleMatrix)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, _camera.GetViewMatrix(Parallax) * ResolutionScaleMatrix);
           
            foreach (Sprite sprite in Sprites)
                sprite.Draw(spriteBatch);

            spriteBatch.End();
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, _camera.GetViewMatrix(Parallax));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(_camera.GetViewMatrix(Parallax)));
        }
    }
}
