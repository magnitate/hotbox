#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameStateManagement;
using DataTypes;
#endregion

namespace Hotbox
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        SpriteFont gameFont;

        List<GameObject.Layer> _layers;
        Dictionary<string, int> _layerDictionary;

        GameObject.Camera2D _camera;
        List<GameObject.Player> playerList;

        GameObject.Player playerOne;
        GameObject.Player playerTwo;
        GameObject.Player playerThree;
        GameObject.Player playerFour;

        Vector2 _averagePlayerPosition = Vector2.Zero;

        float pauseAlpha;

        InputAction pauseAction;
        InputAction resetAction;
        InputAction toggleCollisionVisibility;
        InputAction stunPlayers;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start },
                new Keys[] { Keys.Escape },
                true);

            resetAction = new InputAction(
                new Buttons[] { Buttons.Back },
                new Keys[] { Keys.F1 },
                true);

            toggleCollisionVisibility = new InputAction(
                null,
                new Keys[] { Keys.F2},
                true);

            stunPlayers = new InputAction(
                null,
                new Keys[] { Keys.Q },
                true);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                gameFont = content.Load<SpriteFont>("gamefont");

                //INITIALISING CAMERA
                _camera = new GameObject.Camera2D(ScreenManager.GraphicsDevice.Viewport) { Limits = new Rectangle(0, 0, 16388, 2860), Zoom = 1.0f };


                //INITIALISING PLAYERS
                playerOne = new GameObject.Player(PlayerIndex.One, "lucas", 9, 6);
                playerOne.Position = new Vector2(500, 1890);
                playerOne.Scale = 3.0f;
                playerOne.FramesPerSecond = 8;

                playerTwo = new GameObject.Player(PlayerIndex.Two, "lucas", 9, 6);
                playerTwo.Position = new Vector2(550, 1890);
                playerTwo.Scale = 3.0f;
                playerTwo.FramesPerSecond = 8;
                playerTwo.Colour = Color.Violet;

                playerThree = new GameObject.Player(PlayerIndex.Three, "lucas", 9, 6);
                playerThree.Position = new Vector2(600, 1890);
                playerThree.Scale = 3.0f;
                playerThree.FramesPerSecond = 8;
                playerThree.Colour = Color.Green;

                playerFour = new GameObject.Player(PlayerIndex.Four, "lucas", 9, 6);
                playerFour.Position = new Vector2(650, 1890);
                playerFour.Scale = 3.0f;
                playerFour.FramesPerSecond = 8;
                playerFour.Colour = Color.Blue;

                playerList = new List<GameObject.Player>();
                playerList.Add(playerOne);
                playerList.Add(playerTwo);
                //playerList.Add(playerThree);
                //playerList.Add(playerFour);

                
                //INITIALISING LAYERS
                _layerDictionary = new Dictionary<string, int>();

                _layerDictionary.Add("back", 0);
                _layerDictionary.Add("cloud1", 1);
                _layerDictionary.Add("cloud2", 2);
                _layerDictionary.Add("cloud3", 3);
                _layerDictionary.Add("horizon", 4);
                _layerDictionary.Add("distant", 5);
                _layerDictionary.Add("far", 6);
                _layerDictionary.Add("rear", 7);
                _layerDictionary.Add("mid", 8);
                _layerDictionary.Add("c", 9);
                _layerDictionary.Add("interactive", 10);
                _layerDictionary.Add("player", 11);
                _layerDictionary.Add("front", 12);
                _layerDictionary.Add("near", 13);
                _layerDictionary.Add("fore", 14);
                
                // Create 15 layers with parallax ranging from 0% to 100% (only horizontal)
                _layers = new List<GameObject.Layer>
                {
                    new GameObject.Layer(_camera) { Parallax = new Vector2(0.0f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(0.1f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(0.2f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(0.3f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(0.6f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(0.7f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(0.8f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(1.0f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(1.0f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(1.0f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(1.0f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(1.0f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(1.0f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(1.1f, 1.0f) },
                    new GameObject.Layer(_camera) { Parallax = new Vector2(1.2f, 1.0f) },
                };
                
                
                //LevelType level = content.Load<LevelType>("Levels/tutorial2");
                LevelType level = new LevelType();

                importLayers(level);

                //ADD THE PLAYERS TO THE LAYER LIST
                foreach (GameObject.Player player in playerList)
                    _layers[_layerDictionary["player"]].Sprites.Add(player);

                //load content for every sprite in the layers
                foreach (GameObject.Layer layer in _layers)
                {
                    for (int i = 0; i < layer.Sprites.Count; i++)
                    {
                        layer.Sprites[i].LoadContent(content, layer.Sprites[i].AssetName);
                    }
                }

                //START PLAYING THE LEVEL MUSIC
                AudioManager.PlayBgmMusic("Tutorial");

                //MOVE THE CAMERA TO LOOK AT PLAYER 1
                _camera.LookAt(playerList[0].Position, BaseScreenSize);

                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        /// <summary>
        /// For each layer contained in a level file, it loads the objects into the layer list
        /// </summary>
        /// <param name="level">The data loaded from a level file</param>
        private void importLayers(LevelType level)
        {
            if (level.Layers.Background != null)
            {
                foreach (Sprite s in level.Layers.Background)
                {
                    _layers[0].Sprites.Add(s);
                }
            }

            if (level.Layers.CloudOne != null)
            {
                foreach (Sprite s in level.Layers.CloudOne)
                {
                    _layers[1].Sprites.Add(s);
                }
            }

            if (level.Layers.CloudTwo != null)
            {
                foreach (Sprite s in level.Layers.CloudTwo)
                {
                    _layers[2].Sprites.Add(s);
                }
            }

            if (level.Layers.CloudThree != null)
            {
                foreach (Sprite s in level.Layers.CloudThree)
                {
                    _layers[3].Sprites.Add(s);
                }
            }

            if (level.Layers.Horizon != null)
            {
                foreach (Sprite s in level.Layers.Horizon)
                {
                    _layers[4].Sprites.Add(s);
                }
            }

            if (level.Layers.Distant != null)
            {
                foreach (Sprite s in level.Layers.Distant)
                {
                    _layers[5].Sprites.Add(s);
                }
            }

            if (level.Layers.Far != null)
            {
                foreach (Sprite s in level.Layers.Far)
                {
                    _layers[6].Sprites.Add(s);
                }
            }

            if (level.Layers.Rear != null)
            {
                foreach (Sprite s in level.Layers.Rear)
                {
                    _layers[7].Sprites.Add(s);
                }
            }

            if (level.Layers.Mid != null)
            {
                foreach (Sprite s in level.Layers.Mid)
                {
                    _layers[8].Sprites.Add(s);
                }
            }

            foreach (Sprite s in level.Layers.Collision)
            {
                _layers[9].Sprites.Add(s);
            }

            if (level.Layers.Interactive != null)
            {
                foreach (Sprite s in level.Layers.Interactive)
                {
                    _layers[10].Sprites.Add(s);
                }
            }
        }

        /// <summary>
        /// Resets the level back to it's starting state
        /// </summary>
        private void ResetLevel()
        {
            AudioManager.PlayBgmMusic("Tutorial");

            foreach (GameObject.Player p in playerList)
            {
                p.Reset();
            }

            foreach (Pickup p in _layers[_layerDictionary["interactive"]].Sprites)
            {
                p.Active = true;
            }

            _camera.Reset(playerList[0].Position, BaseScreenSize);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 16, 1); //originally / 32
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 16, 0);


            if (IsActive)
            {
                // Gradually pan to the players when a player dies.
                _camera.Pan();

                //Adjust camera view.

                //THIS SHOULD BECOME ITS OWN CLASS
                //VIEWADJUSTMENT CLASS
                if (_averagePlayerPosition.X > 3600 && _averagePlayerPosition.X < 5500 && _averagePlayerPosition.Y < 1700)
                {
                    if (_camera.Zoom > 0.75f)
                    {
                        _camera.Zoom -= 0.01f;
                        _camera.PlayerYPos += 0.03f;
                    }
                }
                else
                {
                    if (_camera.Zoom < 1.0f)
                    {
                        _camera.Zoom += 0.005f;
                        _camera.PlayerYPos -= 0.03f;
                    }
                }
                //END CLASS


                //Update the animations for the players.
                //Movement of player is done in HandleInput method.
                foreach (GameObject.Player player in playerList)
                {
                    player.Update(gameTime);

                    if (player.IsAlive == false && player.IsReviving == false)
                    {
                        foreach (GameObject.Player medic in playerList)
                        {
                            if (medic.IsAlive & medic.Velocity == Vector2.Zero)
                            {
                                player.Revive(medic);
                            }
                        }
                    }
                }

                foreach (CollisionSurface c in _layers[_layerDictionary["c"]].Sprites)
                    if(c.Colour != Color.Transparent)
                        c.Update(gameTime);

                foreach (Pickup p in _layers[_layerDictionary["interactive"]].Sprites)
                    if (p.Active)
                        p.Update(gameTime);
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            //Sets previous camera position to the last position it was
            //(only when panning is not initiated)
            _camera.PreviousCameraPosition = _averagePlayerPosition;

            _averagePlayerPosition = Vector2.Zero;
            int NumberOfActivePlayers = 0;

            for (int playerIndex = 0; playerIndex < playerList.Count; playerIndex++)
            {

                // Look up inputs for the active player profile.
                // sets the index by default to player one.
                PlayerIndex pI = PlayerIndex.One;

                switch (playerIndex)
                {
                    case 0:
                        pI = PlayerIndex.One;
                        break;
                    case 1:
                        pI = PlayerIndex.Two;
                        break;
                    case 2:
                        pI = PlayerIndex.Three;
                        break;
                    case 3:
                        pI = PlayerIndex.Four;
                        break;
                }

                KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
                GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

                // The game pauses either if the user presses the pause button, or if
                // they unplug the active gamepad. This requires us to keep track of
                // whether a gamepad was ever plugged in, because we don't want to pause
                // on PC if they are playing with a keyboard and have no gamepad at all!
                bool gamePadDisconnected = !gamePadState.IsConnected &&
                                           input.GamePadWasConnected[playerIndex];

                PlayerIndex player;


                //Reset the level
                if (resetAction.Evaluate(input, pI, out player))
                {
                    ResetLevel();
                }


                //Toggle the collision markers on and off
                if (playerIndex == 0)
                {
                    if (toggleCollisionVisibility.Evaluate(input, pI, out player))
                    {
                        foreach (CollisionSurface c in _layers[_layerDictionary["c"]].Sprites)
                        {
                            c.Visible = !c.Visible;
                        }
                    }
                }

                //If the game is paused, add the pause screen
                if (pauseAction.Evaluate(input, pI, out player) || gamePadDisconnected)
                {
                    ScreenManager.AddScreen(new PauseMenuScreen(), pI);
                }
                else
                {
                    if (stunPlayers.Evaluate(input, pI, out player))
                    {
                        playerList[playerIndex].IsStunned = true;
                    }
                    
                    // Otherwise update the player position.
                    playerList[playerIndex].Update(gameTime, input, _layers[_layerDictionary["c"]].Sprites, _layers[_layerDictionary["interactive"]].Sprites);
                  
                }

                //Gets the bounds of the screen to check whether the player is within them
                Rectangle screenBounds = new Rectangle((int)_camera.Position.X, (int)_camera.Position.Y, (int)(BaseScreenSize.X / _camera.Zoom), (int)(BaseScreenSize.Y / _camera.Zoom));

                if (!playerList[playerIndex].BoundingBox().Intersects(screenBounds))
                {

                    //If the player is outside the bounds of the screen and still alive, kill them.
                    if (playerList[playerIndex].IsAlive)
                    {
                        //Initiate the camera pan to the new average position
                        _camera.SlideCamera = true;

                        playerList[playerIndex].IsAlive = false;
                    }
                }

                //If the player is alive, then add their position to the average
                if (playerList[playerIndex].IsAlive)
                {
                    _averagePlayerPosition += playerList[playerIndex].Position;
                    NumberOfActivePlayers++;
                }
            }
            
            //Find the average screen position of all active players (dead players are inactive).
            _averagePlayerPosition.X = _averagePlayerPosition.X / NumberOfActivePlayers;
            _averagePlayerPosition.Y = _averagePlayerPosition.Y / NumberOfActivePlayers;


            //Move the camera to look at this position
            _camera.LookAt(_averagePlayerPosition, BaseScreenSize);

            if (NumberOfActivePlayers == 0)
                ResetLevel();
        }

        /// <summary>
        /// Draws the game statistics
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void drawGameStatistics(GameTime gameTime, SpriteBatch spriteBatch)
        {

            int stringHeight = (int)gameFont.MeasureString("ASTRING").Y;
            //Draws game time
            spriteBatch.DrawString(gameFont, "Elapsed Time: " + gameTime.TotalGameTime.ToString(), new Vector2(10, 10), Color.DarkRed);

            //Draws info about each player
            for (int i = 0; i < playerList.Count; i++)
            {
                spriteBatch.DrawString(gameFont, "Player" + playerList[i].GetPlayerIndex + " Position:" + playerList[i].Position.ToString() + ", Ground: " + playerList[i].IsOnGround.ToString() + ", Falling: " + playerList[i].IsFalling + ", Jumping: " + playerList[i].isJumping + ", Gliding: " + playerList[i].IsGliding + ", isCrouching: " + playerList[i].IsCrouching + ", IsAlive: " + playerList[i].IsAlive + ", Lifeblood: " + playerList[i].LifeBloodCount, new Vector2(10, (10 + stringHeight + (stringHeight * i))), Color.DarkRed);
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.CornflowerBlue, 0, 0);
       
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //Draw all the objects in every layer.
            foreach (GameObject.Layer layer in _layers)
                layer.Draw(spriteBatch, ResolutionScaleMatrix);

            //Begin the spritebatch draw for anything over the top of layers (eg HUD), and take resolution into account.
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, ResolutionScaleMatrix);

            //Draw game statistics.
            drawGameStatistics(gameTime, spriteBatch);
            
            spriteBatch.End();




            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 1.25f); //originally pauseAlpha / 2

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
