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
        public String[] availableSerialPortNames;
        public String[] selectedPortNames;
        public Cache()
        {
            this.selectedPortNames = new String[maxSuportPortsCount];
            this.availableSerialPortNames = new String[0];
        }

    }
}
