using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace BFForever
{
    public class XPR2
    {
        private const int MAGIC_XPR2 = 1481658930;
        private const int MAGIC_TX2D = 1415066180;

        private Bitmap[] images;

        private XPR2()
        {

        }

        public XPR2 FromFile(string path)
        {
            if (!File.Exists(path)) return null;
            XPR2 xpr = new XPR2();

            // Opens .xpr texture
            using (AwesomeReader ar = new AwesomeReader(File.OpenRead(path), true))
            {
                if (ar.ReadInt32() != MAGIC_XPR2) return null;

                ar.ReadInt32(); // Always 2048?

                int texSize = ar.ReadInt32();
                images = new Bitmap[ar.ReadInt32()];


            }

            return xpr;
        }
    }
}
