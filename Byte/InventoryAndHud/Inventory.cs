using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.InventoryAndHud
{
    internal class Inventory
    {
        public int Rupees { get; set; } = 0;
        public int Keys { get; set; } = 0;
        public int Bombs { get; set; } = 0;
        public int Hearts { get; set; } = 0;

        public bool HasBoomerang { get; set; } = false;
        public bool HasBow { get; set; } = false;
        public bool HasArrow { get; set; } = false;

    }
}
