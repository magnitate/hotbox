using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DataTypes
{
    public enum MovementPattern
    {
        /// <summary>
        /// When reaches last waypoint, direction reverses
        /// going back to the one it just came from
        /// </summary>
        Reverse = 0,

        /// <summary>
        /// When reaches last waypoint, proceeds to first waypoint
        /// of the loop
        /// </summary>
        Loop = 1,
    }

    public class CollisionMoving : CollisionSurface
    {
        public CollisionMoving()
            : base()
        {
            CollisionType = TileCollision.Moving;
        }

        /// <summary>
        /// The amount to move the platform
        /// </summary>
        public Vector2 PlatformSpeed;

        /// <summary>
        /// The time when 
        /// </summary>
        private double timeSinceLastAct = 0;

        /// <summary>
        /// The pattern of movement the platform will cycle through
        /// </summary>
        public MovementPattern MovementCycle;
        private bool isReverse = false;

        /// <summary>
        /// An array containing the waypoints which the platform will move between
        /// </summary>
        public Waypoint[] Waypoints;

        private int waypointIndex = 0;

        public bool IsAtWaypoint = true;

        /// <summary>
        /// The index of the waypoint the platform is currently at.
        /// </summary>
        public int WaypointIndex
        {
            get 
            { 
                if( waypointIndex < 0 )
                    return 0;
                else
                    return waypointIndex;
            }
            set
            {
                if (waypointIndex >= Waypoints.Length - 1)
                {
                    if (MovementCycle == MovementPattern.Reverse)
                    {
                        waypointIndex = Waypoints.Length - 2;
                        isReverse = true;
                    }
                    else if (MovementCycle == MovementPattern.Loop)
                        waypointIndex = 0;
                }
                else if (waypointIndex < 1)
                {
                    if (MovementCycle == MovementPattern.Reverse)
                    {
                        isReverse = false;    
                    }
                    waypointIndex = 1;
                }
                else
                {
                    if (isReverse)
                        waypointIndex--;
                    else
                        waypointIndex = value;
                }
            }
        }

        /// <summary>
        /// The index of the next waypoint in the sequence.
        /// </summary>
        public int NextWaypointIndex
        {
            get
            {
                if (waypointIndex + 1 >= Waypoints.Length)
                {
                    if (MovementCycle == MovementPattern.Reverse)
                        return waypointIndex - 1;
                    else
                        return 0;
                }
                else
                {
                    if (isReverse && waypointIndex > 0)
                        return waypointIndex - 1;
                    else
                        return waypointIndex + 1;
                }
            }
        }

        /// <summary>
        /// Updates the position of the platform in relation to it's waypoints depending
        /// on the game time.
        /// </summary>
        /// <param name="gameTime">The current GameTime of the game</param>
        public override void Update(GameTime gameTime)
        {
            if (Colour != Color.White)
                Colour = Color.White;

            timeSinceLastAct += gameTime.ElapsedGameTime.TotalSeconds;

            if (gameTime.TotalGameTime.TotalSeconds == 0 && Waypoints.Length > 0)
            {
                Position = Waypoints[WaypointIndex].Position;
            }

            if (IsAtWaypoint)
            {
                if (timeSinceLastAct > Waypoints[WaypointIndex].PauseDuration)
                {
                    PlatformSpeed.X = ((Waypoints[WaypointIndex].Position.X - Waypoints[NextWaypointIndex].Position.X) / Waypoints[WaypointIndex].TravelDuration) * -1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    PlatformSpeed.Y = ((Waypoints[WaypointIndex].Position.Y - Waypoints[NextWaypointIndex].Position.Y) / Waypoints[WaypointIndex].TravelDuration) * -1 * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    WaypointIndex++;
                    IsAtWaypoint = false;
                    timeSinceLastAct = 0;
                }
            }
            else
            {
                if (timeSinceLastAct < Waypoints[WaypointIndex].TravelDuration)
                {
                    Position += PlatformSpeed;
                }
                else
                {
                    IsAtWaypoint = true;
                    timeSinceLastAct = 0;
                }
            }
        }
    }

    public class Waypoint
    {
        /// <summary>
        /// The position of the waypoint
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The amount of time to wait at the waypoint
        /// </summary>
        public float PauseDuration;

        /// <summary>
        /// The amount of time the platform will take to move from this waypoint to the next
        /// </summary>
        public float TravelDuration;
    }
}
