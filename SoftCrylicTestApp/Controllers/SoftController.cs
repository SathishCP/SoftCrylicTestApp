using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;

namespace SoftCrylicTestApp.Controllers
{
    [BasicAuthentication()]
    public class SoftController : ApiController
    {
        [Route("Create"), HttpPost(), ActionName("Create")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Create([FromUri]Models.EventManager request)
        {
            try
            {
                int response = SoftUtils.CreateEvent(request);

                return await System.Threading.Tasks.Task.FromResult(Ok(response));
            }
            catch (Exception ex)
            {
                return await System.Threading.Tasks.Task.FromResult(Ok(string.Format("{0} {1}", ex.Message, ex.InnerException.Message)));
            }
        }

        [Route("Read"), HttpGet(), ActionName("Read")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Read([FromUri] string Id)
        {
            try
            {
                Models.Events events = SoftUtils.GetEvents(Id);

                return await System.Threading.Tasks.Task.FromResult(Ok(events));
            }
            catch(Exception ex)
            {
                return await System.Threading.Tasks.Task.FromResult(Ok(string.Format("{0} {1}", ex.Message, ex.InnerException.Message)));
            }
        }

        [Route("Update"), HttpPut(), ActionName("Update")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Update([FromBody]Models.EventManager request)
        {
            try
            {
                int response = SoftUtils.UpdateEvent(request);

                return await System.Threading.Tasks.Task.FromResult(Ok(response));
            }
            catch (Exception ex)
            {
                return await System.Threading.Tasks.Task.FromResult(Ok(string.Format("{0} {1}", ex.Message, ex.InnerException.Message)));
            }
        }

        [Route("Delete"), HttpDelete(), ActionName("Delete")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Delete([FromUri]string Id)
        {
            try
            {
                int response = SoftUtils.DeleteEvent(Id);

                return await System.Threading.Tasks.Task.FromResult(Ok(response));
            }
            catch (Exception ex)
            {
                return await System.Threading.Tasks.Task.FromResult(Ok(string.Format("{0} {1}", ex.Message, ex.InnerException.Message)));
            }
        }

        [Route("Download"), HttpGet(), ActionName("Download")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Download([FromUri]string Id)
        {
            try
            {
                Models.Events events = SoftUtils.GetEvents(string.Empty);

                using (System.Data.DataTable dt = SoftUtils.ConvertListToDataTable<Models.EventManager>(events.EventManager))
                {
                    byte[] byteArray = SoftUtils.ExportDataTableIntoMultipleExcelSheets(dt);

                    System.IO.MemoryStream stream = new System.IO.MemoryStream();

                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(stream.GetBuffer())
                    };
                    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "SoftCrylicEvents.xls"
                    };
                    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                    var response = ResponseMessage(result);

                    return await System.Threading.Tasks.Task.FromResult(Ok(response));
                }
            }
            catch (Exception ex)
            {
                return await System.Threading.Tasks.Task.FromResult(Ok(string.Format("{0} {1}", ex.Message, ex.InnerException.Message)));
            }
        }
    }
}