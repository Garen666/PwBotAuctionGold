using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsFormsApplication2;
using System.IO;
using System.Windows.Forms;

namespace PwResourcesBot {

    [Flags]
    public enum ProcessAccessFlags : uint {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VirtualMemoryOperation = 0x00000008,
        VirtualMemoryRead = 0x00000010,
        VirtualMemoryWrite = 0x00000020,
        DuplicateHandle = 0x00000040,
        CreateProcess = 0x000000080,
        SetQuota = 0x00000100,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000
    }


    class ReadMemoryService {

        public static int addressPersX;
        public static int addressPersY;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
             ProcessAccessFlags processAccess,
             bool bInheritHandle,
             int processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(
           IntPtr hProcess,
           int lpBaseAddress,
           byte [] lpBuffer,
           int nSize,
           out IntPtr lpNumberOfBytesWritten
        );

        // Импортируем функцию для чтения памяти чужого процесса 
        // из kernel32
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(
                  IntPtr hProcess,
                  IntPtr lpBaseAddress,
                  [In, Out] byte [] buffer,
                  UInt32 size,
                  out IntPtr lpNumberOfBytesRead
                  );

      
 


        public static String getPersName(IntPtr handle) {
            int tmp = 0;

            tmp = getPersStructure(handle);
            tmp = LowLevelReadIntFromMemory(handle, tmp + GameService.offsetPersName);
            string result = LowLevelReadIntFromMemoryStringUnicode(handle, tmp + 0);

            return result;
        }

        public static String getServerName(IntPtr handle) {
            string url = ReadString_ASCII(handle, 0xF00074);

            if (url == "link1.pwonline.ru") {
                return "Орион";
            } else if (url == "link2.pwonline.ru") {
                return "Вега";
            } else if (url == "link3.pwonline.ru") {
                return "Сириус";
            } else if (url == "link4.pwonline.ru") {
                return "Мира";
            } else if (url == "link5.pwonline.ru") {
                return "Таразед";
            } else if (url == "link6.pwonline.ru") {
                return "Альтаир";
            } else if (url == "link7.pwonline.ru") {
                return "Гелиос";
            } else if (url == "link9.pwonline.ru") {
                return "Атлас";
            } else if (url == "link12.pwonline.ru") {
                return "Кассиопея";
            } else if (url == "link13.pwonline.ru") {
                return "Лиридан";
            } else if (url == "link15.pwonline.ru") {
                return "Гидра";
            } else if (url == "link10.pwonline.ru") {
                return "Луна";
            } else if (url == "link14.pwonline.ru") {
                return "Лисичка";
            } 

            return "";
        }


