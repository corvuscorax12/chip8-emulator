using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace chip_8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateDrawingVisualRectangle();
            Internals internals = new Internals();
            internals.printtoString(1);
        }
        Rectangle[,]? pixel = new Rectangle[64, 32];
       void setpixel(int x, int y, bool state)
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
        private void CreateDrawingVisualRectangle()
        {
            for (int y = 0; y < 32; y++)
            { 
                 mygrid.RowDefinitions.Add(new RowDefinition() { });
            }
            for (int x = 0; x < 62; x++)
            {
                    mygrid.ColumnDefinitions.Add(new ColumnDefinition() { });

            }
            for (int py = 0; py < 32;py++)
            {
                for (int px = 0; px < 62; px++)
                {
                    pixel[px, py] = new Rectangle();
                    pixel[px, py].Fill = Brushes.White;
                    mygrid.Children.Add(pixel[px,py]);
                    Grid.SetRow(pixel[px,py], py);
                    Grid.SetColumn(pixel[px, py],px);
                   
                }
            }
           

        }
    }
}
