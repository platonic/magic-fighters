// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//MF_StateMachine.cs
//Author        : Lisandro Martinez
//Comments      : Not a real state machine
//Date          : 9/01/2012
//Last Modified : 9/27/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;


namespace MagicFighters.GameStates
{
    public enum MF_GameStates
    {
        Initialize,
        FightMode,
        Playing,
        LostLife,
        LevelOver,
        StoryMode,
        GameOver,
    }
    
    public enum MF_PlayerStates
    {
        None,

        //Visibility state
        Alive,
        Dead,
        Active,
        Inactive,


        //Falling
        Fall,
        FallR,
        FallL,

        //Idle
        Idle,
        IdleL,
        IdleR,

        //Walking
        Walk,
        WalkD,
        WalkU,
        WalkR,
        WalkL,

        //Jumping
        Jump,
        JumpR,
        JumpL,
        JumpFW,
        JumpBW,
        JumpInPlace,
        JumpUp,
        JumpUpL,
        JumpUpR,
        JumpDown,
        JumpDownL,
        JumpDownR,

        //Crouch
        Crouch,
        CrouchL,
        CrouchR,

        //Punch
        PunchLH,//left high
        PunchRH,//right high

        //Blanka Electrocute
        Electrocute,
        ElectrocuteR,
        ElectrocuteL,

        //Getting hit animation
        Hit,
        HitR,
        HitL,

        //Especial moves
        Fireball,
        FireballR,
        FireballL,

        //Bots bomb drop
        dropBomb,
    }
}
