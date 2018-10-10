using dotnet_code_challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace dotnet_code_challenge.Services
{
    public class ProcessFeedFiles
    {
        public List<string> ProcessFeedFile(string filename)
        {
            int lastIndex = filename.LastIndexOf('.');
            var ext = filename.Substring(lastIndex + 1).ToLower();

            if (ext == "json")
                return ProcessJSONFeed(filename);
            else if (ext == "xml")
                return ProcessXMLFeed(filename);
            else
                throw new ApplicationException($"cannot process invalid file {filename} with ext: {ext}");
        }

        public List<string> ProcessJSONFeed(string filename)
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

        public List<string> ProcessXMLFeed(string filename)
        {
            List<HorseNamePrice> horseNamePrices;
            List<string> messages;
            meeting meeting;

            DeserialiseXMLFeed(filename, out horseNamePrices, out messages, out meeting);

            var race = meeting.races.race;

            return ConstructMessagesForXMLFeed(horseNamePrices, messages, race);
        }

        private static void DeserialiseXMLFeed(string filename, out List<HorseNamePrice> horseNamePrices, out List<string> messages, out meeting meeting)
        {
            XmlSerializer reader = new XmlSerializer(typeof(meeting));
            horseNamePrices = new List<HorseNamePrice>();
            messages = new List<string>();
            StreamReader file = new StreamReader(filename);
            meeting = (meeting)reader.Deserialize(file);
            file.Close();
        }

        private static List<string> ConstructMessagesForXMLFeed(List<HorseNamePrice> horseNamePrices, List<string> messages, meetingRacesRace race)
        {
            var horsePrices = race.prices.price.horses.ToList();
            race.horses.ToList().ForEach(horse =>
            {
                var price = horsePrices.FirstOrDefault(horsePrice => horsePrice.number == horse.number);
                if (price != null)
                {
                    horseNamePrices.Add(new HorseNamePrice() { Name = horse.name, Price = price.Price });
                }
            });

            messages.Add(race.name);
            horseNamePrices.OrderBy(p => p.Price).ToList().ForEach(hnp =>
            {
                messages.Add($"Name: {hnp.Name}. Price: {hnp.Price}.");
            });

            return messages;
        }

        private class HorseNamePrice
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
    }
}
