using System;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;


namespace MagicFighters.ParticleSystem
{
    public class MF_Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        [XmlIgnore]
        public Texture2D Texture;
        public string TextureName;
        public float RotationRate;
        public float Rotation;
        public float Life;
        public float AlphaRate;
        public float Alpha;
        public float ScaleRate;
        public float Scale;
        public Color Color = Color.White;
    }
}
