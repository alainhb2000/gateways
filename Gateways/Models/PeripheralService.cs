using System;
using System.Linq;

namespace Gateways
{
    public class PeripheralService
    {
        private readonly GatewaysContext _gatewayRepo;
        private readonly GatewayService _gatewayService;

        public PeripheralService(GatewaysContext gatewayRepo, GatewayService gatewayService)
        {
            _gatewayRepo = gatewayRepo;
            _gatewayService = gatewayService;
        }
        
        /// <summary>
        /// Add a new peripheral
        /// </summary>
        /// <param name="gatewaySerialNumber">Gateway serial number</param>
        /// <param name="peripheral">New peripheral data</param>
        /// <exception cref="Exception">If gateway was not found</exception>
        /// <exception cref="Exception">If peripherals are greater than 10</exception>
        /// <exception cref="Exception">If peripheral ID is already used</exception>
        public void Add(string gatewaySerialNumber, Peripheral peripheral)
        {
            var gateway = _gatewayService.GetById(gatewaySerialNumber);
            if (gateway == null)
                throw new Exception($"Gateway {gatewaySerialNumber} not found.");

            if (gateway.Peripherals.Count + 1 > 10)
                throw new Exception($"Gateways can't have more than 10 peripherals.");

            if (gateway.Peripherals.Exists(pr => pr.Id == peripheral.Id))
                throw new Exception($"Peripheral ID {peripheral.Id} already used.");

            gateway.Peripherals.Add(peripheral);
            _gatewayRepo.Gateways.Update(gateway);
        }

        /// <summary>
        /// Update peripheral
        /// </summary>
        /// <param name="gatewaySerialNumber">Gateway serial number</param>
        /// <param name="peripheralId">Peripheral ID</param>
        /// <param name="vendor">Vendor</param>
        /// <param name="creationDate">Creation date</param>
        /// <param name="isOnline">Is Online</param>
        /// <exception cref="Exception">If gateway was not found</exception>
        /// <exception cref="Exception">If peripheral was not found</exception>
        public void Update(string gatewaySerialNumber, int peripheralId, string vendor, DateTime creationDate, bool? isOnline)
        {
            var gateway = _gatewayService.GetById(gatewaySerialNumber);
            if (gateway == null)
                throw new Exception($"Gateway {gatewaySerialNumber} not found.");

            var peripheral = gateway.Peripherals.FirstOrDefault(p => p.Id == peripheralId);
            if (peripheral == null)
                throw new Exception($"Perpheral {peripheralId} not found.");

            peripheral.Vendor = vendor;
            peripheral.CreationDate = creationDate;
            peripheral.IsOnline = isOnline ?? false;
            _gatewayRepo.Gateways.Update(gateway);
        }

        /// <summary>
        /// Removes a peripheral
        /// </summary>
        /// <param name="gatewaySerialNumber">Gateway serial number</param>
        /// <param name="peripheralId">Peripheral ID</param>
        /// <exception cref="Exception">If gateway was not found</exception>
        /// <exception cref="Exception">If peripheral was not found</exception>
        public void Remove(string gatewaySerialNumber, int peripheralId)
        {
            var gateway = _gatewayService.GetById(gatewaySerialNumber);
            if (gateway == null)
                throw new Exception($"Gateway {gatewaySerialNumber} not found.");

            var peripheral = gateway.Peripherals.FirstOrDefault(p => p.Id == peripheralId);
            if (peripheral == null)
                throw new Exception($"Perpheral {peripheralId} not found.");

            gateway.Peripherals.Remove(peripheral);

            _gatewayRepo.Gateways.Update(gateway);
        }

    }
}
