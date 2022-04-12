using Microsoft.AspNetCore.Mvc;
using System;

namespace Gateways.Controllers
{
    public class GatewayController : ControllerBase
    {
        private readonly GatewayService _gatewayService;

        public GatewayController(GatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        /// <summary>
        /// Get all gateways data
        /// </summary>
        /// <returns>JSON with all gateways data</returns>
        [HttpGet]
        public IActionResult All()
        {
            return new JsonResult(_gatewayService.AllGateways());
        }

        /// <summary>
        /// Get all gateway data
        /// </summary>
        /// <param name="id">Gateway serial ID</param>
        /// <returns>JSON with the specified gateway ID data</returns>
        [HttpGet]
        public IActionResult Details([FromRoute] string id)
        {
            return new JsonResult(_gatewayService.GetById(id));
        }

        /// <summary>
        /// Add a new gateway
        /// </summary>
        /// <param name="model">Model with the new gateway data</param>
        /// <returns>JSON with the inserted gateway, otherwise returns 400 with the error message</returns>
        [HttpPost]
        public IActionResult Add([FromForm] AddGatewayModel model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(503);
            }

            var g = new Gateway
            {
                SerialNumber = model.SerialNumber,
                Name = model.Name,
                IP = model.IP
            };

            try
            {
                _gatewayService.Add(g);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return new JsonResult(g);
        }

        /// <summary>
        /// Update a gateway
        /// </summary>
        /// <param name="id">Gateway serial ID</param>
        /// <param name="model">Model with modified data</param>
        /// <returns>200 if successfull, otherwise returns 400 with the error message</returns>
        [HttpPut]
        public IActionResult Update([FromRoute] string id, [FromForm] UpdateGatewayModel model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(503);
            }

            try
            {
                _gatewayService.Update(id, model.Name, model.IP);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Remove a gateway
        /// </summary>
        /// <param name="id">Gateway serial ID</param>
        /// <returns>200 if successfull, otherwise returns 400 with the error message</returns>
        [HttpDelete]
        public IActionResult Remove([FromRoute] string id)
        {
            try
            {
                _gatewayService.Remove(id);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return Ok();
        }
    }
}