        public static void ActiveServerList(IntPtr handle) {
            int tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
            tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x1c);
            tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x18);
            tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x08);
            tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x8c);

            while (tmp > 0) {
                int next = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp);
                tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x08);
                int result = tmp;
                tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x4C);
                string tmp2 = ReadMemoryService.ReadString_ASCII(handle, tmp + 0x0);

                if (tmp2 == "Win_LoginServerList") {

                    int tmp3 = ReadMemoryService.LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
                    tmp3 = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp3 + 0x1c);
                    tmp3 = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp3 + 0x18);
                    tmp3 = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp3 + 0x08);
                    tmp3 += 0x74;

                    byte [] byteArray = BitConverter.GetBytes(result);

                    IntPtr tmpInt = new IntPtr();
                    WriteProcessMemory(handle, tmp3, byteArray, byteArray.Length, out tmpInt);
                    return;
                }
                tmp = next;
            }
        }

        public static void ActiveGoldList(IntPtr handle)
        {
            int tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
            tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x1c);
            tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x18);
            tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x08);
            tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x8c);

            while (tmp > 0)
            {
                int next = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp);
                tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x08);
                int result = tmp;
                tmp = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp + 0x4C);
                string tmp2 = ReadMemoryService.ReadString_ASCII(handle, tmp + 0x0);

                //addToConsoleInfo(tmp2 + " - " + tmp);

                /*if (tmp2 == "Win_GoldTrade")
                {

                    int tmp3 = ReadMemoryService.LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
                    tmp3 = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp3 + 0x1c);
                    tmp3 = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp3 + 0x18);
                    tmp3 = ReadMemoryService.LowLevelReadIntFromMemory(handle, tmp3 + 0x08);
                    tmp3 += 0x74;

                    byte[] byteArray = BitConverter.GetBytes(result);

                    IntPtr tmpInt = new IntPtr();
                    WriteProcessMemory(handle, tmp3, byteArray, byteArray.Length, out tmpInt);
                    return;
                }*/
                tmp = next;
            }
        }

        public static int getPersId(IntPtr handle) {
            return LowLevelReadIntFromMemory(handle, GameService.persIdAddress); ;
        }

        public delegate void MethodContainer();

        //Событие OnCount c типом делегата MethodContainer.
        public static event MethodContainer onCount;

        public static int getPackAcceptAddress(IntPtr handle) {
            int tmp = 0;
            tmp = LowLevelReadIntFromMemory(handle, 0xF1972C);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x4);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x14);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0xC);

            if (tmp == 0) {
                if (getActiveWinName(handle) == "MsgBox_LinkBroken") {
                    Form1.runThread.Abort();
                }
            }

            return tmp;
        }


        public static bool isLock(IntPtr handle) {
            int tmp = 0;
            tmp = LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x1C);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x18);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x8);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0xC4);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x18);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x250);

            if (tmp > 0) {
                return true;
            } else {
                return false;
            }
        }



        public static Int32 GetLutStructure (IntPtr handle) {
            int tmp = 0;

            tmp = LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
            tmp = LowLevelReadIntFromMemory(handle, tmp + GameService.OffsetGame);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x1C);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x24);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x1C);

            return tmp;
        }

       
        public static Dictionary<string, string> GetResInfo(IntPtr handle, int id, int distance) {

            int tmp = GetLutStructure(handle);
            var arr = new Dictionary<string, string>();

            for (int i = 0; i < 768; i++) {
                int resStructure;
                resStructure = LowLevelReadIntFromMemory(handle, tmp + i * 0x4);

                if (resStructure == 0) {
                    continue;
                }

                resStructure = LowLevelReadIntFromMemory(handle, resStructure + 0x4);

                int resId = LowLevelReadIntFromMemory(handle, resStructure + 0x114);

                if (resId != id) {
                    continue;
                }

                Single resDistance = LowLevelReadIntFromMemoryFloat(handle, resStructure + 0x15C);

                if (distance > 0) {
                    if (resDistance > distance) {
                        continue;
                    }

                } else if (resDistance > 25) {
                    continue;
                }

                arr ["id"] = id.ToString();
                arr ["index"] = i.ToString();
                arr ["wid"] = LowLevelReadIntFromMemory(handle, resStructure + 0x110).ToString();
                arr ["distance"] = resDistance.ToString();
                arr ["x"] = LowLevelReadIntFromMemoryFloat(handle, resStructure + 0x3C).ToString();
                arr ["y"] = LowLevelReadIntFromMemoryFloat(handle, resStructure + 0x44).ToString();
                arr ["z"] = LowLevelReadIntFromMemoryFloat(handle, resStructure + 0x40).ToString();

                return arr;
            }


            arr ["id"] = id.ToString();
            arr ["index"] = 0.ToString();
            arr ["wid"] = 0.ToString();
            arr ["distance"] = 0.ToString();

            return arr;
        }


        private static Int32 getPersStructure(IntPtr handle) {
            int tmp = 0;

            tmp = LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
            tmp = LowLevelReadIntFromMemory(handle, tmp + GameService.OffsetGame);
            tmp = LowLevelReadIntFromMemory(handle, tmp + GameService.OffsetPersStructure);

            return tmp;
        }

        public static Int32 getGoldAuction(IntPtr handle) {
            return goldAuctionGold;

            int tmp = 0;
            int tmp2 = 0;

            if (goldAuctionAddress > 0) {
                tmp = LowLevelReadIntFromMemory(handle, goldAuctionAddress);
            } else {
                tmp = LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
                tmp = LowLevelReadIntFromMemory(handle, tmp + GameService.OffsetGame);
                tmp = LowLevelReadIntFromMemory(handle, tmp + 0x18);
                tmp = LowLevelReadIntFromMemory(handle, tmp + 0x08);
                tmp = LowLevelReadIntFromMemory(handle, tmp + 0x8C);
                tmp = LowLevelReadIntFromMemory(handle, tmp + 0x08);
                tmp2 = LowLevelReadIntFromMemory(handle, tmp + 0x264);
                tmp = LowLevelReadIntFromMemory(handle, tmp2 + 0xF0);


                goldAuctionAddress = tmp2 + 0xF0;
            }
            

            return tmp;
        }

        public static Dictionary<string, Dictionary<int, int>> getListGold(IntPtr handle, int procId, int address) {
            listGoldCount++;

            if (listGoldCount > 5) {
                if (getActiveWinName(handle) == "MsgBox_LinkBroken") {
                    Form1.runThread.Abort();
                }
            }

            Dictionary<string, Dictionary<int, int>> goldArray = new Dictionary<string, Dictionary<int, int>>();

            Dictionary<int, int> saleArray = new Dictionary<int, int>();
            Dictionary<int, int> saleArray2 = new Dictionary<int, int>();
            Dictionary<int, int> buyArray = new Dictionary<int, int>();

            

            PacketService.sendPacket(handle, procId, GoldService.RefreshListPacket());

            var dateTime1 = DateTime.Now;

            while (true) {
                var tmp = new byte [141];
                IntPtr read = IntPtr.Zero;

                ReadProcessMemory(handle, (IntPtr) address, tmp, 141, out read);

                if (tmp [0] == 0x81 && tmp [1] == 0x98) {

                    byte [] priceByte = new byte [4];
                    byte [] countByte = new byte [4];

                    byte [] moneyByte = new byte [4];
                    byte [] goldByte = new byte [4];

                    int offsetPrice = 13;
                    int offsetCount = 17;
                    int offsetMoney = 8;
                    int offsetGold = 4;

                    int count2 = tmp [12];

                    if (count2 == 0 || count2 == 255) {
                        count2 = tmp [11];
                        offsetPrice--;
                        offsetCount--;
                        offsetMoney--;
                        offsetGold--;
                    }

                    if (count2 > 16) {
                        count2 = 0;
                    }

                    moneyByte [0] = tmp [offsetMoney];
                    moneyByte [1] = tmp [offsetMoney + 1];
                    moneyByte [2] = tmp [offsetMoney + 2];
                    moneyByte [3] = tmp [offsetMoney + 3];

                    goldByte [0] = tmp [offsetGold];
                    goldByte [1] = tmp [offsetGold + 1];
                    goldByte [2] = tmp [offsetGold + 2];
                    goldByte [3] = tmp [offsetGold + 3];

                    goldAuctionMoney = Int32.Parse(ByteArrayToString(moneyByte), System.Globalization.NumberStyles.HexNumber);
                    goldAuctionGold = Int32.Parse(ByteArrayToString(goldByte), System.Globalization.NumberStyles.HexNumber);



                    for (int i = 0; i < count2; i++) {

                        priceByte [0] = tmp [offsetPrice + i * 8];
                        priceByte [1] = tmp [offsetPrice + i * 8 + 1];
                        priceByte [2] = tmp [offsetPrice + i * 8 + 2];
                        priceByte [3] = tmp [offsetPrice + i * 8 + 3];

                        countByte [0] = tmp [offsetCount + i * 8];
                        countByte [1] = tmp [offsetCount + i * 8 + 1];
                        countByte [2] = tmp [offsetCount + i * 8 + 2];
                        countByte [3] = tmp [offsetCount + i * 8 + 3];

                        int price = Int32.Parse(ByteArrayToString(priceByte), System.Globalization.NumberStyles.HexNumber);
                        int count = Int32.Parse(ByteArrayToString(countByte), System.Globalization.NumberStyles.HexNumber);

                        if (price < 0) {
                            price *= -1;
                            buyArray.Add(price, count);
                        } else {
                            saleArray.Add(price, count);
                        }

                    }


                    
                    goldArray.Add("buy", buyArray);


                    var saleKeys = saleArray.Keys.ToList();
                    saleKeys.Sort();
                    foreach (var key in saleKeys) {
                        saleArray2.Add(key, saleArray[key]);
                    }

                    goldArray.Add("sale", saleArray2);

                    listGoldCount = 0;

                    return goldArray;
                }
                var dateTime2 = DateTime.Now;
                var diffInSeconds = (dateTime2 - dateTime1).TotalSeconds;

                if (diffInSeconds > 5) {
                    if ((tmp[0] == 0 && tmp[1] == 0 && tmp[2] == 0 && tmp[3] == 0) || LowLevelReadIntFromMemory(handle, GameService.OnlineAddress) == 2) {
                        if (getActiveWinName(handle) == "MsgBox_LinkBroken") {
                            Form1.runThread.Abort();
                        }
                    }
                    return getListGold(handle, procId, address);
                }

                Thread.Sleep(10);
            }
        }

        public static string getActiveWinName(IntPtr handle) {
            int tmp = 0;

            tmp = LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
            tmp = LowLevelReadIntFromMemory(handle, tmp + GameService.OffsetGame);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x18);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x08);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x74);
            tmp = LowLevelReadIntFromMemory(handle, tmp + 0x4c);
            string tmp2 = ReadString_ASCII(handle, tmp + 0x0);

            return tmp2;
        }

        private static int listGoldSupplierCount = 0;
        private static int listGoldCount = 0;


        public static Dictionary<string, Dictionary<int, Dictionary<string, int>>> getListGoldSupplier(IntPtr handle, int procId, int address) {
            listGoldSupplierCount++;

            if (listGoldSupplierCount > 5) {
                if (getActiveWinName(handle) == "MsgBox_LinkBroken") {
                    Form1.runThread.Abort();
                }
            }

            Dictionary<string, Dictionary<int, Dictionary<string, int>>> goldArray = new Dictionary<string, Dictionary<int, Dictionary<string, int>>>();

            Dictionary<int, Dictionary<string, int>> saleArray = new Dictionary<int, Dictionary<string, int>>();
            Dictionary<int, Dictionary<string, int>> buyArray = new Dictionary<int, Dictionary<string, int>>();

            //addToConsole("Пакет на обновление");

            PacketService.sendPacket(handle, procId, GoldService.RefreshListPacketSupplier());

            //addToConsole("отправили");
            //address = 0x1ce7f3e8;

            var dateTime1 = DateTime.Now;

            while (true) {
                var tmp = new byte [230];
                IntPtr read = IntPtr.Zero;

                ReadProcessMemory(handle, (IntPtr) address, tmp, 230, out read);
                //addToConsole("0 - " + tmp [0] + "1 - " + tmp [1]);
       

                if (tmp [0] == 0x81 && tmp [1] == 0x96) {

                    int rateCount = tmp [8];
                    int offsetId = 9;
                    int offsetPrice = 21;
                    int offsetCount = 25;

                    if (tmp[7] == 0) {
                        goldArray.Add("buy", buyArray);
                        goldArray.Add("sale", saleArray);
                        return goldArray;
                    } else if (tmp[9] == 0) {

                    } else if (tmp [8] == 0 && tmp[3] == 0) {
                        rateCount = tmp [7];

                        offsetId--;
                        offsetPrice--;
                        offsetCount--;
                    }

                    var tmp123 = BitConverter.ToString(tmp);

                    if (rateCount > 10 || rateCount < 0) {
                        /*addToConsoleBug("");
                        addToConsoleBug(DateTime.Now.ToString());
                        addToConsoleBug("Пришел конченый запрос");
                        addToConsoleBug(BitConverter.ToString(tmp));*/
                        return getListGoldSupplier(handle, procId, address);
                    }

                    byte [] idsByte2 = new byte [4];
                    byte [] priceByte2 = new byte [4];
                    byte [] countByte2 = new byte [4];

                    for (int i = 0; i < rateCount; i++) {
                        Dictionary<string, int> goldInfo = new Dictionary<string, int>();


                        idsByte2 [0] = tmp [offsetId + i * 21];
                        idsByte2 [1] = tmp [offsetId + i * 21 + 1];
                        idsByte2 [2] = tmp [offsetId + i * 21 + 2];
                        idsByte2 [3] = tmp [offsetId + i * 21 + 3];

                        priceByte2 [0] = tmp [offsetPrice + i * 21];
                        priceByte2 [1] = tmp [offsetPrice + i * 21 + 1];
                        priceByte2 [2] = tmp [offsetPrice + i * 21 + 2];
                        priceByte2 [3] = tmp [offsetPrice + i * 21 + 3];

                        countByte2 [0] = tmp [offsetCount + i * 21];
                        countByte2 [1] = tmp [offsetCount + i * 21 + 1];
                        countByte2 [2] = tmp [offsetCount + i * 21 + 2];
                        countByte2 [3] = tmp [offsetCount + i * 21 + 3];

                        int id = Int32.Parse(ByteArrayToString(idsByte2), System.Globalization.NumberStyles.HexNumber);
                        int price = Int32.Parse(ByteArrayToString(priceByte2), System.Globalization.NumberStyles.HexNumber);
                        int count = Int32.Parse(ByteArrayToString(countByte2), System.Globalization.NumberStyles.HexNumber);

                        if (count < 0 || count > 1000) {
                            /*addToConsoleBug("");
                            addToConsoleBug(DateTime.Now.ToString());
                            addToConsoleBug("Пришел конченый запрос, количество хреновое");
                            addToConsoleBug(BitConverter.ToString(tmp));*/
                            return getListGoldSupplier(handle, procId, address);
                        }

                        goldInfo.Add("id", id);
                        goldInfo.Add("count", count);

                        if (price > 0) {
                            goldInfo.Add("price", price);

                            saleArray.Add(id, goldInfo);
                        } else {
                            goldInfo.Add("price", price * -1);
                            buyArray.Add(id, goldInfo);
                        }

                    }

                    goldArray.Add("buy", buyArray);
                    goldArray.Add("sale", saleArray);

                    listGoldSupplierCount = 0;

                    return goldArray;
                }
                var dateTime2 = DateTime.Now;
                var diffInSeconds = (dateTime2 - dateTime1).TotalSeconds;

                if (diffInSeconds > 5) {
                    //addToConsole("Прошло 5 сек");

                    if ((tmp [0] == 0 && tmp [1] == 0 && tmp [2] == 0 && tmp [3] == 0) || LowLevelReadIntFromMemory(handle, GameService.OnlineAddress) == 2) {
                        if(getActiveWinName(handle) == "MsgBox_LinkBroken") {
                            Form1.runThread.Abort();
                        }
                    }
                    return getListGoldSupplier(handle, procId, address);
                }

            }

            
        }

        public static int MakeMonye(IntPtr handle2, int processId2) {
            int result = 0;
            int gold = 0;

            result += getMoneyAuction(handle2);

            var goldSupplier = getListGoldSupplier(handle2, processId2, getPackAcceptAddress(handle2));


            MessageBox.Show(result.ToString());
            return result;
        }


        public static void addToConsole(string text) {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Admin\Desktop\bot_" + Form1.procId + ".txt", true);
            file.WriteLine(text);
            file.Close();
        }

        public static void addToConsoleBug(string text) {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Admin\Desktop\bot_" + Form1.procId + "_bug.txt", true);
            file.WriteLine(text);
            file.Close();
        }

        public static void addToConsoleInfo(string text)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Admin\Desktop\bot_" + Form1.procId + "_info.txt", true);
            file.WriteLine(text);
            file.Close();
        }

        public static string ByteArrayToString(byte [] ba) {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        public static int moneyAuctionAddress = 0;
        public static int goldAuctionAddress = 0;
        public static int goldAuctionGold = 0;
        public static int goldAuctionMoney = 0;


        public static Int32 getMoneyAuction(IntPtr handle) {
            return goldAuctionMoney;

            int tmp = 0;
            int tmp2 = 0;

            if (moneyAuctionAddress > 0) {
                tmp = LowLevelReadIntFromMemory(handle, moneyAuctionAddress);
            } else {
                tmp = LowLevelReadIntFromMemory(handle, GameService.BaseAddress);
                tmp = LowLevelReadIntFromMemory(handle, tmp + GameService.OffsetGame);
                tmp = LowLevelReadIntFromMemory(handle, tmp + 0x18);
                tmp = LowLevelReadIntFromMemory(handle, tmp + 0x08);
                tmp = LowLevelReadIntFromMemory(handle, tmp + 0x8C);
                tmp = LowLevelReadIntFromMemory(handle, tmp + 0x08);
                tmp2 = LowLevelReadIntFromMemory(handle, tmp + 0x274);
                tmp = LowLevelReadIntFromMemory(handle, tmp2 + 0xF0);

                if (tmp == 8192) {
                    MessageBox.Show("Закрой окно епрст!");
                    return getMoneyAuction(handle);
                }

                moneyAuctionAddress = tmp2 + 0xF0;
            }

            return tmp;
        }


        public static Single ByteToInt(byte[] buffer) {
            return BitConverter.ToSingle(buffer, 0);
        }

        public static Int32 LowLevelReadIntFromMemory(IntPtr handle, int address) {
            //handle = OpenProcess(ProcessAccessFlags.All, false, Form1.handleProcessId);
            byte [] buffer = new byte [8];
            IntPtr read = IntPtr.Zero;

            ReadProcessMemory(handle, (IntPtr) address, buffer, 4, out read);

            return (int) BitConverter.ToUInt32(buffer, 0);
        }


        // Читает из памяти String по указанному адресу с заданной длиной.
        public static String LowLevelReadIntFromMemoryStringUnicode(IntPtr handle, Int32 address) {
            var buffer = new byte [100];
            IntPtr read = IntPtr.Zero;

            ReadProcessMemory(handle, (IntPtr) address, buffer, 100, out read);

            var enc = new UnicodeEncoding();
            var rtnStr = enc.GetString(buffer);

            return (rtnStr.IndexOf('\0') != -1) ? rtnStr.Substring(0, rtnStr.IndexOf('\0')) : rtnStr;
        }

        // Читает из памяти Float по указанному адресу.
        public static Single LowLevelReadIntFromMemoryFloat(IntPtr handle, Int32 address) {
            //handle = OpenProcess(ProcessAccessFlags.All, false, Form1.handleProcessId);
            var buffer = new byte [4];
            IntPtr read = IntPtr.Zero;

            ReadProcessMemory(handle, (IntPtr) address, buffer,4, out read);

            return BitConverter.ToSingle(buffer, 0);
        }

        // Читает из памяти String по указанному адресу с заданной длиной в кодировке ANSCII.
        public static String ReadString_ASCII(IntPtr handle, Int32 address) {
            var buffer = new byte [100];
            IntPtr read = IntPtr.Zero;

            ReadProcessMemory(handle, (IntPtr) address, buffer, 100, out read);

            var enc = new ASCIIEncoding();
            var rtnStr = enc.GetString(buffer);

            return (rtnStr.IndexOf('\0') != -1) ? rtnStr.Substring(0, rtnStr.IndexOf('\0')) : rtnStr;
        }

        // Читает из памяти Int64 по указанному адресу.
        /*public static Int64 ReadInt64(IntPtr handle, int address) {
            var buffer = new byte [8];
            IntPtr read = IntPtr.Zero;

            ReadProcessMemory(handle, (IntPtr) address, buffer, 8, out read);

            return BitConverter.ToInt64(buffer, 0);
        }*/



        // Читает из памяти Double по указанному адресу.
        /*public static Double ReadDouble(IntPtr handle, Int32 address) {
            var buffer = new byte [8];
            IntPtr read = IntPtr.Zero;

            ReadProcessMemory(handle, (IntPtr) address, buffer, 8, out read);

            return BitConverter.ToDouble(buffer, 0);
        }*/

    }
}
