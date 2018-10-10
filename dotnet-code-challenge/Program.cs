using dotnet_code_challenge.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace dotnet_code_challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string feedDirectory = $"{Path.GetDirectoryName(path)}\\FeedData";

            var processFeeds = new ProcessFeedFiles();
            List<string> messages = new List<string>();

            try
            {
                foreach (string filename in Directory.EnumerateFiles(feedDirectory))
                {
                    messages = processFeeds.ProcessFeedFile(filename);
                    messages.ForEach(message =>
                    {
                        Console.WriteLine(message);
                    });
                    Console.WriteLine(String.Empty);
                }

                // TODO: move file to "processed" location.
                Console.Write("press enter to continue");
                Console.Read();

                Console.Read();
            }
            catch (Exception ex)
            {

                Console.Read();
            }


        }
    }

    internal class ProcessFeeds
    {
        public ProcessFeeds()
        {
        }
    }
}
