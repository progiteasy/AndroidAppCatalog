namespace Microservices.AppCatalogAPI
{
    public static class Settings
    {
        public const string ErrorsLogFileName = "Errors.log";
        public static string AppCatalogDbConnectionString { get; set; }
        public static string AppDescriptionWriterServiceAddress { get; set; }
    }
}