﻿using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace LabelCreator.Helpers
{
    public class PrinterHandler
    {
        public static PrinterSettings GetPrinterSettings(string printerName)
        {
            var settings = new PrinterSettings
            {
                PrinterName = printerName
            };

            return settings;
        }


        public static void GetPrinterSettings2(string printerName)
        {
            string query = string.Format("SELECT * from Win32_Printer WHERE Name LIKE '%{0}'", printerName);

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            using (ManagementObjectCollection coll = searcher.Get())
            {
                try
                {
                    foreach (ManagementObject printer in coll)
                    {
                        foreach (PropertyData property in printer.Properties)
                        {
                            Console.WriteLine(string.Format("{0}: {1}", property.Name, property.Value));
                        }
                    }
                }
                catch (ManagementException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}