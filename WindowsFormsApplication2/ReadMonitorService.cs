using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2 {
    class ReadMonitorService {
        [DllImport("gdi32.dll")]
        private static extern int BitBlt(IntPtr srchDc, int srcX, int srcY, int srcW, int srcH,
                                 IntPtr desthDc, int destX, int destY, int op);


        public static bool number1_1, number1_2, number1_3, number1_4, number1_5, number1_6 = false;
        public static bool number2_1, number2_2, number2_3, number2_4, number2_5, number2_6 = false;
        public static bool number3_1, number3_2, number3_3, number3_4, number3_5, number3_6 = false;
        public static bool number4_1, number4_2, number4_3, number4_4, number4_5, number4_6 = false;
        public static bool number5_1, number5_2, number5_3, number5_4, number5_5, number5_6 = false;
        public static bool number6_1, number6_2, number6_3, number6_4, number6_5, number6_6 = false;
        public static bool number7_1, number7_2, number7_3, number7_4, number7_5, number7_6 = false;
        public static bool number8_1, number8_2, number8_3, number8_4, number8_5, number8_6 = false;
        public static bool number9_1, number9_2, number9_3, number9_4, number9_5, number9_6 = false;
        public static bool number10_1, number10_2, number10_3, number10_4, number10_5, number10_6 = false;

        public static Color GetPixel2(IntPtr hwnd, int x, int y) {
            Bitmap screenPixel = new Bitmap(1, 1);
            using (screenPixel = new Bitmap(1, 1)) {
                using (Graphics gdest = Graphics.FromImage(screenPixel)) {
                    using (Graphics gsrc = Graphics.FromHwnd(hwnd)) {
                        IntPtr hsrcdc = gsrc.GetHdc();
                        IntPtr hdc = gdest.GetHdc();
                        BitBlt(hdc, 0, 0, 1, 1, hsrcdc, x, y, (int) CopyPixelOperation.SourceCopy);
                        gdest.ReleaseHdc();
                        gsrc.ReleaseHdc();
                    }
                }

                return screenPixel.GetPixel(0, 0);
            }
        }


        public string [] getGoldList(IntPtr hwnd, int x1, int x2, int y1, int y2, int type) {

            int lenY = y2 - y1 + 1;
            int lenX = x2 - x1 + 1;

            string [] resultArray = new string [10];
            bool [,] array2 = new bool [lenX, lenY];

            int indexX = 0;
            int indexY = 0;

            int firstX = 0;
            int firstY = 0;

            uint hash = 0;
            bool isBoolYellow = false;

            for (int x = x1; x <= x2; x++) {
                for (int y = y1; y <= y2; y++) {
                    hash = (uint) GetPixel2(hwnd, x, y).GetHashCode();

                    if (type == 7) {
                        isBoolYellow = isGreen(GetPixel2(hwnd, x, y).R, GetPixel2(hwnd, x, y).G, GetPixel2(hwnd, x, y).B) || isYellow(hash);
                    } else if (type == 2 || type == 4 || type == 6) {
                        isBoolYellow = isGreen(GetPixel2(hwnd, x, y).R, GetPixel2(hwnd, x, y).G, GetPixel2(hwnd, x, y).B);
                    } else {
                        isBoolYellow = isYellow(hash);
                    }

                    array2 [indexX, indexY] = isBoolYellow;
                    if ((firstX == 0 || firstX > indexX) && isBoolYellow) {
                        firstX = indexX;
                    }

                    if (type == 5 || type == 6) {
                        if ((firstY == 0 || firstY > indexY) && (isGreen(GetPixel2(hwnd, x, y).R, GetPixel2(hwnd, x, y).G, GetPixel2(hwnd, x, y).B) || isYellow(hash))) {
                            firstY = indexY;
                        }
                    } else if ((firstY == 0 || firstY > indexY) && isBoolYellow) {
                        firstY = indexY;
                    }

                    indexY++;
                }

                indexX++;
                indexY = 0;
            }




            int indexXYellow = firstX;
            int indexYYellow = firstY;



            string str = "";

            bool isPrice = true;
            bool isCount = false;
            bool nextLine = false;

            int tmpIndexXYellow = indexXYellow;
            int tmpIndexYYellow = indexYYellow;

            int indexAdd = 6;

            int index = 0;

            while (true) {
                indexAdd = 6;
                nextLine = false;

                if (indexYYellow + 9 >= lenY) {
                    break;
                } else if (indexXYellow + 5 >= lenX) {
                    indexXYellow = tmpIndexXYellow;
                    indexYYellow += 15;

                    resultArray [index] = str;
                    index++;

                    str = "";
                    continue;
                }

                number1_1 = array2 [indexXYellow, indexYYellow];
                number1_2 = array2 [indexXYellow + 1, indexYYellow];
                number1_3 = array2 [indexXYellow + 2, indexYYellow];
                number1_4 = array2 [indexXYellow + 3, indexYYellow];
                number1_5 = array2 [indexXYellow + 4, indexYYellow];
                number1_6 = array2 [indexXYellow + 5, indexYYellow];

                number2_1 = array2 [indexXYellow, indexYYellow + 1];
                number2_2 = array2 [indexXYellow + 1, indexYYellow + 1];
                number2_3 = array2 [indexXYellow + 2, indexYYellow + 1];
                number2_4 = array2 [indexXYellow + 3, indexYYellow + 1];
                number2_5 = array2 [indexXYellow + 4, indexYYellow + 1];
                number2_6 = array2 [indexXYellow + 5, indexYYellow + 1];

                number3_1 = array2 [indexXYellow, indexYYellow + 2];
                number3_2 = array2 [indexXYellow + 1, indexYYellow + 2];
                number3_3 = array2 [indexXYellow + 2, indexYYellow + 2];
                number3_4 = array2 [indexXYellow + 3, indexYYellow + 2];
                number3_5 = array2 [indexXYellow + 4, indexYYellow + 2];
                number3_6 = array2 [indexXYellow + 5, indexYYellow + 2];

                number4_1 = array2 [indexXYellow, indexYYellow + 3];
                number4_2 = array2 [indexXYellow + 1, indexYYellow + 3];
                number4_3 = array2 [indexXYellow + 2, indexYYellow + 3];
                number4_4 = array2 [indexXYellow + 3, indexYYellow + 3];
                number4_5 = array2 [indexXYellow + 4, indexYYellow + 3];
                number4_6 = array2 [indexXYellow + 5, indexYYellow + 3];

                number5_1 = array2 [indexXYellow, indexYYellow + 4];
                number5_2 = array2 [indexXYellow + 1, indexYYellow + 4];
                number5_3 = array2 [indexXYellow + 2, indexYYellow + 4];
                number5_4 = array2 [indexXYellow + 3, indexYYellow + 4];
                number5_5 = array2 [indexXYellow + 4, indexYYellow + 4];
                number5_6 = array2 [indexXYellow + 5, indexYYellow + 4];

                number6_1 = array2 [indexXYellow, indexYYellow + 5];
                number6_2 = array2 [indexXYellow + 1, indexYYellow + 5];
                number6_3 = array2 [indexXYellow + 2, indexYYellow + 5];
                number6_4 = array2 [indexXYellow + 3, indexYYellow + 5];
                number6_5 = array2 [indexXYellow + 4, indexYYellow + 5];
                number6_6 = array2 [indexXYellow + 5, indexYYellow + 5];

                number7_1 = array2 [indexXYellow, indexYYellow + 6];
                number7_2 = array2 [indexXYellow + 1, indexYYellow + 6];
                number7_3 = array2 [indexXYellow + 2, indexYYellow + 6];
                number7_4 = array2 [indexXYellow + 3, indexYYellow + 6];
                number7_5 = array2 [indexXYellow + 4, indexYYellow + 6];
                number7_6 = array2 [indexXYellow + 5, indexYYellow + 6];

                number8_1 = array2 [indexXYellow, indexYYellow + 7];
                number8_2 = array2 [indexXYellow + 1, indexYYellow + 7];
                number8_3 = array2 [indexXYellow + 2, indexYYellow + 7];
                number8_4 = array2 [indexXYellow + 3, indexYYellow + 7];
                number8_5 = array2 [indexXYellow + 4, indexYYellow + 7];
                number8_6 = array2 [indexXYellow + 5, indexYYellow + 7];

                number9_1 = array2 [indexXYellow, indexYYellow + 8];
                number9_2 = array2 [indexXYellow + 1, indexYYellow + 8];
                number9_3 = array2 [indexXYellow + 2, indexYYellow + 8];
                number9_4 = array2 [indexXYellow + 3, indexYYellow + 8];
                number9_5 = array2 [indexXYellow + 4, indexYYellow + 8];
                number9_6 = array2 [indexXYellow + 5, indexYYellow + 8];

                number10_1 = array2 [indexXYellow, indexYYellow + 9];
                number10_2 = array2 [indexXYellow + 1, indexYYellow + 9];
                number10_3 = array2 [indexXYellow + 2, indexYYellow + 9];
                number10_4 = array2 [indexXYellow + 3, indexYYellow + 9];
                number10_5 = array2 [indexXYellow + 4, indexYYellow + 9];
                number10_6 = array2 [indexXYellow + 5, indexYYellow + 9];

                if (str == "" && !number1_1 && !number2_1 && !number3_1 && !number4_1 && !number5_1 && !number6_1 && !number7_1 && !number8_1 && !number9_1 && !number10_1) {
                    indexXYellow++;
                    continue;
                }
                if (type == 5 || type == 6) {
                    string myChar = makeChar();

                    if (myChar == "П") {
                        str += myChar;
                        nextLine = true;
                    }

                } else if (!number1_1 && !number2_2 && !number3_3 && !number4_4 && !number5_5 && !number6_6
                    && !number10_1 && !number9_2 && !number8_3 && !number7_4 && !number6_5 && !number5_6
                    && !number9_2 && !number9_3
                ) {

                    if (str != "") {
                        nextLine = true;
                    }
                } else if (number9_2 && number9_3 && number10_2 && number10_3
                    && !number1_1 && !number1_2 && !number1_3
                    && !number2_1 && !number2_2 && !number2_3
                    && !number3_1 && !number3_2 && !number3_3
                    && !number4_1 && !number4_2 && !number4_3
                    && !number5_1 && !number5_2 && !number5_3
                    && !number6_1 && !number6_2 && !number6_3
                    && !number7_1 && !number7_2 && !number7_3
                    && !number8_1 && !number8_2 && !number8_3
                ) {
                    str += ",";
                    indexAdd = 3;
                } else if ((!number1_6 && !number2_6 && !number3_6 && !number4_6 && !number5_6 && !number6_6 && !number7_6 && !number8_6 && !number9_6 && !number10_6
                    && number1_5 && number2_5 && number3_5 && number4_5 && number5_5 && number6_5 && number7_5 && number8_5 && number9_5 && number10_5
                    && number1_4 && number2_4 && number3_4 && number4_4 && number5_4 && number6_4 && number7_4 && number8_4 && number9_4 && number10_4)

                    ) {
                    str += "1";

                } else if (!number1_5 && !number2_5 && !number3_5 && !number4_5 && !number5_5 && !number6_5 && !number7_5 && !number8_5 && !number9_5 && !number10_5
                    && number1_4 && number2_4 && number3_4 && number4_4 && number5_4 && number6_4 && number7_4 && number8_4 && number9_4 && number10_4
                    && number1_3 && number2_3 && number3_3 && number4_3 && number5_3 && number6_3 && number7_3 && number8_3 && number9_3 && number10_3
                    ) {
                    str += "1";
                    indexAdd = 5;

                } else if (
                    !number6_6 && !number9_1 && !number9_2 && !number9_3 && !number9_6 && !number10_1 && !number10_2 && !number10_3 && !number10_6
                    && number8_1 && number8_2 && number8_3 && number8_4 && number8_5 && number8_6 && number9_4 && number9_5 && number10_4 && number10_5
                ) {
                    str += "4";

                } else if (
                    !number1_1 && !number1_6 && !number10_1 && !number10_6 && !number3_4 && !number4_4 && !number5_4 && !number6_4 && !number7_4 && !number8_4
                    && !number4_3 && !number5_3 && !number6_3 && !number7_3
                    && number2_3 && number2_4 && number9_2 && number9_3 && number5_2 && number6_2 && number5_5 && number5_6
                ) {
                    str += "0";
                } else if (
                    !number1_1 && !number1_6 && !number3_4 && !number4_3 && !number4_4 && !number5_4 && !number8_4 && !number10_1 && !number10_6
                    && number2_2 && number3_2 && number4_2 && number5_2 && number6_2 && number6_3 && number6_4 && number6_5 && number6_6
                    && number7_6 && number5_6 && number4_6 && number8_5 && number9_5 && number10_3 && number10_4 && number8_2 && number9_2
                ) {
                    str += "9";
                } else if (
                    !number1_1 && !number1_6 && !number10_1 && !number3_4 && !number7_4 && !number8_4 && !number5_1
                    && number1_3 && number1_4 && number5_2 && number5_3 && number5_4 && number5_5 && number10_3 && number10_4
                    && number2_2 && number3_2 && number4_2 && number6_2 && number7_2 && number8_2 && number9_2
                    && number2_5 && number3_5 && number4_5 && number6_5 && number7_5 && number8_5 && number9_5

                ) {
                    str += "8";

                } else if (
                    !number3_1 && !number4_1 && !number5_1 && !number6_1 && !number7_1 && !number8_1 && !number9_1 && !number10_1
                     && !number3_2 && !number4_2 && !number5_2 && !number6_2 && !number7_2 && !number3_3 && !number4_3
                     && !number4_6 && !number5_6 && !number6_6 && !number7_6 && !number8_6 && !number9_6 && !number10_6
                     && !number6_5 && !number7_5 && !number8_5 && !number9_5 && !number10_5
                     && number2_2 && number2_3 && number2_4 && number2_5 && number2_6
                     && number3_5 && number4_4 && number5_4 && number6_4 && number7_3 && number8_3 && number9_3 && number10_3
                ) {
                    str += "7";
                } else if (!number1_1 && !number2_1 && !number1_6 && !number10_1 && !number3_4 && !number4_6 && !number7_4 && !number8_4
                     && number2_2 && number2_3 && number2_4 && number2_5 && number3_5 && number3_2 && number4_2 && number5_2 && number6_2
                     && number7_2 && number8_2 && number9_2 && number5_3 && number5_4 && number5_5 && number6_6 && number7_6 && number8_6
                     && number9_5 && number10_4 && number10_3
                ) {
                    str += "6";

                } else if (
                    !number1_1 && !number2_1 && !number3_1 && !number3_4 && !number3_5 && !number3_6 && !number4_6 && !number10_1
                     && !number7_1 && !number7_3 && !number7_4 && !number6_4 && !number8_4
                     && number2_2 && number2_3 && number2_4 && number2_5 && number2_6 && number3_2 && number4_2
                     && number5_2 && number5_3 && number5_4 && number5_5 && number6_6 && number7_6 && number8_6 && number9_5
                     && number10_4 && number10_3 && number9_2 && number8_2
                ) {
                    str += "5";
                } else if (
                    !number1_1 && !number1_6 && !number3_4 && !number4_3 && !number4_4 && !number5_1 && !number5_2 && !number5_3 && !number6_1 && !number6_2
                    && !number7_1 && !number8_1 && !number6_6 && !number7_6 && !number8_5 && !number8_6
                    && number1_3 && number1_4 && number1_5 && number2_2 && number2_3 && number2_4 && number2_5
                    && number3_2 && number3_5 && number3_6 && number4_5 && number4_6 && number5_5 && number6_4 && number6_5
                    && number7_3 && number7_4 && number8_2 && number8_3
                    && number9_2 && number9_3 && number9_4 && number9_5 && number9_6
                    && number10_2 && number10_3 && number10_4 && number10_5 && number10_6
                ) {
                    str += "2";
                } else if (
                    !number1_1 && !number1_6 && !number3_4 && !number4_1 && !number4_3 && !number5_1 && !number5_2 && !number6_1 && !number6_2
                    && !number7_1 && !number7_3 && !number7_4 && !number8_4
                    && number3_2 && number2_2 && number2_3 && number2_4 && number2_5 && number3_5 && number4_5 && number5_5 && number6_5
                    && number5_4 && number7_6 && number8_6 && number9_5 && number10_4 && number10_3 && number9_2 && number8_2
                ) {
                    str += "3";
                }


                if (nextLine) {
                    indexXYellow = tmpIndexXYellow;
                    indexYYellow += 15;

                    resultArray [index] = str;
                    str = "";
                    index++;




                } else {
                    indexXYellow += indexAdd;
                }


            }

            return resultArray;
        }

        public static bool isYellow(uint color) {

            if (color <= 0xFFFFFF80 && color >= 0xFF645D0A) {
                return true;
            }

            return false;

        }

        public static bool isGreen(byte R, byte G, byte B) {



            if (R <= 0x23 && G >= 0x6E && B <= 0x14) {
                return true;
            }

            return false;

        }

        public static string makeChar() {
            if (number1_1 && number2_1 && number3_1 && number4_1 && number5_1 && number6_1 && number7_1 && number8_1 && number9_1 && !number10_1
                && number1_2 && number2_2 && number3_2 && number4_2 && number5_2 && number6_2 && number7_2 && number8_2 && number9_2 && !number10_2
                && !number3_3 && !number4_3 && !number5_3 && !number6_3 && !number7_3 && !number8_3 && !number9_3 && !number10_3
            ) {
                return "П";
            }

            return "";
        }
    }
}
