using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PwResourcesBot {
    class PacketService {

        [Flags]
        public enum AllocationType {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }


        [DllImport("user32.dll", SetLastError= true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(
            IntPtr hProcess,
            Int32 lpBaseAddress,
            [In, Out] Byte [] buffer,
            Int32 nSize,
            out Int32 lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern int VirtualAllocEx(IntPtr hProcess, Int32 lpAddress,
           Int32 dwSize, AllocationType flAllocationType, MemoryProtection flProtect);


        public static void sendPacket(IntPtr handle, int processId, byte [] packet) {
            //временная переменная
            int tmpInt;
            int PacketAllocMemory;

            int alloc_address = VirtualAllocEx(handle, 0, 2500, AllocationType.Commit, MemoryProtection.ReadWrite);
            PacketAllocMemory = alloc_address + 2000;
            //Записываем в открытую память пакет в выделенное место
            WriteProcessMemory(handle, PacketAllocMemory, packet, packet.Length, out tmpInt);

          
            Asm asm = new Asm();
            asm.Pushad();
            asm.Mov_EAX(GameService.PackCall);
            asm.Mov_ECX_DWORD_Ptr(GameService.BaseAddress);
            asm.Mov_ECX_DWORD_Ptr_ECX_Add(0x20);
            asm.Mov_EDI(PacketAllocMemory);
            asm.Push6A(packet.Length);
            asm.Push_EDI();
            asm.Call_EAX();
            asm.Popad();
            asm.Ret();
            asm.RunAsm((int)processId, 0);

            /* Asm asm = new Asm();

             asm.Pushad();
             asm.Mov_EAX_DWORD_Ptr(GameService.BaseAddress);
             asm.Mov_EAX_DWORD_Ptr_EAX_Add(0x20);
             asm.Push6A(getByteLen(packetStr));
             //asm.Push68(StrToByte(packetStr));

             asm.Mov_EDX(GameService.PackCall);
             asm.Call_EDX();
             asm.Popad();
             asm.Ret();
             asm.RunAsm(Form1.handleProcessId, 0);*/
        }

        public static byte[] StrToByte(string packet) {
            byte [] bytePacket = {};

            int len = packet.Length;
            //len = len / 2;
            len = (len / 2) - 1;

            /*for (int i=0; i <= len; i++) {
                char tmp1 = packet [i * 2 + 1];
                char tmp2 = packet [i * 2 + 2];

                bytePacket [i] = byte.Parse(tmp1.ToString() + tmp2.ToString());
            }*/

            bytePacket = Encoding.Default.GetBytes(packet);

            return bytePacket;

        }

        public static int getByteLen(string packet) {
            int len = packet.Length;
            len = (len % 2);
            return len;
        }
    }
}
