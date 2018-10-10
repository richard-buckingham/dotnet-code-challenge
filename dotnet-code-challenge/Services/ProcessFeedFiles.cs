using dotnet_code_challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace dotnet_code_challenge.Services
{
    public class ProcessFeedFiles
    {
        public List<string> ProcessFeedFile(string filename)
        {
            int lastIndex = filename.LastIndexOf('.');
            var ext = filename.Substring(lastIndex + 1).ToLower();

            if (ext == "json")
            {
                return ProcessJSONFeed(filename);
            }

            return new List<string>();
        }

        private List<string> ProcessJSONFeed(string filename)
        {
            List<string> messages = new List<string>();

            using (StreamReader file = File.OpenText(filename))
            {
                List<Selection> selections = new List<Selection>();

                JsonSerializer serializer = new JsonSerializer();
                JSONFeedModel feedModel = (JSONFeedModel)serializer.Deserialize(file, typeof(JSONFeedModel));

                feedModel.RawData.Markets.ToList().ForEach(market =>
                {
                    selections.AddRange(market.Selections);
                });

                messages.Add(feedModel.RawData.FixtureName);
                selections.OrderBy(s => s.Price).ToList().ForEach(selection =>
                {
                    messages.Add($"Name: {selection.Tags.name}. Price: {selection.Price}.");
                });
            }

            return messages;
        }
    }
}
