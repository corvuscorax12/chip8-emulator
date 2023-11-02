using System;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Windows.Media;
using System.Windows.Controls;
using System.IO;
using System.Collections;

namespace chip_8
{
    internal class Internals
    {
        Rectangle[,] pixel;
        byte[] memory =  new byte[4096];
        byte[] fmemory = File.ReadAllBytes("C:\\Users\\aldawson\\Source\\Repos\\corvuscorax12\\chip8-emulator\\ibm.ch8");
        public Rectangle[,] Rect
        {
            set { pixel = value; }
                }
        public Internals()
        {
            int i = 0;
            foreach (var item in fmemory)
            {
      
                memory[i + 512] = item;
                i++;
            }
        }
        int IR = 512;
        int PC = 512;
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
                case 0x6000:
                    _6XNN(opCode);
                    tick();
                    break;
                case 0x7000:
                    _7XNN(opCode);
                    tick();
                    break;
                case 0xA000:
                    ANNN(opCode);
                    tick();
                    break;
                default:
                    break;
            }
        }
        void ANNN(int opCode)
        {
            int addr = (opCode & 0x0FFF);
            IR = addr;
        }
        void JMP(int opCode)
        {
            int addr = (opCode & 0x0FFF);
            PC = addr;
        }
        void _6XNN(int opCode)
        {
            int regX = (opCode & 0x0F00) >> 12;
            int value = (opCode & 0x00FF);
            regs[regX] = value;
        }
        void _7XNN(int opCode)
        {
            int regX = (opCode & 0x0F00) >> 12;
            int value = (opCode & 0x00FF);
            regs[regX] += value;
        }
        public static Boolean GetBitX(byte bytes, int x)
        {
            var index = x / 8;
            var bit = x - index * 8;

            return (bytes & (1 << bit)) != 0;
        }
        void DXYN(int opCode)
        {
            int X = (opCode & 0x0F00) >> 8;
            int Y = (opCode & 0x00F0) >> 4;
            int regX = regs[X];
            int regY = regs[Y];
            int Nibble = (opCode & 0x000F);
            regs[15] = 0;
            for (int n = 0; n < Nibble; n++)
            {
            int yCoord = regY & 31;
                    int bit = 8;
              for (int i = 0; i < 8; i++)
                {
                    int xCoord = regX & 63;

                    if (!GetBitX(memory[IR + n], bit))
                    {
                        setpixel(xCoord + i, yCoord+n, false);
                    }
                    else if (GetBitX(memory[IR + n], bit))
                    {
                        setpixel(xCoord + i, yCoord+n, true);
                    }
                    bit--;
                    if (xCoord == 32 -1)
                    {
                        break;
                    }
                   
                }
                if (yCoord == 64 -1)
                {
                    break;
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
        public void setpixel(int x, int y, bool state)
        {
          
            if (!state)
            {
                pixel[x, y].Fill = Brushes.White;
            }
            else if (state)
            {
                pixel[x, y].Fill = Brushes.Black;
            }
          
        }
    }
}