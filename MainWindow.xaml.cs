using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
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
            CompositionTarget.Rendering += GameLoop;
            InitializeComponent();
            CreateDrawingVisualRectangle();
            internals.Rect = pixel;
            internals.printtoString(1);
        }
        int x = 0;
        int y = 0;
        Internals internals = new Internals();
        void GameLoop(object Sender, EventArgs e)
        {
           internals.decode();
        }
        Rectangle[,]? pixel = new Rectangle[64, 32];
    
        private void CreateDrawingVisualRectangle()
        {
            for (int y = 0; y < 32; y++)
            { 
                 mygrid.RowDefinitions.Add(new RowDefinition() { });
            }
            for (int x = 0; x < 64; x++)
            {
                    mygrid.ColumnDefinitions.Add(new ColumnDefinition() { });

            }
            for (int py = 0; py < 32;py++)
            {
                for (int px = 0; px < 64; px++)
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
