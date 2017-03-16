using PwResourcesBot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();

            updateComboProcess();
        }

        [DllImport("User32")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);


        public static int procId = 0;
        public static Dictionary<int, Dictionary<string, string>> buyArray = new Dictionary<int, Dictionary<string, string>>();
        public static Dictionary<int, Dictionary<string, string>> saleArray = new Dictionary<int, Dictionary<string, string>>();

        public static Thread runThread;
        public static Thread runThread2;
        public static int logCount = 0;

        private static Dictionary<string, Dictionary<int, int>> goldArray = new Dictionary<string, Dictionary<int, int>>();
        private static Dictionary<int, int> goldBuyArray = new Dictionary<int, int>();
        private static Dictionary<int, int> goldSaleArray = new Dictionary<int, int>();

        private static Dictionary<string, Dictionary<int, Dictionary<string, int>>> supplierArray = new Dictionary<string, Dictionary<int, Dictionary<string, int>>>();
        private static Dictionary<int, Dictionary<string, int>> supplierBuyArray = new Dictionary<int, Dictionary<string, int>>();
        private static Dictionary<int, Dictionary<string, int>> supplierSaleArray = new Dictionary<int, Dictionary<string, int>>();

        private int supplierCount = 0;

        [DllImport("User32.dll")]
        public static extern bool PostMessage(IntPtr hwnd, int uMsg, int wParam, int lParam);
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int VK_ESCAPE = 0x1B;
        const int VK_ENTER = 13;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hwnd, String lpString);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);


        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;


        private void button6_Click(object sender, EventArgs e) {
            PacketService.sendPacket(
                Process.GetProcessById(procId).Handle,
                procId,
                GoldService.AddGoldSellPacket(Int32.Parse(textBox7.Text), Int32.Parse(textBox8.Text))
            );
        }

        private void button7_Click(object sender, EventArgs e) {
            PacketService.sendPacket(
                Process.GetProcessById(procId).Handle,
                procId,
                GoldService.CancelGoldBuyPacket(Int32.Parse(textBox9.Text), Int32.Parse(textBox10.Text))
            );
        }

        private void button8_Click(object sender, EventArgs e) {
            listBox1.Items.Clear();

            var tmp = ReadMemoryService.getListGold(Process.GetProcessById(procId).Handle, procId, ReadMemoryService.getPackAcceptAddress(Process.GetProcessById(procId).Handle));


            listBox1.Items.Add("Продажа:");
            var saleArray = tmp ["sale"];
            foreach (var key in saleArray.Keys) {
                listBox1.Items.Add(key + " - " + saleArray [key]);
            }

            listBox1.Items.Add("");
            listBox1.Items.Add("Покупка:");

            var buyArray = tmp ["buy"];

            foreach (var key in buyArray.Keys) {
                listBox1.Items.Add(key + " - " + buyArray [key]);
            }




            listBox2.Items.Clear();

            var tmp2 = ReadMemoryService.getListGoldSupplier(Process.GetProcessById(procId).Handle, procId, ReadMemoryService.getPackAcceptAddress(Process.GetProcessById(procId).Handle));

            var buyArray2 = tmp2 ["buy"];
            var saleArray2 = tmp2 ["sale"];
            int count = saleArray2.Count + buyArray2.Count;

            listBox2.Items.Add("Всего: " + count);
            listBox2.Items.Add("");

            listBox2.Items.Add("Продажа");

            foreach (var tmp3 in saleArray2) {
                listBox2.Items.Add(tmp3.Value ["id"] + " - " + tmp3.Value ["price"] + " - " + tmp3.Value ["count"]);
            }

            listBox2.Items.Add("");

            listBox2.Items.Add("Покупка");

            foreach (var tmp3 in buyArray2) {
                listBox2.Items.Add(tmp3.Value ["id"] + " - " + tmp3.Value ["price"] + " - " + tmp3.Value ["count"]);
            }

        }

        public void Restart() {

            // убиваем старый процесс
            AppService.ClosePW();

            // проверка пинга
            var status = new IPStatus();

            do {
                Ping pingSender = new Ping();
                IPAddress address = IPAddress.Loopback;
                PingReply reply = pingSender.Send(address);
                status = reply.Status;

                Thread.Sleep(5000);
            } while (status != IPStatus.Success);

            runThread = new Thread(new ThreadStart(run));

            runThread.Start();
        }

        /* public delegate void RTChangeText(string sText);

         private void ChangeText(string sText) {
             this.richTextBox1.Text = sText;
         }
 */
        public delegate string GetTextBoxCountMax_D();

        private string GetTextBoxCountMax() {

            return this.textBoxCountMax.Text;
        }

        /*public string GetTextBoxCountMax() {
            string str = "";
            this.BeginInvoke((MethodInvoker) (() => str = textBoxCountMax.Text));

            return str;
        }*/

        public async void changeName(string name, string serverName) {
            name += " (" + serverName + ")";

            this.BeginInvoke((MethodInvoker) (() => this.Text = name));
            this.BeginInvoke((MethodInvoker) (() => notifyIcon1.Text = name));

            //this.Text = name;
            //notifyIcon1.Text = name;
        }

        public string GetTextBoxPriceStep() {
            return this.textBoxPriceStep.Text;
        }

        public async void addToLog(string str) {
            this.BeginInvoke((MethodInvoker) (() => listBox3.Items.Add(str)));

            this.BeginInvoke((MethodInvoker) (() => listBox3.SelectedIndex = listBox3.Items.Count - 1));
            this.BeginInvoke((MethodInvoker) (() => listBox3.SelectedIndex = -1));
        }

        public void HideButton() {
            this.BeginInvoke((MethodInvoker) (() => buttonRun.Hide()));
            this.BeginInvoke((MethodInvoker) (() => buttonStop.Show()));
            this.BeginInvoke((MethodInvoker) (() => buttonRun.Hide()));
         
        }

        private void MakeMoneyText(string str) {
            this.BeginInvoke((MethodInvoker) (() => textBoxMakeMoney.Text = str));
        }


        private void clearLog() {
            this.BeginInvoke((MethodInvoker) (() => listBox3.Items.Clear()));
        }

        private void button9_Click(object sender, EventArgs e) {
            buttonRun.Hide();
            buttonStop.Show();

            runThread2 = new Thread(new ThreadStart(check));
            runThread2.Start();
        }

       

        private void run() {
            clearLog();


            if (Form1.procId == 0) {
                StartNewWindow();
            }
            

            IntPtr handle = GetHandler();
            Random rnd = new Random();

            int address = ReadMemoryService.getPackAcceptAddress(Process.GetProcessById(procId).Handle);
   


            
            int sleepCount = 10000;
            double margin = 0;
            double myMoney = 0;
            int myGold = 0;
            int salePrice = 0;
            int buyPrice = 0;

            while (true) {
                logCount++;

                if (logCount > 10) {
                    clearLog();
                    logCount = 0;
                }

                margin = 0;
                myMoney = 0;
                myGold = 0;
                addToLog("");
                addToLog("Получаем поставленное");
                // получаем поставленные
                supplierArray = ReadMemoryService.getListGoldSupplier(Process.GetProcessById(procId).Handle, procId, address);
                supplierBuyArray = supplierArray ["buy"];
                supplierSaleArray = supplierArray ["sale"];
                // наше количество ставок
                supplierCount = supplierBuyArray.Count + supplierSaleArray.Count;

                Thread.Sleep(100);

                // получаем список
                addToLog("Получаем Общий");
                goldArray = ReadMemoryService.getListGold(Process.GetProcessById(procId).Handle, procId, address);
                goldBuyArray = goldArray ["buy"];
                goldSaleArray = goldArray ["sale"];
                salePrice = goldSaleArray.First().Key;
                buyPrice = goldBuyArray.First().Key;

                margin = salePrice - buyPrice - (buyPrice * 0.04);
                addToLog("Маржа: " + margin);

                Thread.Sleep(100);

                // деньги с аука
                myMoney += ReadMemoryService.getMoneyAuction(Process.GetProcessById(procId).Handle);
                myGold += ReadMemoryService.getGoldAuction(Process.GetProcessById(procId).Handle);

                foreach (var tmp3 in supplierSaleArray) {
                    myGold += tmp3.Value ["count"] * 100;
                }

                foreach (var tmp3 in supplierBuyArray) {
                    myMoney += tmp3.Value ["price"] * tmp3.Value ["count"];
                }

                myMoney += (myGold * 0.98 * buyPrice / 100);
                myMoney = Math.Round(myMoney / 1000);
                MakeMoneyText(myMoney.ToString("N"));

                if (supplierCount < 10 && margin > 3000) {
                    // еще есть возможность ставить ставки

                    // есть и скупка и продажа
                    if (goldBuyArray.Count > 0 && goldSaleArray.Count > 0) {
                        checkBuy();
                        Thread.Sleep(1000);

                        if (supplierCount < 10) {
                            checkSale();
                        }

                    } else if (goldBuyArray.Count > 0) {

                    } else if (goldSaleArray.Count > 0) {

                    }

                } else if (margin > 10000) {
                    // маржа должна быть больше, ибо потери при отмене
                    // нужно чтото отменять

                }

                Thread.Sleep(sleepCount + rnd.Next(10, 100) * 100);
            }
        }

        private void checkBuy() {
            addToLog("");
            addToLog("Логика закупки");

            int money = ReadMemoryService.getMoneyAuction(Process.GetProcessById(procId).Handle);

            int firstPrice = 0;

            int count = 0;
            int price = 0;
            int index = 0;
            int goldCount = 0;
            bool isRate = false;

            int countBeforeMax = Int32.Parse(GetTextBoxCountMax());
            int priceStep = Int32.Parse(GetTextBoxPriceStep());

            int maxCount = 0;

            foreach (var tmp in goldBuyArray) {
                price = tmp.Key;
                count = tmp.Value;

                if (index == 0) {
                    // первая ставка, проверяем если наша или денег нет то выходим
                    if (IsOurRateBuy(price)) {
                        addToLog("Наша ставка первая - return");
                        return;
                    }
                    
                    if ((price * 1.02) > money) {
                        addToLog("Денег нет ("+money+") - return");
                        // денег нет
                        return;
                    }

                    firstPrice = price;
                    
                    // добавляем к количеству перед нами
                    goldCount += count;

                } else {
                    // нашли нашу ставку
                    if (IsOurRateBuy(price)) {
                        isRate = true;
                        break;
                    } else {
                        // добавляем к количеству перед нами
                        goldCount += count;
                    }

                }

                index++;

            }

            // наши ставки есть в списке из 8 последних
            if (isRate) {
                addToLog("Ставки есть, до наших - " + goldCount);

                // до на сбольше 40
                if (goldCount > countBeforeMax) {
                    // делаем ставку
                    firstPrice += priceStep;

                    maxCount = getMaxCountBuy(firstPrice);

                    PacketService.sendPacket(Process.GetProcessById(procId).Handle, procId, GoldService.AddGoldBuyPacket(firstPrice, maxCount));

                    addToLog("До нас - "+ goldCount + ", Делаем ставку: " + firstPrice + " - " + maxCount);
                    supplierCount++;
                }

            } else {
                // ставок нет

                // делаем ставку
                firstPrice += priceStep;

                maxCount = getMaxCountBuy(firstPrice);

                PacketService.sendPacket(Process.GetProcessById(procId).Handle, procId, GoldService.AddGoldBuyPacket(firstPrice, maxCount));

                addToLog("Ставок нет, Делаем ставку: "+firstPrice + " - " + maxCount);

                supplierCount++;
            }

        }

        private void checkSale() {
            addToLog("");
            addToLog("Логика Продажи");

            int money = ReadMemoryService.getGoldAuction(Process.GetProcessById(procId).Handle);

            int firstPrice = 0;

            int count = 0;
            int price = 0;
            int index = 0;
            int goldCount = 0;
            bool isRate = false;
            int countBeforeMax = Int32.Parse(textBoxCountMax.Text);
            int priceStep = Int32.Parse(textBoxPriceStep.Text);
            

            int maxCount = 0;

            foreach (var tmp in goldSaleArray) {
                price = tmp.Key;
                count = tmp.Value;

                if (index == 0) {
                    // первая ставка, проверяем если наша или денег нет то выходим
                    if (IsOurRateSale(price)) {
                        addToLog("Наша ставка первая - return");
                        return;
                    }

                    if (money < 102) {
                        addToLog("Денег нет (" + money + ") - return");
                        // денег нет
                        return;
                    }

                    firstPrice = price;

                    // добавляем к количеству перед нами
                    goldCount += count;

                } else {
                    // нашли нашу ставку
                    if (IsOurRateSale(price)) {
                        isRate = true;
                        break;
                    } else {
                        // добавляем к количеству перед нами
                        goldCount += count;
                    }

                }

                index++;

            }

            // наши ставки есть в списке из 8 последних
            if (isRate) {
                addToLog("Ставки есть, до наших - " + goldCount);

                // до на сбольше 40
                if (goldCount > countBeforeMax) {
                    // делаем ставку
                    firstPrice -= priceStep;

                    maxCount = getMaxCountSale();

                    PacketService.sendPacket(Process.GetProcessById(procId).Handle, procId, GoldService.AddGoldSellPacket(firstPrice, maxCount));

                    addToLog("До нас - " + goldCount + ", Делаем ставку: " + firstPrice + " - " + maxCount);

                    supplierCount++;
                }

            } else {
                // ставок нет

                // делаем ставку
                firstPrice -= priceStep;

                maxCount = getMaxCountSale();

                PacketService.sendPacket(Process.GetProcessById(procId).Handle, procId, GoldService.AddGoldSellPacket(firstPrice, maxCount));

                addToLog("Ставок нет, Делаем ставку: " + firstPrice + " - " + maxCount);

                supplierCount++;
            }

        }

        public int getMaxCountBuy(int price) {
            double realPrice = price * 1.02;
            int money = ReadMemoryService.getMoneyAuction(GetHandler());
            int maxCount = getMaxCount();

            int realCount = (int) Math.Floor(money / realPrice);

            if (maxCount < realCount) {
                return maxCount;
            } else {
                return realCount;
            }

        }

        public int getMaxCountSale() {
            int money = ReadMemoryService.getGoldAuction(GetHandler());
            int maxCount = getMaxCount();

            int realCount = (int) Math.Floor(money / 1.02 / 100);

            if (maxCount < realCount) {
                return maxCount;
            } else {
                return realCount;
            }

        }

        // Максимальное количество которое можно купит / продать
        public int getMaxCount() {
            return Int32.Parse(textBoxStep.Text);
        }

        public static bool IsOurRateBuy(int price) {
            foreach (var tmp in supplierBuyArray) {
                if (tmp.Value["price"] == price) {
                    return true;
                }

            }

            return false;
        }

        public static bool IsOurRateSale(int price) {
            foreach (var tmp in supplierSaleArray) {
                if (tmp.Value ["price"] == price) {
                    return true;
                }

            }

            return false;
        }


        public static IntPtr GetHandler() {
            return Process.GetProcessById(procId).Handle;
        }

        private void buttonStop_Click(object sender, EventArgs e) {
            runThread.Abort();
            runThread2.Abort();

            buttonRun.Show();
            buttonStop.Hide();
        }

        private void button5_Click(object sender, EventArgs e) {
            PacketService.sendPacket(
                Process.GetProcessById(procId).Handle, 
                procId, 
                GoldService.AddGoldBuyPacket(Int32.Parse(textBox5.Text), Int32.Parse(textBox6.Text))
            );
        }

        private void button9_Click_1(object sender, EventArgs e) {
            while (true) {
                var persId = BitConverter.GetBytes(ReadMemoryService.getPersId(Process.GetProcessById(Form1.procId).Handle));

                ReadMemoryService.getListGoldSupplier(Process.GetProcessById(procId).Handle, procId, ReadMemoryService.getPackAcceptAddress(Process.GetProcessById(procId).Handle));
                Thread.Sleep(1000);
            }
        }

   

        private void Form1_Deactivate(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseClick_1(object sender, MouseEventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                notifyIcon1.Visible = false;
            }
        }

        private void button10_Click(object sender, EventArgs e) {
            ReadMemoryService.MakeMonye(Process.GetProcessById(procId).Handle, procId);
        }

        private void button11_Click(object sender, EventArgs e) {
            AppService.Start("");
        }

      

        public void MouseLeftClick(int a, int b) {
            Cursor.Position = new Point(a, b);

            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void StartNewWindow() {

            while (File.Exists("lock.lock")) {
                Thread.Sleep(5000);
            }

            // файл уникальности
            File.Create("lock.lock").Close();



            Form1.procId = 0;
            String name = "elementclient";

            Process [] pwProceses = Process.GetProcessesByName(name);

            int [] pwProcessIdArray = new int [pwProceses.Length];

            int count = 0;
            foreach (Process pwProcess in pwProceses) {

                pwProcessIdArray [count] = pwProcess.Id;
                count++;
            }

            while (true) {
                try {
                    AppService.Start(Form1.textServerBox);
                    break;
                } catch (WebException) {
                    Thread.Sleep(5000);
                }

            }

            while (Form1.procId == 0) {
                Thread.Sleep(2000);

                Process [] pwProceses2 = Process.GetProcessesByName(name);

                foreach (Process pwProcess in pwProceses2) {
                    if (pwProcessIdArray.Contains(pwProcess.Id)) {
                        continue;
                    }

                    Form1.procId = pwProcess.Id;

                    break;
                }

            }

            

            Thread.Sleep(20000);

            ReadMemoryService.ActiveServerList(Process.GetProcessById(Form1.procId).Handle);

            // escape для выбора сервера
            PostMessage(Process.GetProcessById(Form1.procId).MainWindowHandle, WM_KEYDOWN, VK_ESCAPE, 0);
            Thread.Sleep(100);
            PostMessage(Process.GetProcessById(Form1.procId).MainWindowHandle, WM_KEYUP, VK_ESCAPE, 0);

            // удаляем файл
            File.Delete("lock.lock");

            Thread.Sleep(10000);

            // enter для входа
            PostMessage(Process.GetProcessById(Form1.procId).MainWindowHandle, WM_KEYDOWN, VK_ENTER, 0);
            Thread.Sleep(100);
            PostMessage(Process.GetProcessById(Form1.procId).MainWindowHandle, WM_KEYUP, VK_ENTER, 0);
            Thread.Sleep(1000);
            PostMessage(Process.GetProcessById(Form1.procId).MainWindowHandle, WM_KEYDOWN, VK_ENTER, 0);
            Thread.Sleep(100);
            PostMessage(Process.GetProcessById(Form1.procId).MainWindowHandle, WM_KEYUP, VK_ENTER, 0);

            Thread.Sleep(30000);

            string userName = ReadMemoryService.getPersName(Process.GetProcessById(Form1.procId).Handle);


            if (userName == "") {
                Form1.runThread.Abort();
            }

            changeName(userName, Form1.textServerBox);
            // замена title окна
            SetWindowText(Process.GetProcessById(Form1.procId).MainWindowHandle, userName + " (" + Form1.textServerBox + ")");

            // выбор аукциониста
            PacketService.sendPacket(Process.GetProcessById(Form1.procId).Handle, Form1.procId, GoldService.selectedNPC);

            while (ReadMemoryService.isLock(Process.GetProcessById(Form1.procId).Handle)) {

                if (ReadMemoryService.getActiveWinName(Process.GetProcessById(Form1.procId).Handle) == "MsgBox_LinkBroken") {
                    Form1.runThread.Abort();
                }

                Thread.Sleep(1000);
            }

            Thread.Sleep(1000);
            PacketService.sendPacket(Process.GetProcessById(Form1.procId).Handle, Form1.procId, GoldService.selectedNPCMenu);
            Thread.Sleep(1000);
            PacketService.sendPacket(Process.GetProcessById(Form1.procId).Handle, Form1.procId, GoldService.RefreshListPacket());
        }

        private void buttonServerSelected_Click(object sender, EventArgs e) {
            buttonRun.Hide();
            buttonStop.Show();

            runThread2 = new Thread(new ThreadStart(check));
            runThread2.Start();
        }

       private void check() {
            runThread = new Thread(new ThreadStart(run));

            runThread.Start();

            while (true) {

                if (!runThread.IsAlive) {
                    Restart();
                }

                Thread.Sleep(5000);
            }
        }

        private void button3_Click(object sender, EventArgs e) {
           PacketService.sendPacket(Process.GetProcessById(procId).Handle, procId, GoldService.RefreshListPacket());
        }

        public int getGoldInAuction() {
            return ReadMemoryService.getGoldAuction(Process.GetProcessById(procId).Handle);
        }

        public int getMoneyInAuction() {
            return ReadMemoryService.getMoneyAuction(Process.GetProcessById(procId).Handle);
        }

        private void button2_Click(object sender, EventArgs e) {
            updateComboProcess();
        }


        private void updateComboProcess() {
            String name = "elementclient";

            Process [] pwProceses = Process.GetProcessesByName(name);

            comboBox1.Items.Clear();

            foreach (Process pwProcess in pwProceses) {
                comboBox1.Items.Add(
                    new {
                        Text = pwProcess.Id + " - " + ReadMemoryService.getPersName(pwProcess.Handle) + " (" + ReadMemoryService.getServerName(pwProcess.Handle) + ")",
                        Value = pwProcess.Id
                    }
                );
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            ReadMemoryService.moneyAuctionAddress = 0;
            ReadMemoryService.goldAuctionAddress = 0;

            Object obj = comboBox1.SelectedItem;
            procId = (int) GetPropValue(obj, "Value");

            textBox2.Text = getMoneyInAuction().ToString();
            textBox1.Text = getGoldInAuction().ToString();
            

            string name = ReadMemoryService.getPersName(Process.GetProcessById(procId).Handle);

            name += " (" + ReadMemoryService.getServerName(Process.GetProcessById(procId).Handle) + ")";

            this.Text = name;
            notifyIcon1.Text = name;
        }

        public static object GetPropValue(object src, string propName) {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ReadMemoryService.ActiveGoldList(Process.GetProcessById(4548).Handle);
        }

        public static string textServerBox = "";

        private void comboBoxServerSelected_SelectedIndexChanged(object sender, EventArgs e) {
            textServerBox = comboBoxServerSelected.SelectedItem.ToString();
        }
    }
}
