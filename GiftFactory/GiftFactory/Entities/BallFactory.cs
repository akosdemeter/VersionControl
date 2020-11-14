using GiftFactory.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftFactory.Entities
{
    public class BallFactory: IToyFactory
    {
        public Abstractions.Toy CreateNew()
        {
            return new Ball();
        }
    }
}
