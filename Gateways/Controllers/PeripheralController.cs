using Microsoft.AspNetCore.Mvc;
using System;

namespace Gateways.Controllers
{
    public class PeripheralController : ControllerBase
    {
        private readonly PeripheralService _peripheralService;

        public PeripheralController(PeripheralService peripheralService)
        {
            _peripheralService = peripheralService;
        }

        /// <summary>
        /// Add new peripheral to a gateway
        /// </summary>
        /// <param name="id">Gateway serial ID</param>
        /// <param name="model">Model with the new peripheral data</param>
        /// <returns>JSON with the inserted peripheral, if fails returns status 400 with the error message</returns>
        [HttpPost]
        public IActionResult Add([FromRoute] string id, [FromForm] AddPeripheralModel model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(503);
            }

            var p = new Peripheral
            {
                Id = model.Id,
                Vendor = model.Vendor,
                CreationDate = model.CreationDate,
                IsOnline = model.IsOnline ?? false
            };

            try
            {
                _peripheralService.Add(id, p);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return new JsonResult(p);
        }

        /// <summary>
        /// Update peripheral
        /// </summary>
        /// <param name="id">Gateway serial ID</param>
        /// <param name="subid">Peripheral ID</param>
        /// <param name="model">Model with modified peripheral data</param>
        /// <returns>200 if successfull, otherwise returns 400 with the error message</returns>
        [HttpPut]
        public IActionResult Update([FromRoute] int id, [FromRoute] string subid, [FromForm] UpdatePeripheralModel model)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(503);
            }

            try
            {
                _peripheralService.Update(subid, id, model.Vendor, model.CreationDate, model.IsOnline);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Remove peripheral
        /// </summary>
        /// <param name="id">Gateway serial ID</param>
        /// <param name="subid">Peripheral ID</param>
        /// <returns>200 if successfull, otherwise returns 400 with the error message</returns>
        [HttpDelete]
        public IActionResult Remove([FromRoute] int id, [FromRoute] string subid)
        {
            try
            {
                _peripheralService.Remove(subid, id);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

            return Ok();
        }
    }
}
