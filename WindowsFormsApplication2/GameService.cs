using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace PwResourcesBot {
    class GameService {
        public static int BaseAddress = 0xEFF604;
        public static int GameAddress = 0xEFFDAC;
        public static int PackCall = 0x87A600;
        public static int AutoPath = 0x459740;
        public static int PickWalk = 0x4C4AD0;
        public static int persIdAddress = 0xEFF784;
        public static int OnlineAddress = 0x757C69AC;

        public static int OffsetGame = 0x1C;
        public static int OffsetPersStructure = 0x34;

        // массив действий персонажа
        public static int OffsetPersActionArrayStructure = 0x154C;
        // делаем что либо или нет (1/0)
        // 1 - при хотьбе и при копании / 0 - прерывание между подлетом и копанием
        public static int OffsetPersActionWork = 0x38;

        public static string packetFly = "280001010C00D5B20000";
        public static string packetPetCall = "640000000000";
        public static string packetPetCall2 = "640001000000";
        public static string packetPetReCall = "6500";

        // оффсеты структура персонажа
        public static int offsetPersName = 0x700;
        public static int offsetPersLocX = 0x3C;
        public static int offsetPersLocY = 0x44;
        public static int offsetPersLocZ = 0x40;

    }
}
