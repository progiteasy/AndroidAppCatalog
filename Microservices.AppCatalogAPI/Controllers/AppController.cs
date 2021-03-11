using Microservices.AppCatalogAPI.Modules;
using MicroservicesCommonData;
using MicroservicesCommonData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microservices.AppCatalogAPI.Controllers
{
    [Route("api/app")]
    public class AppController : Controller
    {
        private readonly AppCatalogDbContext _dbContext;
        private readonly FileLogger _errorsLogger;

        public AppController(AppCatalogDbContext dbContext)
        {
            _dbContext = dbContext;
            _errorsLogger = new FileLogger(Settings.ErrorsLogFileName);
        }

        [HttpGet]
        public IActionResult GetApp([FromBody]int appId)
        {
            if (appId <= 0)
                return BadRequest("Значение AppId должно быть больше 0");

            try
            {
                var app = _dbContext.Apps.Find(appId);

                if (app == null)
                    return NotFound("Приложение с заданным AppId в базе данных не найдено");

                return Ok(new AppViewModel(app.Id, app.PackageName, app.Name, app.Downloads));
            }
            catch (Exception ex)
            {
                _errorsLogger.WriteMessage(ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult PostApp([FromBody]string appPageLink)
        {
            if (appPageLink == null)
                return BadRequest("Значение AppPageLink не может быть null");

            appPageLink = appPageLink.Trim().ToLower();

            if (!Regex.IsMatch(appPageLink, @"^(http(s)?:(\/){2})?play\.google\.com\/store\/apps\/details\?id\=([a-z]([a-z]|[0-9])+\.)+[a-z]([a-z]|[0-9])+(&|$)"))
                return BadRequest("Недопустимое значение AppPageLink");

            try
            {
                appPageLink = appPageLink.Split('&')[0];

                var appPackageName = appPageLink.Split('=')[1];
                var app = _dbContext.Apps.FirstOrDefault(a => a.PackageName == appPackageName);

                if (app != null)
                    return Conflict($"Приложение уже существует в базе данных: AppId = {app.Id}");

                app = new AppModel() { PackageName = appPackageName };

                _dbContext.Apps.Add(app);
                _dbContext.SaveChanges();

                AppDescriptionWriterPostReply appDescriptionWriterPostReply = AppDescriptionWriterClient.PostData(appPackageName);

                if (!appDescriptionWriterPostReply.IsRequestArgumentValid)
                {
                    return BadRequest("Неверное значение AppPageLink");
                }
                else if (!appDescriptionWriterPostReply.IsOperationSuccessful)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                return Ok($"Приложение успешно добавлено в базу данных: AppId = {app.Id}");
            }
            catch (Exception ex)
            {
                _errorsLogger.WriteMessage(ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            finally
            {
                var app = _dbContext.Apps.SingleOrDefault(a => (a.Name == null) && (a.Downloads == null));

                if (app != null)
                {
                    _dbContext.Apps.Remove(app);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}