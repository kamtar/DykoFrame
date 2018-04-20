using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using System.Net;
using System.Net.Sockets;

namespace DykoFrame
{
    namespace Network
    {
        public interface IServiceClient
        {
             GameServicePort GetServicePort();
             Task<GeneralRequestResponse> HandleRq(RequestPayload rq);
        }
    }
}
