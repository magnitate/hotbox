using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HotboxTest.GameObject
{
    public class Camera2D
    {
        //The furthest and nearest you can zoom.
        const float ZoomDefault = 1.0f;
        const float ZoomUpperBounds = 1.0f;
        const float ZoomLowerBounds = 0.75f;

        //The highest and lowest the player is positioned
        //on the screen's Y axis
        const float PlayerYDefault = 1.5f;
        const float PlayerYUpperBounds = 2.5f;
        const float PlayerYLowerBounds = 1.5f;

        public Camera2D(Viewport viewport)
        {
            Origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Zoom = 1.0f;
            PlayerYPos = 1.5f;
            _viewport = viewport;
        }

        public Vector2 Origin { get; set; }


        //Sets playerYPos to the given value, but locks it within the bounds
        private float _playerYPos = PlayerYDefault;
        public float PlayerYPos
        {
            get { return _playerYPos; }
            set
            {
                _playerYPos = value;
                if (_playerYPos < PlayerYLowerBounds)
                    _playerYPos = PlayerYLowerBounds;
                else if (_playerYPos > PlayerYUpperBounds)
                    _playerYPos = PlayerYUpperBounds;
            }
        }

        //Sets zoom to the given value, but locks it within the bounds
        private float _zoom = ZoomDefault;
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = value;
                if (_zoom < ZoomLowerBounds)
                {
                    _zoom = ZoomLowerBounds;
                }
                else if (_zoom > ZoomUpperBounds)
                {
                    _zoom = ZoomUpperBounds;
                }
            }
        }

        public float Rotation { get; set; }

        private readonly Viewport _viewport;
        private Rectangle? _limits;
        private Vector2 _position;

        //Determines whether the camera should be panning
        //or not. Whenever it is set, it resets the movefactor
        bool _slideCamera = false;

        public bool SlideCamera
        {
            get { return _slideCamera; }
            set
            {
                _slideCamera = value;
                _moveCameraFactor = 0.0f;
            }
        }

        //Tracks the progress of the pan
        float _moveCameraFactor = 0.0f;
        //the amount to pan by - the smaller, the faster it pans
        const float CameraPanAmount = 32;
        
        
        //Previous camera position only set when the camera is not
        //panning towards a new position
        Vector2 _previousCameraPosition = Vector2.Zero;

        public Vector2 PreviousCameraPosition
        {
            get { return _previousCameraPosition; }
            set
            {
                if (_moveCameraFactor == 0.0f)
                    _previousCameraPosition = value;
            }
        }

        /// <summary>
        /// When called, if the camera should be sliding, it increments the pan value
        /// Otherwise, if max pan value is reached, it disables the sliding.
        /// </summary>
        public void Pan()
        {
            if (SlideCamera)
                _moveCameraFactor = Math.Min(_moveCameraFactor + (1f / CameraPanAmount), 1);

            if (_moveCameraFactor == 1.0f)
            {
                SlideCamera = false;
            }
        }

        /// <summary>
        /// Resets the camera with default values to look at the given position
        /// </summary>
        /// <param name="ResetPosition">The position to look at</param>
        /// <param name="BaseScreenSize"></param>
        public void Reset(Vector2 ResetPosition, Vector2 BaseScreenSize)
        {
            SlideCamera = false;
            Zoom = ZoomDefault;
            PlayerYPos = PlayerYDefault;
            LookAt(ResetPosition, BaseScreenSize);
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            // To add parallax, simply multiply it by the position
            return Matrix.CreateTranslation(new Vector3(-Position * parallax, 0.0f)) *
                // The next line has a catch. See note below.
                   Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom, Zoom, 1) *
                   Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public Rectangle? Limits
        {
            get { return _limits; }
            set
            {
                if (value != null)
                {
                    // Assign limit but make sure it's always bigger than the viewport
                    _limits = new Rectangle
                    {
                        X = value.Value.X,
                        Y = value.Value.Y,
                        Width = System.Math.Max(_viewport.Width, value.Value.Width),
                        Height = System.Math.Max(_viewport.Height, value.Value.Height)
                    };

                    // Validate camera position with new limit
                    Position = Position;
                }
                else
                {
                    _limits = null;
                }
            }
        }

        public void LookAt(Vector2 position, Vector2 baseScreenSize)
        {
            //Position = position - new Vector2(_viewport.Width / 2.0f, _viewport.Height / 2.0f);
            //Position = position - new Vector2(baseScreenSize.X / 2.5f, baseScreenSize.Y / PlayerYPos);

            if (SlideCamera)
            {
                float xPos = MathHelper.Lerp(PreviousCameraPosition.X, position.X, _moveCameraFactor);
                float yPos = MathHelper.Lerp(PreviousCameraPosition.Y, position.Y, _moveCameraFactor);

                Position = new Vector2(xPos, yPos) - new Vector2(baseScreenSize.X / 2.5f, baseScreenSize.Y / PlayerYPos);
            }
            else
            {
                Position = position - new Vector2(baseScreenSize.X / 2.5f, baseScreenSize.Y / PlayerYPos);
            }
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                // If there's a limit set and the camera is not transformed clamp position to limits
                if (Limits != null && Zoom == 1.0f && Rotation == 0.0f)
                {
                    _position.X = MathHelper.Clamp(_position.X, Limits.Value.X, Limits.Value.X + Limits.Value.Width - _viewport.Width);
                    _position.Y = MathHelper.Clamp(_position.Y, Limits.Value.Y, Limits.Value.Y + Limits.Value.Height - _viewport.Height);
                }
            }
        }
    }
}
