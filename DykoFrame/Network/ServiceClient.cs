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
        class ServiceClient
        {

            private TcpClient tcpClient;
            public GameServicePort ServicePort { get; protected set; }


            public ServiceClient(GameServicePort id)
            {
                ServicePort = id;
            }

            public Task<GeneralRequestResponse> HandleRq(RequestPayload rq)
            {
                return Task.Run(() =>
                {
                    tcpClient = new TcpClient();
                    SecurePacket p = new SecurePacket((UInt16)ServicePort, MessagePackSerializer.Serialize(rq));
                    byte[] data = MessagePackSerializer.Serialize(p.GetSecuredPacket());
                    tcpClient.Connect("80.211.223.84", 1287);
                    NetworkStream s = tcpClient.GetStream();
                    s.Write(data, 0, data.Length);

                    while (!s.DataAvailable) ;
                    List<byte> replyData = new List<byte>();
                    while (s.DataAvailable)
                    {
                        byte[] buff = new byte[512];
                        s.Read(buff, 0, (int)buff.Length);
                        replyData.AddRange(buff);
                    }

                    byte[] unsecuredData = SecurePacket.GetData(MessagePackSerializer.Deserialize<SecurePacket.SecureFrame>(replyData.ToArray()));

                    GeneralRequestResponse result = MessagePackSerializer.Deserialize<GeneralRequestResponse>(unsecuredData);
                    s.Close();
                    tcpClient.Dispose();
                    return result;
                });

            }
        }
    }
}
