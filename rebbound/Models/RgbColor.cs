using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rebbound.Models
{
    public class RgbColor
    {
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }
        public string Name { get; private set; }

        public RgbColor(byte r, byte g, byte b)
            : this(r, g, b, string.Empty)
        {
        }

        public RgbColor(byte r, byte g, byte b, string name)
        {
            this.R = r;
            this.G = g;
            this.B = b;

            this.Name = name;
        }
    }
}
