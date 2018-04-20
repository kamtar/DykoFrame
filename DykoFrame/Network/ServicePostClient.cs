using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Networking;
using UnityEngine;

namespace DykoFrame
{
    namespace Network
    {
        public class ServicePostClient : MonoBehaviour
        {

            UnityWebRequest webRequest;
            public GameServicePort ServicePort { get; protected set; }
            public GeneralRequestResponse? result;

            public GameServicePort GetServicePort()
            {
                return ServicePort;
            }

            public ServicePostClient(GameServicePort id)
            {
                ServicePort = id;
               
            }

            public void HandleRq(RequestPayload rq)
            {
                result = null;
                StartCoroutine(SendRq(rq));
            }

            IEnumerator<UnityWebRequestAsyncOperation> SendRq(RequestPayload rq)
            {
                webRequest = new UnityWebRequest("http://80.211.223.84:90/httpTunnel/webglUnity/BytesPost", UnityWebRequest.kHttpVerbPOST);
                SecurePacket p = new SecurePacket((UInt16)ServicePort, MessagePackSerializer.Serialize(rq));
                byte[] data = MessagePackSerializer.Serialize(p.GetSecuredPacket());

                UploadHandlerRaw dataHandler = new UploadHandlerRaw(data);
                dataHandler.contentType = "application/x-www-form-urlencoded"; // might work with 'multipart/form-data'
                webRequest.uploadHandler = dataHandler;

                DownloadHandlerBuffer downHand = new DownloadHandlerBuffer();
                webRequest.downloadHandler = downHand;

                yield return webRequest.SendWebRequest();

                List<byte> replyData = new List<byte>();

                byte[] unsecuredData = SecurePacket.GetData(MessagePackSerializer.Deserialize<SecurePacket.SecureFrame>(downHand.data));

                result = MessagePackSerializer.Deserialize<GeneralRequestResponse>(unsecuredData);
                webRequest.Dispose();
            }
        }
    }
}
