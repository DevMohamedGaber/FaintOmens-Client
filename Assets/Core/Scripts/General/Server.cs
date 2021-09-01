using System;
using Mirror;
namespace Game
{
    public static class Server
    {
        public static int number;
        public static string name;
        public static DateTime createdAt;
        public static TimeZoneInfo timeZone;
        public static DateTime time => DateTime.Now;//TimeZoneInfo.ConvertTime(DateTime.Now, timeZone);
        public static bool IsPlayerIdWithInServer(uint id)
        {
            return id < (number * 10000000);
        }
    }
}