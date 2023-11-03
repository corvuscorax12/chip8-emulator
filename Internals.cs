using System;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Windows.Media;
using System.Windows.Controls;
using System.IO;
using System.Collections;
using static System.Formats.Asn1.AsnWriter;
using System.Threading;
using System.Windows.Documents;
using System.Reflection.Emit;

namespace chip_8
{
    internal class Internals
    {
        Rectangle[,] pixel;
        byte[] memory = new byte[4096];
        byte[] fmemory = File.ReadAllBytes("C:\\Users\\scoop\\Source\\Repos\\chip8-emulators\\test2.ch8");
        byte delayTimer;
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
       byte[] regs = new byte[16];
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
            if (delayTimer > 0)
            {
                delayTimer -= 1;
            }
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
                case 0x3000:
                       _3XNN(opCode);
                    break;
                case 0x4000:
                    _4XNN(opCode);
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
                case 0x8000:
                    _8000(opCode);
                    tick();
                    break;
                case 0xF000:
                    switch (opCode &  0x00FF)
                    {
                        case 0x55:
                            for (int i = 0; i <= ((opCode & 0x0F00) >> 8);i++)
                            {
                                memory[IR + i] = regs[i];
                            }
                        break;
                        default:
                            break;
                    }
                    tick();
                break;
                default:
                    tick();
                    break;
            }
        }
        void _FX07(int opCode)
        {
            int VX = (opCode & 0x0f00) >> 8;
            regs[VX] = delayTimer;
        }
        void _8000(int opCode)
        {
                    int VX = (opCode & 0x0f00) >> 8;
                    int VY = (opCode & 0x00f0) >> 4;
                     int i = (opCode & 0x000f);
            switch (i)
            {
                case 0x0:
                    regs[VX] = regs[VY];
                    break;
                case 0x1:
                    regs[VX] |= regs[VY];
                    break;
                case 0x2:
                    regs[VX] &= regs[VY];
                    break;
                case 0x3:
                    regs[VX] ^= regs[VY];
                    break;
                case 0x4:
                    if (regs[VX] + regs[VY] > 255)
                    {
                        regs[0xf] = 1;
                        regs[VX] += regs[VY];
                    }
                    else
                    {
                        regs[0xf] = 0;
                        regs[VX] += regs[VY];

                    }
                    break;
                case 5:
                    if (regs[VY] > regs[VX])
                    {

                        regs[15] = 1;
                        regs[VX] -= regs[VY];

                    }
                    else
                    {
                        regs[15] = 0;
                        regs[VX] -= regs[VY];
                    }
                    break;
                case 6:
                    regs[VX] >>= 1;
                    break;
                case 7:
                    if (regs[VX] > regs[VY])
                    {

                        regs[15] = 1;
                        regs[VX] = (byte)(regs[VY] - regs[VX]);

                    }
                    else
                    {
                        regs[15] = 0;
                        regs[VX] = (byte)(regs[VY] - regs[VX]);
                    }
                    break;
                case 0xe:
                    regs[VX] <<= 1;
                    break;

                default:
                    break;
            }

        }
        void _3XNN(int opCode)
        {
            int vx = (opCode & 0x0F00) >> 8;
            if (regs[vx] == (opCode & 0x00FF))
            {
                tick();
                tick();
            }
            else
            {
                tick();
            }
        }
        void _4XNN(int opCode)
        {
            int vx = (opCode & 0x0F00) >> 8;
            if (regs[vx] != (opCode & 0x00FF))
            {
                tick();
                tick();
            }
            else
            {
                tick();
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
            int regX = (opCode & 0x0F00) >> 8;
            int value = (opCode & 0x00FF);
            regs[regX] = (byte)value;
        }
        void _7XNN(int opCode)
        {
            regs[(byte)((opCode & 0x0F00) >> 8)] += (byte) (opCode & 0X00FF);

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
            int yCoord = (regY + n) % 32;
            int bit = 7;
              for (int i = 0; i < 8; i++)
                {
                    int xCoord =(regX +i) % 64;

                    if (!GetBitX(memory[IR + n], bit))
                    {
                        setpixel(xCoord, yCoord, false);
                    }
                    else if (GetBitX(memory[IR + n], bit))
                    {
                        setpixel(xCoord , yCoord, true);
                    }
                    bit--;
                
                }
            
            }
            
        }
        public void printtoString(int ir)
        {
            IR=ir;
            var address1 = memory[IR]<<8;
            int    address2 = memory[IR + 1];
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
          
            if (pixel[x,y].Fill == Brushes.Black && !state)
            {
                pixel[x, y].Fill = Brushes.White;
            }
            else if (pixel[x, y].Fill == Brushes.Black && state)
            {
                pixel[x, y].Fill = Brushes.Black;
            }
            else if (pixel[x, y].Fill == Brushes.White && !state)
            {
                pixel[x, y].Fill = Brushes.White;
                regs[15] = 1;
            }
            else if (pixel[x, y].Fill == Brushes.White && state)
            {
                pixel[x, y].Fill = Brushes.Black;
            }


        }
    }
}