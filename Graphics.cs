using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace chip_8
{
    class Graphics
    {
        Graphics[,] pixel;
        Graphics(ref Rectangle[,] myRect)
        {
           ref pixel = ref myRect;
        }
        public void setpixel(int x, int y, bool state, ref Rectangle[,] pixel)
        {
            if (pixel[x, y].Fill == Brushes.White && state == false)
            {
                pixel[x, y].Fill = Brushes.White;
            }
            else if (pixel[x, y].Fill == Brushes.White && state == true)
            {
                pixel[x, y].Fill = Brushes.Black;
            }
            else if (pixel[x, y].Fill == Brushes.Black && state == false)
            {
                pixel[x, y].Fill = Brushes.Black;
            }
            else if (pixel[x, y].Fill == Brushes.Black && state == true)
            {
                pixel[x, y].Fill = Brushes.White;
            }
        }

    }

}
