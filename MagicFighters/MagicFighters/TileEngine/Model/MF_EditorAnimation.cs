using System;


namespace MagicFighters.Model
{

    public class MF_EditorAnimation
    {
        public string TexturePath { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float FrameLength { get; set; }
        public bool isFlipped { get; set; }
    }
}
