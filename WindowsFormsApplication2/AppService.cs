using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication2 {
    class AppService {

        public static string serverName = "";
        private static string currenctServerPath = "E:/GamesMailRu/Perfect World/element/userdata/currentserver.ini";

        class ServerData {
            public string port;
            public string url;
            public string login;
            public string password;
            public string userId;
            public string userId2;
        }
        private static Dictionary<string, ServerData> serverDataList = new Dictionary<string, ServerData>() {
            { "Орион", new ServerData { port = "29000", url = "link1.pwonline.ru", login = "freyd16@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Вега", new ServerData { port = "29000", url = "link2.pwonline.ru", login = "freyd.pw2@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Сириус", new ServerData { port = "29000", url = "link3.pwonline.ru", login = "freyd.pw15@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Мира", new ServerData { port = "29000", url = "link4.pwonline.ru", login = "freyd.pw5@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Таразед", new ServerData { port = "29000", url = "link5.pwonline.ru", login = "freyd.pw3@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Альтаир", new ServerData { port = "29000", url = "link6.pwonline.ru", login = "freyd.pw4@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Гелиос", new ServerData { port = "29000", url = "link7.pwonline.ru", login = "", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Атлас", new ServerData { port = "29000", url = "link9.pwonline.ru", login = "freyd.pw11@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Кассиопея", new ServerData { port = "29000", url = "link12.pwonline.ru", login = "freyd.pw12@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Лиридан", new ServerData { port = "29000", url = "link13.pwonline.ru", login = "freyd.pw13@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Гидра", new ServerData { port = "29000", url = "link15.pwonline.ru", login = "genius4@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Луна", new ServerData { port = "29000", url = "link10.pwonline.ru", login = "", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966"} },
            { "Лисичка", new ServerData { port = "29000", url = "link14.pwonline.ru", login = "freyd.pw14@rambler.ru", password = "123456789AA", userId = "2280814267945939651", userId2 = "1434141888741733966" } },
        };

        [DllImport("shell32.dll")]
        private static extern int ShellExecute(int hWnd, string Operation, string File, string Parameters,
                                           string Directory, int nShowCmd);



        public static void Start(string sName) {
            serverName = sName;


            var serverData = serverDataList [serverName];
            string str = "[Server]\r\nCurrentServer=" + sName + "\r\nCurrentServerAddress=" + serverData.port + ":" + serverData.url + "\r\nCurrentLine=0";

            File.WriteAllText(currenctServerPath, str);

            string userId = SendAuth(serverData.userId, serverData.userId2, serverData.login, serverData.password);
            string key = SendAutoLogin(serverData.userId, serverData.userId2, serverData.login, serverData.password);
            string userId2 = SendPersList(serverData.userId, serverData.userId2, serverData.login, serverData.password);

            int i = 0;

            ShellExecute(i, "open", "elementclient.exe", " startbypatcher user:" + userId2 + " _user:" + userId + " token2:" + key, "E:\\GamesMailRu\\Perfect World\\element\\", 7);


        }


        private static string SendAuth (string userId, string userId2, string userName, string password) {
            string strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><Auth ProjectId=\"61\" SubProjectId=\"0\" ShardId=\"0\" UserId=\""+
                userId+"\" UserId2=\""+userId2+"\" Username=\""+ userName + "\" Password=\""+ password + "\" FirstLink=\"_1lp=0&amp;_1ld=2046937_0\"/>";

            HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://authdl.mail.ru/sz.php?hint=Auth");
            req.Method = "POST";
            req.Timeout = 100000;
            req.Host = "authdl.mail.ru";
            req.Accept = "*/*";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Downloader/12480 MailRuGameCenter/1248 Safari/537.36";
            byte [] sentData = Encoding.GetEncoding(1251).GetBytes(strXml);
            req.ContentLength = sentData.Length;
            Stream sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            WebResponse res = req.GetResponse();
            Stream ReceiveStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);

            Char [] read = new Char [256];
            int count = sr.Read(read, 0, 256);
            string Out = String.Empty;
            while (count > 0) {
                String str = new String(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
            }

            var match = Regex.Match(Out, "PersId=\"(.*)\" ");
            return match.Groups [1].ToString();
        }

        private static string SendAutoLogin(string userId, string userId2, string userName, string password) {
            string strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><AutoLogin ProjectId=\"61\" SubProjectId=\"0\" ShardId=\"0\" UserId=\"" +
                userId + "\" UserId2=\"" + userId2 + "\" Username=\"" + userName + "\" Password=\"" + password + "\" FirstLink=\"_1lp=0&amp;_1ld=2046937_0\"/>";

            HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://authdl.mail.ru/sz.php?hint=AutoLogin");
            req.Method = "POST";
            req.Timeout = 100000;
            req.Host = "authdl.mail.ru";
            req.Accept = "*/*";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Downloader/12480 MailRuGameCenter/1248 Safari/537.36";
            byte [] sentData = Encoding.GetEncoding(1251).GetBytes(strXml);
            req.ContentLength = sentData.Length;
            Stream sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            WebResponse res = req.GetResponse();
            Stream ReceiveStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);

            Char [] read = new Char [256];
            int count = sr.Read(read, 0, 256);
            string Out = String.Empty;
            while (count > 0) {
                String str = new String(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
            }

            var match = Regex.Match(Out, "Key=\"(.*)\"");
            return match.Groups [1].ToString();
        }

        private static string SendPersList(string userId, string userId2, string userName, string password) {
            string strXml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><PersList ProjectId=\"61\" SubProjectId=\"0\" ShardId=\"0\" UserId=\"" +
                userId + "\" UserId2=\"" + userId2 + "\" Username=\"" + userName + "\" Password=\"" + password + "\"/>";

            HttpWebRequest req = (HttpWebRequest) WebRequest.Create("https://authdl.mail.ru/sz.php?hint=PersList");
            req.Method = "POST";
            req.Timeout = 100000;
            req.Host = "authdl.mail.ru";
            req.Accept = "*/*";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.110 Downloader/12480 MailRuGameCenter/1248 Safari/537.36";
            byte [] sentData = Encoding.GetEncoding(1251).GetBytes(strXml);
            req.ContentLength = sentData.Length;
            Stream sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            WebResponse res = req.GetResponse();
            Stream ReceiveStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);

            Char [] read = new Char [256];
            int count = sr.Read(read, 0, 256);
            string Out = String.Empty;
            while (count > 0) {
                String str = new String(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
            }

            var match = Regex.Match(Out, " Id=\"([0-9]+)\"");
            return match.Groups [1].ToString();
        }

        public static void ClosePW() {
            Process.GetProcessById(Form1.procId).Kill();

            Form1.procId = 0;
        }

    }
}
