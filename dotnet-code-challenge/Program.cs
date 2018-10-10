using System;
using System.IO;

namespace dotnet_code_challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string feedDirectory = $"{Path.GetDirectoryName(path)}\\FeedData";

            try
            {
                foreach (string filename in Directory.EnumerateFiles(feedDirectory))
                {
                    Console.WriteLine(filename);

                }

                Console.Read();
            }
            catch (Exception ex)
            {

                Console.Read();
            }


        }
    }
}
