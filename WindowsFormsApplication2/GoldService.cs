using PwResourcesBot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2 {
    class GoldService {
        private static byte [] refreshList = { 0x25, 0x00, 0x2B, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x97, 0x01, 0x00, 0x00, 0x07, 0x20, 0xED, 0xE0, 0x00, 0x00, 0x00, 0x00 };
        private static byte [] refreshListSupplier = { 0x25, 0x00, 0x2B, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x95, 0x01, 0x00, 0x00, 0x07, 0x20, 0xED, 0xE0, 0x00, 0x00, 0x00, 0x00 };
        private static byte [] addGoldBuy =     {0x25, 0x00, 0x2B, 0x00, 0x00, 0x00, 0x15, 0x00, 0x00, 0x00, 0x91, 0x01, 0x00, 0x00, 0x00, 0x9D, 0x40, 0x91, 0x01, 0x00, 0x03, 0x95, 0xF8, 0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00};
        private static byte [] addGoldSell =    {0x25, 0x00, 0x2B, 0x00, 0x00, 0x00, 0x15, 0x00, 0x00, 0x00, 0x91, 0x01, 0x00, 0x00, 0x00, 0x9D, 0x40, 0x91, 0x00, 0x00, 0x10, 0x8E, 0x48, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00};
        private static byte [] cancelGoldBuy =  {0x25, 0x00, 0x2B, 0x00, 0x00, 0x00, 0x14, 0x00, 0x00, 0x00, 0x9B, 0x01, 0x00, 0x00, 0x00, 0x9D, 0x40, 0x91, 0x00, 0x0A, 0xF8, 0xEB, 0xFF, 0xFF, 0xFE, 0x0C, 0x00, 0x00, 0x00, 0x00};

        public static byte [] selectedNPC = {0x02, 0x00, 0x45, 0x00, 0x10, 0x80};
        public static byte [] selectedNPCMenu = {0x23, 0x00, 0x45, 0x00, 0x10, 0x80};

        public static byte[] RefreshListPacket() {
            var persId = BitConverter.GetBytes(ReadMemoryService.getPersId(Process.GetProcessById(Form1.procId).Handle));
            byte [] packet = refreshList;

            packet [14] = persId [3];
            packet [15] = persId [2];
            packet [16] = persId [1];
            packet [17] = persId [0];

            return packet;
        }

        public static byte [] RefreshListPacketSupplier() {
            var persId = BitConverter.GetBytes(ReadMemoryService.getPersId(Process.GetProcessById(Form1.procId).Handle));
            byte [] packet = refreshListSupplier;

            packet [14] = persId [3];
            packet [15] = persId [2];
            packet [16] = persId [1];
            packet [17] = persId [0];

            return packet;
        }

        public static byte[] AddGoldBuyPacket(int price, int count) {
            var persId = BitConverter.GetBytes(ReadMemoryService.getPersId(Process.GetProcessById(Form1.procId).Handle));
            byte [] packet = addGoldBuy;

            packet [14] = persId [3];
            packet [15] = persId [2];
            packet [16] = persId [1];
            packet [17] = persId [0];

            var priceByte = BitConverter.GetBytes(price);

            packet [19] = priceByte [3];
            packet [20] = priceByte [2];
            packet [21] = priceByte [1];
            packet [22] = priceByte [0];

            var countByte = BitConverter.GetBytes(count);

            packet [23] = countByte [3];
            packet [24] = countByte [2];
            packet [25] = countByte [1];
            packet [26] = countByte [0];

            return packet;
        }

        public static byte [] AddGoldSellPacket(int price, int count) {
            var persId = BitConverter.GetBytes(ReadMemoryService.getPersId(Process.GetProcessById(Form1.procId).Handle));
            byte [] packet = addGoldSell;

            packet [14] = persId [3];
            packet [15] = persId [2];
            packet [16] = persId [1];
            packet [17] = persId [0];

            var priceByte = BitConverter.GetBytes(price);

            packet [19] = priceByte [3];
            packet [20] = priceByte [2];
            packet [21] = priceByte [1];
            packet [22] = priceByte [0];

            var countByte = BitConverter.GetBytes(count);

            packet [23] = countByte [3];
            packet [24] = countByte [2];
            packet [25] = countByte [1];
            packet [26] = countByte [0];

            return packet;
        }

        public static byte [] CancelGoldBuyPacket(int id, int price) {
            var persId = BitConverter.GetBytes(ReadMemoryService.getPersId(Process.GetProcessById(Form1.procId).Handle));
            byte [] packet = cancelGoldBuy;

            packet [14] = persId [3];
            packet [15] = persId [2];
            packet [16] = persId [1];
            packet [17] = persId [0];

            var idByte = BitConverter.GetBytes(id);

            packet [18] = idByte [3];
            packet [19] = idByte [2];
            packet [20] = idByte [1];
            packet [21] = idByte [0];

            var priceByte = BitConverter.GetBytes(price);

            packet [22] = priceByte [3];
            packet [23] = priceByte [2];
            packet [24] = priceByte [1];
            packet [25] = priceByte [0];

            return packet;
        }


    }
}
