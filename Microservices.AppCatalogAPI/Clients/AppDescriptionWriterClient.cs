using Grpc.Net.Client;
using MicroservicesCommonData;

namespace Microservices.AppCatalogAPI.Modules
{
    public static class AppDescriptionWriterClient
    {
        public static AppDescriptionWriterPostReply PostData(string appPackageName)
        {
            using var grpcChannel = GrpcChannel.ForAddress(Settings.AppDescriptionWriterServiceAddress);
            var appDescriptionWriterClient = new AppDescriptionWriter.AppDescriptionWriterClient(grpcChannel);
            var appDescriptionWriterPostReply = appDescriptionWriterClient.PostData(new AppDescriptionWriterPostRequest() { AppPackageName = appPackageName });
            
            return appDescriptionWriterPostReply;
        }
    }
}