using Grpc.Core;
using Microservices.AppDescriptionWriter.Modules;
using MicroservicesCommonData;
using MicroservicesCommonData.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.AppDescriptionWriter.Services
{
    public class AppDescriptionWriterService : MicroservicesCommonData.AppDescriptionWriter.AppDescriptionWriterBase
    {
        private readonly AppCatalogDbContext _dbContext;
        private readonly FileLogger _errorsLogger;

        public AppDescriptionWriterService(AppCatalogDbContext dbContext)
        {
            _dbContext = dbContext;
            _errorsLogger = new FileLogger(Settings.ErrorsLogFileName);
        }

        public override Task<AppDescriptionWriterPostReply> PostData(AppDescriptionWriterPostRequest request, ServerCallContext context)
        {
            try
            {
                var app = _dbContext.Apps.Single(a => a.PackageName == request.AppPackageName);
                var appDescription = AppDescriptionParser.ReadData(AppViewModel.ConvertToPageLink(request.AppPackageName));

                app.Name = appDescription.Name;
                app.Downloads = appDescription.Downloads;

                _dbContext.SaveChanges();

                return Task.FromResult(new AppDescriptionWriterPostReply()
                {
                    IsOperationSuccessful = true,
                    IsRequestArgumentValid = true
                });
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new AppDescriptionWriterPostReply()
                {
                    IsOperationSuccessful = false,
                    IsRequestArgumentValid = false
                });
            }
            catch (Exception ex)
            {
                _errorsLogger.WriteMessage(ex.Message);

                return Task.FromResult(new AppDescriptionWriterPostReply()
                {
                    IsOperationSuccessful = false,
                    IsRequestArgumentValid = true
                });
            }
        }
    }
}