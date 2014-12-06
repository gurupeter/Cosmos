﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cosmos.HAL;
using Cosmos.HAL.Drivers.PCI.Network;
using Cosmos.System.Network;
using Cosmos.System.Network.IPv4;

namespace PlaygroundSystem
{
    public class HALGlobal
    {
        public static void Execute()
        {
            Console.WriteLine("Finding PCI device");
            var xNicDev = PCI.GetDevice(0x1022, 0x2000);
            if (xNicDev == null)
            {
                Console.WriteLine("  Not found!!");
                return;
            }

            var xNicDevNormal = xNicDev as PCIDeviceNormal;
            if (xNicDevNormal == null)
            {
                Console.WriteLine("  Unable to cast as PCIDeviceNormal!");
                return;
            }
            var xNic = new AMDPCNetII(xNicDevNormal);
            NetworkStack.Init();
            xNic.Enable();
            NetworkStack.ConfigIP(xNic, new Config(new Address(192, 168, 17, 100), new Address(255, 255, 255, 0)));

            var xU = new UDPPacket(new Address(192, 168, 17, 100), new Address(192, 168, 17, 101), 15, 25, new byte[]
                                                                                                           {
                                                                                                               1,
                                                                                                               2,
                                                                                                               3,
                                                                                                               4,
                                                                                                               5,
                                                                                                               6,
                                                                                                               7,
                                                                                                               8,
                                                                                                               9,
                                                                                                               0xAA,
                                                                                                               0xBB,
                                                                                                               0xCC,
                                                                                                               0xDD,
                                                                                                               0xEE,
                                                                                                               0xFF
                                                                                                           });

            xNic.QueueBytes(xU.GetBytes());
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}