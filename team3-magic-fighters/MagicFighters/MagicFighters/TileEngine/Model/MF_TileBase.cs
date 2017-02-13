using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicFighters.TileEngine.Model
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
          public int LiveSpan { get; set; }
          
     }
}
