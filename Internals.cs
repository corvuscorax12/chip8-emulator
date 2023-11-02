using System;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Windows.Media;
using System.Windows.Controls;

namespace chip_8
{
    internal class Internals
    {
        Rectangle[,] pixel;
        public Rectangle[,] Rect
        {
            set { pixel = value; }
                }
        public Internals()
        {
            memory[0] = 0xd1;
            memory[1] = 0x03;
        }
        byte[] memory =  new byte[4096];
        int IR = 0;
        int PC = 0;
        int[] regs = new int[16];
        public void tick()
        {
            PC += 2;
        }
        
        public void decode()
        {
            var address1 = memory[PC] << 8;
            var address2 = memory[PC+ 1];
            var opCode = address1 + address2;
            Console.WriteLine("0x{0:X4}",opCode);
            switch (opCode & 0xF000)
            {
                case 0x0000:
                    switch (opCode & 0x00F0)
                    {
                        case 0x00E0:
                            clearPixels();
                            tick();
                            break;
                        default:
                            break;
                    }
                    break;
                case 0xD000:
                    DXYN(opCode);
                    tick();
                    break;
                case 0x1000:
                    JMP(opCode);
                    break;
                default:
                    break;
            }
        }
        void JMP(int opCode)
        {
            int addr = (opCode & 0x0FFF);
            IR = addr;
        }
        void _6XNN(int opCode)
        {
            int regX = (opCode & 0x0F00) >> 16;
            int value = (opCode & 0x00FF);
            regs[regX] = value;
        }
        void DXYN(int opCode)
        {
            int regX = regs[(opCode & 0x0F00) >> 16];
            int regY = regs[(opCode & 0x00F0) >> 8];
            int Nibble = (opCode & 0x000F);
            byte pixels = 0b00001010;
            for (int i = 0; i < sizeof(byte); i++)
            {
                int bPixels = (pixels >> i) & 0b000000001;
                if (bPixels == 0)
                {
                    setpixel(regX, regY, false);
                }
                if (bPixels == 1)
                {
                    setpixel(regX, regY, true);
                }
            }
            
        }
        public void printtoString(int ir)
        {
            IR=ir;
            var address1 = memory[IR]<<8;
            var address2 = memory[IR + 1];
            var hexValue = address1 + address2;
            Console.WriteLine("{0:X}",hexValue);
        }
        private void clearPixels()
        {
           
                for (int py = 0; py < 32; py++)
                {
                    for (int px = 0; px < 64; px++)
                    {
                        pixel[px, py].Fill = Brushes.White;                      
                    }
                }
            
        }
        private void setpixel(int x, int y, bool state)
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