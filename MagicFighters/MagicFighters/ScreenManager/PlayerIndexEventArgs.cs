// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//PlayerIndexEventArgs.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// Custom event argument which includes the index of the player who
    /// triggered the event. This is used by the MenuEntry.Selected event.
    /// </summary>
    class PlayerIndexEventArgs : EventArgs
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }


        /// <summary>
        /// Gets the index of the player who triggered this event.
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        PlayerIndex playerIndex;
    }
}
