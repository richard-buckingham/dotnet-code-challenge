using dotnet_code_challenge.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace dotnet_code_challenge.Test
{
    public class ProcessFeedFilesTests
    {
        [Fact]
        public void ProcessJsonFeed_ExpectRaceName_ThenSortedNameAndPrice()
        {
            // arrange
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string feedFile = $"{Path.GetDirectoryName(path)}\\FeedData\\Wolferhampton_Race1.json";

            ProcessFeedFiles service = new ProcessFeedFiles();

            // act
            List<String> messages = service.ProcessJSONFeed(feedFile);

            // assert
            Assert.Equal(3, messages.Count);
            Assert.Equal("13:45 @ Wolverhampton", messages[0]);
            Assert.Equal("Name: Fikhaar. Price: 4.4.", messages[1]);
            Assert.Equal("Name: Toolatetodelegate. Price: 10.", messages[2]);
        }

        [Fact]
        public void ProcessXMLFeed_ExpectRaceName_ThenSortedNameAndPrice()
        {
            // arrange
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string feedFile = $"{Path.GetDirectoryName(path)}\\FeedData\\Caulfield_Race1.xml";

            ProcessFeedFiles service = new ProcessFeedFiles();

            // act
            List<String> messages = service.ProcessXMLFeed(feedFile);

            // assert
            Assert.Equal(3, messages.Count);
            Assert.Equal("Evergreen Turf Plate", messages[0]);
            Assert.Equal("Name: Advancing. Price: 4.2.", messages[1]);
            Assert.Equal("Name: Coronel. Price: 12.", messages[2]);
        }
    }
}
