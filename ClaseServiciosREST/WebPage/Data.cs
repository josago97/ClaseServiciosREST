using System;

namespace WebPage
{
    public static class Data
    {
        public static string Nickname { get; private set; }

        static Data()
        {
            GetData();
        }

        private static void GetData()
        {
            Nickname = Environment.GetEnvironmentVariable("Nickname");
        }
    }
}
