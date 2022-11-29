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
        [Route("Inquiry"), HttpGet(), ActionName("Inquiry")]
        public async System.Threading.Tasks.Task<IHttpActionResult> Inquiry([FromUri] string REQ_UNIQUE_ID)
        {
            return await System.Threading.Tasks.Task.FromResult(Ok(DateTime.Now.ToLongDateString()));
        }
        /*[Route("MandateInquiry"), HttpGet(), ActionName("MandateInquiry")]
        public async System.Threading.Tasks.Task<IHttpActionResult> MandateInquiry([FromUri] string REQ_UNIQUE_ID)
        { }

        [Route("MandateIngress"), HttpPost(), ActionName("MandateIngress")]
        public async System.Threading.Tasks.Task<IHttpActionResult> MandateIngress([FromBody]MandateRequest request)
        { }

        [Route("MandateIngress"), HttpPut(), ActionName("MandateIngress")]
        public async System.Threading.Tasks.Task<IHttpActionResult> MandateIngress([FromBody]MandateRequest request)
        { }*/
    }
}