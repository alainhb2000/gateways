using System;
using System.Collections.Generic;

namespace Gateways
{
    public static class DemoData
    {
        public static void Create(GatewaysContext ctx)
        {
            ctx.Gateways.Add(new Gateway
            {
                SerialNumber = "123456",
                Name = "Company 1",
                IP = "172.16.54.1",
                Peripherals = new List<Peripheral>
                {
                    new Peripheral
                    {
                        Id = 1000,
                        CreationDate  = DateTime.Now.AddDays(-100),
                        Vendor = "Cisco",
                        IsOnline = true
                    }
                }
            });

            ctx.Gateways.Add(new Gateway
            {
                SerialNumber = "234544",
                Name = "Company 2",
                IP = "172.16.54.16",
                Peripherals = new List<Peripheral>
                {
                    new Peripheral
                    {
                        Id = 2000,
                        CreationDate  = DateTime.Now.AddDays(-40),
                        Vendor = "TP-LINK",
                        IsOnline = true
                    },
                    new Peripheral
                    {
                        Id = 2001,
                        CreationDate  = DateTime.Now.AddDays(-45),
                        Vendor = "Huawei",
                        IsOnline = false
                    },
                    new Peripheral
                    {
                        Id = 2003,
                        CreationDate  = DateTime.Now.AddDays(-22),
                        Vendor = "ZyXel",
                        IsOnline = true
                    },
                    new Peripheral
                    {
                        Id = 2004,
                        CreationDate  = DateTime.Now.AddDays(-11),
                        Vendor = "Xiaomi",
                        IsOnline = true
                    },
                    new Peripheral
                    {
                        Id = 2005,
                        CreationDate  = DateTime.Now.AddDays(-15),
                        Vendor = "DLink",
                        IsOnline = false
                    },
                    new Peripheral
                    {
                        Id = 2006,
                        CreationDate  = DateTime.Now.AddDays(-4),
                        Vendor = "KWifi",
                        IsOnline = false
                    },
                    new Peripheral
                    {
                        Id = 2007,
                        CreationDate  = DateTime.Now.AddDays(-12),
                        Vendor = "Nokia",
                        IsOnline = false
                    },
                    new Peripheral
                    {
                        Id = 2008,
                        CreationDate  = DateTime.Now.AddDays(-8),
                        Vendor = "Mikrotik",
                        IsOnline = false
                    },
                    new Peripheral
                    {
                        Id = 2009,
                        CreationDate  = DateTime.Now.AddDays(-38),
                        Vendor = "Ubiquiti",
                        IsOnline = false
                    },
                    new Peripheral
                    {
                        Id = 2010,
                        CreationDate  = DateTime.Now.AddDays(-18),
                        Vendor = "Telindous",
                        IsOnline = false
                    }

                }
            });

            ctx.SaveChanges();
        }
    }
}
