using System;

namespace MagicFighters.Model
{
     public class MF_TileBase
     {
          public int ID { get; set; }
          public string Name { get; set; }
          public string Description { get; set; }
          public bool isVisible { get; set; }
          /// <summary>
          /// 0 = forever in seconds
          /// </summary>
          public int LifeSpan { get; set; }
          
     }
}
