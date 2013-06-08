using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
using DataTypes;

namespace Hotbox.GameObject
{
    public class Player : DataTypes.SpriteAnimation
    {
        PlayerIndex playerIndex;

        public PlayerIndex GetPlayerIndex
        {
            get { return playerIndex; }
        }

        InputAction moveLeft;
        InputAction moveRight;
        InputAction jump;
        InputAction sprint;
        InputAction crouch;

        //This class is used to keep track of which player
        //is acting as the medic for the revive
        Player theMedic;

        private float previousBottom;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        /// <summary>
        /// Constants for controling horizontal movement
        /// </summary>
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.58f; //default 0.48f
        private const float AirDragFactor = 0.58f;
        private const float SprintBoostFactor = 1.15f;
        private const float CrouchSlowFactor = 0.5f;
        private const float SprintCrouchSlowFactor = 0.8f;

        /// <summary>
        /// Constants for controlling vertical movement
        /// </summary>
        private const float MaxJumpTime = 0.45f;
        private const float JumpLaunchVelocity = -4500.0f;
        private const float GravityAcceleration = 4500.0f;
        private const float MaxFallSpeed = 1350.0f;
        private const float JumpControlPower = 0.14f;
        private const float MaxGlideTime = 5.0f;
        private const float GlideFallFactor = 0.5f;
        private const float GlideMoveFactor = 0.65f;

        /// <summary>
        /// Input configuration
        /// </summary>
        private const float MoveStickScale = 1.0f;
        private const float AccelerometerScale = 1.5f;

        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        private bool isOnGround;

        /// <summary>
        /// Check variables for the different player states
        /// </summary>
        private float movement;
        private bool isSprinting;
        private bool isCrouching;
        private bool isSliding;
        private bool isGliding;
        private bool isBouncing = false;
        private bool isAlive = true;
        private bool isReviving = false;

        /// <summary>
        /// The check variables for jumping
        /// </summary>
        private bool wantsToJump;
        public bool isJumping;
        private bool wasJumping;
        private float jumpTime;

        /// <summary>
        /// Control variables for bouncing
        /// </summary>
        private bool wasBouncing;
        private bool bounceHorizontal;
        private const float MaxBounceTime = 0.65f;
        private float bounceTime = 0.0f;
        //These will be set by the Bounce collision surface
        private float BounceLaunchVelocityY = 0.0f;
        private float BounceLaunchVelocityX = 0.0f;

        /// <summary>
        /// 
        /// </summary>
        private float SlideMoveFactor = 0.0f;
        private float SlideDirection = 0.0f;

        /// <summary>
        /// Checks for if the player is currently alive
        /// </summary>
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        /// <summary>
        /// Check to see if the player is being revived
        /// </summary>
        public bool IsReviving
        {
            get { return isReviving; }
            set { isReviving = value; }
        }

        /// <summary>
        /// Checks to see if the player is crouching
        /// </summary>
        public bool IsCrouching
        {
            get { return isCrouching; }
        }

        /// <summary>
        /// Checks to see if the player is bouncing
        /// </summary>
        public bool IsBouncing
        {
            get { return isBouncing; }
        }


