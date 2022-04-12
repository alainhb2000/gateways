using System;
using System.Collections.Generic;
using System.Linq;

namespace Gateways
{
    public class GatewayService
    {
        private readonly GatewaysContext _gatewayRepo;

        public GatewayService(GatewaysContext gatewayRepo)
        {
            _gatewayRepo = gatewayRepo;
        }

        /// <summary>
        /// Get all gateways data
        /// </summary>
        /// <returns>A list of gateways</returns>
        public IEnumerable<Gateway> AllGateways()
        {
            return _gatewayRepo.Gateways.ToList();
        }

        /// <summary>
        /// Add new gateway
        /// </summary>
        /// <param name="gateway">Gateway data</param>
        /// <exception cref="Exception">If serial number already exists</exception>
        /// <exception cref="Exception">If name already exists</exception>
        /// <exception cref="Exception">If ip address already exists</exception>
        public void Add(Gateway gateway)
        {
            if (SeralNumberExists(gateway.SerialNumber))
                throw new Exception($"Serial Number {gateway.SerialNumber} already exists.");

            if (NameExists(gateway.Name))
                throw new Exception($"Name {gateway.Name} already exists.");

            if (IpExists(gateway.IP))
                throw new Exception($"IP address {gateway.IP} already exists.");

            _gatewayRepo.Add(gateway);
            _gatewayRepo.SaveChanges();
        }

        /// <summary>
        /// Get gateway data using serial number
        /// </summary>
        /// <param name="serialNumber">Gateway serial number</param>
        /// <returns>Gateway data</returns>
        public Gateway GetById(string serialNumber)
        {
            return _gatewayRepo.Gateways.FirstOrDefault(g => g.SerialNumber == serialNumber);
        }

        /// <summary>
        /// Checks if serial number already exists
        /// </summary>
        /// <param name="serialNumber">Serial number</param>
        /// <returns>True if exists, otherwise returns false</returns>
        public bool SeralNumberExists(string serialNumber)
        {
            return _gatewayRepo.Gateways.Any(c => c.SerialNumber == serialNumber);
        }

        /// <summary>
        /// Checks if name already exists
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>True if exists, otherwise returns false</returns>
        public bool NameExists(string name)
        {
            return _gatewayRepo.Gateways.Any(c => c.Name == name);
        }

        /// <summary>
        /// Checks if ip address already exists
        /// </summary>
        /// <param name="ip">IP address</param>
        /// <returns>True if exists, otherwise returns false</returns>
        public bool IpExists(string ip)
        {
            return _gatewayRepo.Gateways.Any(c => c.IP == ip);
        }

        /// <summary>
        /// Update gateway data
        /// </summary>
        /// <param name="serialNumber">Serial number</param>
        /// <param name="name">Name</param>
        /// <param name="ip">IP address</param>
        /// <exception cref="Exception">If serial number already exists</exception>
        /// <exception cref="Exception">If name already exists</exception>
        /// <exception cref="Exception">If ip address already exists</exception>
        public void Update(string serialNumber, string name, string ip)
        {

            var gateway = GetById(serialNumber);

            if (gateway == null)
                throw new Exception($"Gateway {serialNumber} not found.");

            if (gateway.Name != name && NameExists(name))
                throw new Exception($"IP address {ip} already exists.");

            if (gateway.IP != ip && IpExists(ip))
            {
                throw new Exception($"IP address {ip} already exists.");
            }

            gateway.Name = name;
            gateway.IP = ip;

            _gatewayRepo.Gateways.Update(gateway);
            _gatewayRepo.SaveChanges();
        }

        /// <summary>
        /// Remove gateway
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <exception cref="Exception">If gateway was not found</exception>
        public void Remove(string serialNumber)
        {
            var gateway = _gatewayRepo.Gateways.FirstOrDefault(g => g.SerialNumber == serialNumber);
            if (gateway == null)
                throw new Exception($"Gateway {serialNumber} not found.");

            _gatewayRepo.Gateways.Remove(gateway);
            _gatewayRepo.SaveChanges();
        }


    }
}
