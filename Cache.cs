using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USART_Monitor
{
    public class Cache
    {
        public const int maxSuportPortsCount = 2;
        public List<String> availableSerialPortNames;
        public List<String> selectedPortNames;
        public bool bConnected;

        // settings
        public String dateTimeFormat;
        public String logFileName;
        public bool bShowDateTime;
        public bool bShowRXTX;
        public Cache()
        {
            this.bConnected = false;
            this.selectedPortNames = new List<string>(maxSuportPortsCount);
            this.availableSerialPortNames = new List<string>();

            dateTimeFormat = "MM/dd/yyyy hh:mm:ss.fff tt";
            logFileName = "D:/USART-log.txt";

            bShowDateTime = true;
        }
    }
}