        /// <summary>
        /// Checks if the player is gliding
        /// </summary>
        private float glideTime;
        public bool IsGliding
        {
            get
            {
                if (!IsOnGround)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Check to see whether the player is falling
        /// </summary>
        public bool IsFalling
        {
            get
            {
                if (!IsOnGround & (jumpTime == 0.0f | (jumpTime == 0.0f & glideTime == 0.0f)))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Keeps track of whether the player is sliding down a
        /// wall or not
        /// </summary>
        private bool isOnWall = false;
        private const float OnWallSlowFactor = 0.5f;
        public bool IsOnWall
        {
            get { return isOnWall; }
            set { isOnWall = value; }
        }


        /// <summary>
        /// Stun state control variables and check
        /// </summary>
        private const float MaxStunTime = 1.0f;
        private float stunTime = 0.0f;
        private bool isStunned = false;
        public bool IsStunned
        {
            get
            {
                if (isStunned == true & stunTime < MaxStunTime)
                    stunTime += 0.01f;
                else if (stunTime > MaxStunTime)
                    IsStunned = false;

                return isStunned;
            }
            set
            {
                isStunned = value;
                stunTime = 0.0f;
            }
        }

        /// <summary>
        /// Keeps track of the amount of lifeblood a player picks
        /// up during the level
        /// </summary>
        private int lifebloodCount = 0;
        public int LifeBloodCount
        {
            get { return lifebloodCount; }
        }

        /// <summary>
        /// The control variables for reviving
        /// </summary>
        const float ReviveTimeDefault = 2.0f;
        float reviveTimer = ReviveTimeDefault;

        public void Revive(GameObject.Player medic)
        {
            theMedic = medic;
            reviveTimer = ReviveTimeDefault;
            IsReviving = true;
        }

        public override Rectangle BoundingBox()
        {
            //if( isCrouching)
            //    return new Rectangle((int)Position.X, (int)Position.Y + (height / 2), width, height / 2);
            //else
                return new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }


        public Player(PlayerIndex thePlayerIndex, string theAssetName, int frames, int animations)
            : base(frames, animations)
        {
            playerIndex = thePlayerIndex;
            AssetName = theAssetName;

            moveLeft = new InputAction(
                new Buttons[] { Buttons.DPadLeft },
                new Keys[] { Keys.Left, Keys.A },
                false);

            moveRight = new InputAction(
                new Buttons[] { Buttons.DPadRight },
                new Keys[] { Keys.Right, Keys.D },
                false);

            jump = new InputAction(
                new Buttons[] { Buttons.A },
                new Keys[] { Keys.Space, Keys.Up },
                false);

            sprint = new InputAction(
                new Buttons[] { Buttons.RightTrigger },
                new Keys[] { Keys.LeftShift },
                false);

            crouch = new InputAction(
                new Buttons[] { Buttons.RightShoulder },
                new Keys[] { Keys.LeftControl },
                false);
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager theContentManager, string theAssetName)
        {
            base.LoadContent(theContentManager, theAssetName);

            AnimationClass anim = new AnimationClass();
            anim.Colour = Colour;

            AddAnimation("LeftIdle", 1, 4, anim.Copy());
            AddAnimation("Left", 2, 8, anim.Copy());
            AddAnimation("LeftJump", 3, 9, anim.Copy());
            
            anim.SpriteEffect = SpriteEffects.FlipHorizontally;
            
            AddAnimation("RightIdle", 1, 4, anim.Copy());
            AddAnimation("Right", 2, 8, anim.Copy());
            AddAnimation("RightJump", 3, 9, anim.Copy());

            Animation = "RightIdle";
        }

        public void Update(GameTime gameTime, InputState input, List<Sprite> level, List<Sprite> pickups)
        {
            if (IsAlive)
            {
                base.Update(gameTime);

                GetInput(gameTime, input);

                ApplyPhysics(gameTime, level);

                GetPickups(pickups);

                //clear input
                movement = 0.0f;
                //isJumping = false;
                //isGliding = false;
            }
            else if (IsReviving)
            {
                if (reviveTimer > 0.0f & theMedic.Velocity == Vector2.Zero)
                {
                    reviveTimer -= 1f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (reviveTimer < 0.0f)
                {
                    Position = new Vector2(theMedic.Position.X, theMedic.Position.Y - height);
                    IsAlive = true;
                    IsReviving = false;
                }
                else
                {
                    IsReviving = false;
                }
            }
        }

        /// <summary>
        /// Gets player horizontal movement and jump commands from input.
        /// </summary>
        private void GetInput(GameTime gameTime, InputState input)
        {          
            int thisPlayer = (int)playerIndex;
            GamePadState gamePadState = input.CurrentGamePadStates[thisPlayer];

            if (!IsStunned)
            {
                //Get analog horizontal movement.
                movement = gamePadState.ThumbSticks.Left.X * MoveStickScale;

                //Ignore small movements to prevent running in place.
                if (Math.Abs(movement) < 0.5f)
                    movement = 0.0f;


                // If any digital horizontal movement input is found, override the analog movement.
                PlayerIndex player;

                if (moveLeft.Evaluate(input, playerIndex, out player))
                {
                    movement = -1.0f;
                }
                else if (moveRight.Evaluate(input, playerIndex, out player))
                {
                    movement = 1.0f;
                }


                //Change animation based on movement input.
                if (movement < 0)
                {
                    if (Animation != "Left")
                        Animation = "Left";
                }
                else if (movement > 0)
                {
                    if (Animation != "Right")
                        Animation = "Right";
                }
                else
                {
                    if (!Animation.Contains("Idle"))
                    {
                        if (Animation.Contains("Left"))
                            Animation = "LeftIdle";
                        else
                            Animation = "RightIdle";
                    }
                }

                //if( !apexReached)
                wantsToJump = jump.Evaluate(input, playerIndex, out player);

                isSprinting = sprint.Evaluate(input, playerIndex, out player);
                isCrouching = crouch.Evaluate(input, playerIndex, out player);

                /*if (!wasJumping)
                {
                    if ((isJumping && jumpTime == 0.0f) && !IsOnGround)
                        isGliding = true;
                    else
                        isGliding = false;
                }*/

                //if( IsFalling & !wasJumping & !IsOnGround)
                //isGliding = jump.Evaluate(input, playerIndex, out player);
            }
        }

        /// <summary>
        /// Updates the player's velocity and position based on input, gravity, etc.
        /// </summary>
        private void ApplyPhysics(GameTime gameTime, List<Sprite> level)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            // Base velocity is a combination of horizontal movement control and
            // acceleration downward due to gravity.
            velocity.X += movement * MoveAcceleration * elapsed;

            if (isSliding)
                velocity.X += SlideDirection * MoveAcceleration * elapsed * SlideMoveFactor;

            if (isSprinting && isCrouching)
                velocity.X *= SprintCrouchSlowFactor;
            else if (isSprinting)
                velocity.X *= SprintBoostFactor;
            else if (isCrouching)
                velocity.X *= CrouchSlowFactor;

            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * elapsed, -MaxFallSpeed, MaxFallSpeed);

            velocity.Y = DoJump(velocity.Y, gameTime);

            velocity = DoGlide(velocity, gameTime);

            velocity.Y = DoBounceY(velocity.Y, gameTime);

            velocity.X = DoBounceX(velocity.X, gameTime);

            //Apply pseudo-drag horizontally.
            if (IsOnGround)
                velocity.X *= GroundDragFactor;
            else
                velocity.X *= AirDragFactor;

            //Prevent the player from running faster than his top speed.
            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            if (IsOnWall && IsFalling & !IsBouncing)
                velocity.Y *= OnWallSlowFactor;

            //Apply velocity.
            Position += velocity * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            IsOnWall = false;
            isSliding = false;

            // If the player is now colliding with the level, separate them.
            HandleCollisions(level);

            //If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == previousPosition.X)
                velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        /// <summary>
        /// Calculates the Y velocity accounting for jumping and
        /// animates accordingly.
        /// </summary>
        /// <remarks>
        /// During the accent of a jump, the Y velocity is completely
        /// overridden by a power curve. During the decent, gravity takes
        /// over. The jump velocity is controlled by the jumpTime field
        /// which measures time into the accent of the current jump.
        /// </remarks>
        /// <param name="velocityY">
        /// The player's current velocity along the Y axis.
        /// </param>
        /// <returns>
        /// A new Y velocity if beginning or continuing a jump.
        /// Otherwise, the existing Y velocity.
        /// </returns>
        private float DoJump(float velocityY, GameTime gameTime)
        {
            // If the player wants to jump
            if (wantsToJump)
            {
                // Begin or continue a jump
                if ((!wasJumping && (IsOnGround | IsOnWall)) || jumpTime > 0.0f)
                {
                    //if (jumpTime == 0.0f)
                    //    jumpSound.Play();
                    Rotation = 0.0f;

                    isJumping = true;
                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                    if (Animation.Contains("Left"))
                        Animation = "LeftJump";
                    else if (Animation.Contains("Right"))
                        Animation = "RightJump";

                }

                // If we are in the ascent of the jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                    isJumping = false;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                jumpTime = 0.0f;
                isJumping = false;
            }
            wasJumping = isJumping;

            return velocityY;
        }

        private float DoBounceY(float theVelocity, GameTime gameTime)
        {
            if (isBouncing)
            {
                // Begin or continue a jump
                if (!wasBouncing || bounceTime > 0.0f)
                {
                    bounceTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                // If we are in the ascent of the jump
                if (0.0f < bounceTime && bounceTime <= MaxBounceTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    theVelocity = BounceLaunchVelocityY * (1.0f - (float)Math.Pow(bounceTime / MaxBounceTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    bounceTime = 0.0f;
                    isBouncing = false;
                }
            }
            else
            {
                // Continues not jumping or cancels a jump in progress
                bounceTime = 0.0f;
                isBouncing = false;
            }
            wasBouncing = isBouncing;

            return theVelocity;
        }
        
        private float DoBounceX(float theVelocity, GameTime gameTime)
        {
            if (isBouncing)
            {
                bounceHorizontal = true;
            }

            if (IsOnGround)
                bounceHorizontal = false;

            if (bounceHorizontal)
            {
                theVelocity = BounceLaunchVelocityX;
            }

            return theVelocity;
        }

        /// <summary>
        /// Applies a slow factor to the horizontal and vertical movement
        /// of the player if wanting to glide
        /// </summary>
        /// <remarks>
        /// During the glide, the player is slowed by the factors defined
        /// for falling and moving on the x axis. When the time runs out the
        /// player falls due to gravity, or has hit the ground
        /// </remarks>
        /// <param name="tempVelocity">
        /// The player's current velocity along the Y axis and X axis.
        /// </param>
        /// <returns>
        /// A new velocity if beginning or continuing a glide.
        /// Otherwise, the existing velocity.
        /// </returns>
        private Vector2 DoGlide(Vector2 tempVelocity, GameTime gameTime)
        {
            // If the player wants to glide
            if (isGliding)
            {
                if (IsFalling)
                {
                    glideTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    isJumping = false;
                }

                // If we are in the middle of a glide
                if (0.1f < glideTime & glideTime <= MaxGlideTime)
                {
                    // Slow the player to 
                    tempVelocity *= new Vector2(GlideMoveFactor, GlideFallFactor);
                }
                else if(glideTime > MaxGlideTime)
                {
                    // Reached the duration of the glide
                    glideTime = 0.0f;
                }
            }
            else
            {
                // Continues not gliding or cancels a glide in progress
                glideTime = 0.0f;
            }

            return tempVelocity;
        }

        /// <summary>
        /// Detects and resolves all collisions between the player and his neighboring
        /// tiles. When a collision is detected, the player is pushed away along one
        /// axis to prevent overlapping. There is some special logic for the Y axis to
        /// handle platforms which behave differently depending on direction of movement.
        /// </summary>
        private void HandleCollisions(List<Sprite> level)
        {
            Rectangle bounds = BoundingBox();

            //Reset flag to search for ground collision.
            isOnGround = false;

            foreach (CollisionSurface tile in level)
            {
                // If this tile is collidable,
                if (this.BoundingBox().Intersects(tile.BoundingBox()))
                {
                    Rectangle tileBounds = tile.BoundingBox();

                    //Sets the colour to a dark blue when you are colliding with the surface
                    if( tile.Colour != Color.Transparent)
                        tile.Colour = Color.DarkBlue;

                    if (tile.CollisionType == TileCollision.Impassable || tile.CollisionType == TileCollision.Platform || tile.CollisionType == TileCollision.Conveyor)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            //Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX || tile.CollisionType == TileCollision.Platform)
                            {
                                //If we crossed the top of a tile, we are on the ground.
                                if (previousBottom <= tileBounds.Top)
                                {
                                    isOnGround = true;
                                    isJumping = false;
                                    isGliding = false;

                                    if (tile.CollisionType == TileCollision.Conveyor)
                                    {
                                        if (movement != 0)
                                            SlideDirection = movement;

                                        SlideMoveFactor = tile.SlideBoost;
                                    }
                                }

                                //Ignore platforms, unless we are on the ground.
                                if (IsOnGround)
                                {
                                    // Resolve the collision along the Y axis.
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);
                                    
                                    Rotation = 0.0f;

                                    if (tile.CollisionType == TileCollision.Conveyor)
                                        isSliding = true;

                                    //Perform further collisions with the new bounds.
                                    bounds = BoundingBox();
                                }
                            }
                            else if (tile.CollisionType == TileCollision.Impassable)
                            {
                                // Resolve the collision along the X axis
                                Position = new Vector2(Position.X + (depth.X - 5), Position.Y);

                                IsOnWall = true;

                                //Perform further collisions with the new bounds.
                                bounds = BoundingBox();
                            }
                        }
                    }
                    else if (tile.CollisionType == TileCollision.Slope)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        float displacement = RectangleExtensions.GetSlopePosition(bounds, tileBounds, tile.BottomSlopePoint);

                        //If we crossed the top of a tile, we are on the ground.
                        if (previousBottom >= displacement - (this.BoundingBox().Height / 2))
                        {
                            isOnGround = true;
                            isJumping = false;
                            isGliding = false;
                        }

                        //Ignore platforms, unless we are on the ground.
                        if ((IsOnGround & !wantsToJump) | (IsOnGround & isJumping) | (IsOnGround & isGliding))
                        {
                            // Resolve the collision along the Y axis.
                            Position = new Vector2(Position.X, displacement - this.BoundingBox().Height + 1);

                            Rotation = RectangleExtensions.GetSlopeAngle(tileBounds, tile.BottomSlopePoint) / 2;

                            //Perform further collisions with the new bounds.
                            bounds = BoundingBox();
                        }
                    }
                    else if (tile.CollisionType == TileCollision.Bounce)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Vector2 depth = RectangleExtensions.GetIntersectionDepth(bounds, tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            //Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX || tile.CollisionType == TileCollision.Platform)
                            {
                                //If we crossed the top of a tile, we are now bouncing.
                                if (previousBottom <= tileBounds.Top)
                                {
                                    isBouncing = true;
                                    BounceLaunchVelocityX = tile.BounceVelocityX;
                                    BounceLaunchVelocityY = tile.BounceVelocityY;
                                }

                                //Ignore platforms, unless we are bouncing
                                if (IsBouncing)
                                {
                                    // Resolve the collision along the Y axis.
                                    Position = new Vector2(Position.X, Position.Y + depth.Y);

                                    Rotation = 0.0f;

                                    //Perform further collisions with the new bounds.
                                    bounds = BoundingBox();
                                }
                            }
                            else
                            {
                                // Resolve the collision along the X axis
                                Position = new Vector2(Position.X + depth.X, Position.Y);

                                //Perform further collisions with the new bounds.
                                bounds = BoundingBox();
                            }
                        }
                    }
                    else if (tile.CollisionType == TileCollision.Slide)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        float displacement = RectangleExtensions.GetSlopePosition(bounds, tileBounds, tile.BottomSlopePoint);

                        //If we crossed the top of a tile, we are on the ground.
                        if (previousBottom >= displacement - (this.BoundingBox().Height / 2))
                        {
                            isOnGround = true;
                            isJumping = false;
                            isGliding = false;
                            
                            SlideDirection = tile.SlideDirection;
                            SlideMoveFactor = tile.SlideBoost;
                        }

                        //Ignore platforms, unless we are on the ground.
                        if ((IsOnGround & !wantsToJump) | (IsOnGround & isJumping) | (IsOnGround & isGliding))
                        {
                            // Resolve the collision along the Y axis.
                            Position = new Vector2(Position.X, displacement - this.BoundingBox().Height + 1);

                            isSliding = true;

                            Rotation = RectangleExtensions.GetSlopeAngle(tileBounds, tile.BottomSlopePoint) / 2;

                            //Perform further collisions with the new bounds.
                            bounds = BoundingBox();
                        }
                    }
                }
            }

            //Save the new bounds bottom.
            previousBottom = bounds.Bottom;
        }

        private void GetPickups(List<Sprite> pickups)
        {
            foreach (Pickup p in pickups)
            {
                if(this.BoundingBox().Intersects(p.BoundingBox()))
                {
                    if (p.Active)
                    {
                        p.Active = false;
                        lifebloodCount += p.Value;
                        AudioManager.PlaySfxCue("Bing");
                    }
                }
            }
        }

        public void Reset()
        {
            Position = new Vector2(500, 1750);
            IsAlive = true;
            lifebloodCount = 0;
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {   
            if( IsAlive)
                theSpriteBatch.Draw(mSpriteTexture, Position, Animations[Animation].Rectangles[FrameIndex], Animations[Animation].Colour, Rotation, Origin, Scale, Animations[Animation].SpriteEffect, 0f);
        }
    }
}
