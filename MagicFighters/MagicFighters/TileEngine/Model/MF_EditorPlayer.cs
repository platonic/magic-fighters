using System;

namespace MagicFighters.Model
{
    public class MF_EditorPlayer 
    {

        /// <summary>
        /// Defines the path
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Defines the PlayerType
        /// </summary>
        public string PlayerType { get; set; }
        /// <summary>
        /// Defines the PlayerType
        /// </summary>
        public string AIType { get; set; }
        /// <summary>
        /// Defines if the player is alive
        /// </summary>
        public bool IsAlive { get; set; }
        
        /// <summary>
        /// Defines if the player is active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Player's Magic Points
        /// Decreases when a skill is used
        /// </summary>
        public float MP { get; set; }
        /// <summary>
        /// Player's Hit Points
        /// Decrease every time the player receives damage
        /// </summary>
        public float HP { get; set; }
        /// <summary>
        /// Player scores when an enemy is defeated
        /// </summary>
        public float Score { get; set; }
  
        /// <summary>
        /// How much pull will the player have when jumping
        /// </summary>
        public float JumpGravity { get; set; }

        /// <summary>
        /// Defines if the player animation is running or not
        /// </summary>
        public bool isAnimationRunning { get; set; }

        /// <summary>
        /// Defines if the player is jumping
        /// </summary>
        public bool isPlayerJumping { get; set; }

        /// <summary>
        /// The player speed
        /// </summary>
        public MF_TileVector2 Speed { get; set; }

        /// <summary>
        /// The player position if does not have animations
        /// </summary>
        public MF_TileVector2 Position { get; set; }

        /// <summary>
        /// The player position if does not have animations
        /// </summary>
        public MF_TileVector2 StartingOffset { get; set; }

        /// <summary>
        /// Player's animations
        /// </summary>
        public MF_EditorAnimation [] Animations { get; set; }
        /// <summary>
        /// The name of the player
        /// </summary>
        public string Name { get; set; }
        public MF_Size Size { get; set; }

    }
}
