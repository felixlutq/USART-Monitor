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
        public Cache()
        {
            this.bConnected = false;
            this.selectedPortNames = new List<string>(maxSuportPortsCount);
            this.availableSerialPortNames = new List<string>();
        }
    }
}
