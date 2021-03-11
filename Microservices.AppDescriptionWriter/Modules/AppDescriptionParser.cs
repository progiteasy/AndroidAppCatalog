using HtmlAgilityPack;
using MicroservicesCommonData.Models;
using System;

namespace Microservices.AppDescriptionWriter.Modules
{
    public static class AppDescriptionParser
    {
        public static AppModel ReadData(string appPageLink)
        {
            var webClient = new HtmlWeb();

            var readDocument = new HtmlDocument();
            readDocument.LoadHtml(webClient.Load(appPageLink).Text);

            if (readDocument.DocumentNode.SelectSingleNode("//title").InnerHtml == "Not Found")
                throw new ArgumentException();

            var appNameNode = readDocument.DocumentNode.SelectSingleNode("(//h1[@class='AHFaub'])[1]/span");
            var appDownloadsNode = readDocument.DocumentNode.SelectSingleNode("(//span[@class='htlgb'])[6]");

            return new AppModel()
            {
                Name = appNameNode.InnerHtml,
                Downloads = appDownloadsNode.InnerHtml.Replace(',', ' ')
            };
        }
    }
}