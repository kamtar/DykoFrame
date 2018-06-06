using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MsgPack;
using System.Net;
using System.Net.Sockets;

namespace DykoFrame
{
    namespace Network
    {
        public interface IServiceClient
        {
             GameServicePort GetServicePort();
           
        }
    }
}
