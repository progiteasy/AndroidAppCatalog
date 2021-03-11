namespace MicroservicesCommonData.Models
{
    public class AppViewModel
    {
        public int Id { get; private set; }
        public string PageLink { get; private set; }
        public string Name { get; private set; }
        public string Downloads { get; private set; }

        public static string ConvertToPageLink(string packageName) => $"https://play.google.com/store/apps/details?id={packageName}";

        public AppViewModel(int id, string packageName, string name, string downloads)
        {
            Id = id;
            PageLink = ConvertToPageLink(packageName);
            Name = name;
            Downloads = downloads;
        }
    }
}