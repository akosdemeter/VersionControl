using GiftFactory.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GiftFactory.Entities
{
    public class Present: Toy
    {
        public SolidBrush BoxColor { get; private set; }
        public SolidBrush RibbonColor { get; private set; }
        public Present(Color boxcolor, Color ribboncolor)
        {
            BoxColor = new SolidBrush(boxcolor);
            RibbonColor = new SolidBrush(ribboncolor);
        }
        protected override void DrawImage(Graphics g)
        {
            g.FillRectangle(BoxColor, 0, 0, Width, Height);
            g.FillRectangle(RibbonColor, 0, Height*2/5, Width, Height/5);
            g.FillRectangle(RibbonColor, Width*2/5, 0, Width/5, Height);
        }
    }
}
