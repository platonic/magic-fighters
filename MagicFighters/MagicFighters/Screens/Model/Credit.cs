// ----------------------------------------------------------------------------------
//Westwood College Project 2012
//MagicFighters Team 3
//Professor: Mark Baldwin
//Copyright 2012 © Westwood College 
// ----------------------------------------------------------------------------------

#region File Description
//-----------------------------------------------------------------------------
//Credit.cs
//Author        : Lisandro Martinez
//Comments      : Optimized by Lisandro Martinez
//Date          : 9/01/2012
//Last Modified : 9/01/2012    By: Lisandro Martinez
//-----------------------------------------------------------------------------
#endregion
using System;
namespace MagicFighters.Screens.Model
{
    public enum FlowDirection
    {
        LeftBottom,
        LeftCenter,
        LeftTop,

        TopLeft,
        TopCenter,
        TopRight,

        RightTop,
        RightCenter,
        RightBottom,
        
        BottomLeft,
        BottomCenter,
        BottomRight,


    }
    public class Credit
    {
        public Microsoft.Xna.Framework.Vector2 Position { get; set; }
        public string Name { get; set; }
        public string Comments { get; set; }
        public string Title { get; set; }
        public FlowDirection FlowDirection { get; set; }
        public bool isRotating { get; set; }

        public Credit() { }
        public Credit(Microsoft.Xna.Framework.Vector2 Position,string Comments, string Name, string Title, FlowDirection FlowDirection, bool isRotating) 
        {
            this.FlowDirection = FlowDirection;
            this.isRotating = isRotating;
            this.Name = Name;
            this.Position = Position;
            this.Title = Title;
            this.Comments = Comments;
            
        }
    }
}
