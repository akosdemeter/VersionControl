using GiftFactory.Abstractions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftFactory.Entities
{
    public class PresentFactory: IToyFactory
    {
        public Color BoxColor { get; set; }
        public Color RibbonColor { get; set; }
        public Abstractions.Toy CreateNew()
        {
            return new Present(BoxColor, RibbonColor);
        }
    }
}
