using Application.AccessSystem;
using Application.ProxyServices;
using CloudDrive.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace CloudDrive.Controllers
{
    //TODO: вынести в отдельный файл
    public class ProxyDto
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
    }

    [ApiController]
    [Authorize]
    [Route("api")]
    public class ProxyController : ControllerBase
    {
        private readonly IAccessService _accessService;
        private readonly IProxyService _proxyService;

        public ProxyController(IAccessService accessService, IProxyService proxyService)
        {
            _accessService = accessService;
            _proxyService = proxyService;
        }

        [HttpPost]
        [Route("proxy/{key}/{id}")]
        public async Task<IActionResult> Proxy([FromRoute] string key,
            [FromRoute] string id,
            [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] JsonElement body)
        {
            string userId = User.GetUserId();

            bool hasAccess = await _accessService.HasAccess(userId, id);

            if (!hasAccess)
            {
                return Forbid();
            }

            string json = string.Empty;

            if (body.ValueKind != JsonValueKind.Undefined)
            {
                json = body.GetRawText();
            }

            HttpResponseMessage response = _proxyService.ResoveRequest(key, id, json);

            string responseJson = await response.Content.ReadAsStringAsync();

            Response.Headers.Add("Content-Type", "application/json");
            return StatusCode((int) response.StatusCode, responseJson);
        }
    }
}
