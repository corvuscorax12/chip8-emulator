using System;
using System.Net.Sockets;

namespace chip_8
{
    internal class Internals
    {
        public Internals()
        {
            memory[0] = 0xff;
            memory[1] = 0x69;
        }
        byte[] memory =  new byte[4096];
        int IR = 0;
        public void printtoString(int ir)
        {
            IR=ir;
            var address1 = memory[IR]<<8;
            var address2 = memory[IR + 1];
            var hexValue = address1 + address2;
            Console.WriteLine("{0:X}",hexValue);
        }
        
    }
}