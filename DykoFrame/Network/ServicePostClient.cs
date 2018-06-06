using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MsgPack;
using MsgPack.Serialization;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Networking;
using UnityEngine;
using System.IO;

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
                webRequest = new UnityWebRequest("https://ssl.dyko.eu/cgi-bin/webglHttpsTunel/BytesPost", UnityWebRequest.kHttpVerbPOST);

                MessagePackSerializer.PrepareType<byte>();
                MessagePackSerializer.PrepareType<byte[]>();
                MessagePackSerializer.PrepareType<RequestPayload>();
                var s = MessagePackSerializer.Get<RequestPayload>();


                byte[] ss = s.PackSingleObject(rq);

                SecurePacket p = new SecurePacket((UInt16)ServicePort,ss);
                MessagePackSerializer.PrepareType<UInt16>();
                MessagePackSerializer.PrepareType<SecurePacket.SecureFrame>();
                var sd = MessagePackSerializer.Get<SecurePacket.SecureFrame>();

                byte[] data = sd.PackSingleObject(p.GetSecuredPacket());
            
                
                UploadHandlerRaw dataHandler = new UploadHandlerRaw(data);
                dataHandler.contentType = "application/octet-stream"; // might work with 'multipart/form-data'
                webRequest.uploadHandler = dataHandler;
                
                DownloadHandlerBuffer downHand = new DownloadHandlerBuffer();
                webRequest.downloadHandler = downHand;

                yield return webRequest.SendWebRequest();

                List<byte> replyData = new List<byte>();

                byte[] unsecuredData = SecurePacket.GetData(sd.UnpackSingleObject(downHand.data));

                MessagePackSerializer.PrepareType<GeneralResponseState>();
                MessagePackSerializer.PrepareType<GeneralRequestResponse>();
                var sr = MessagePackSerializer.Get<GeneralRequestResponse>();

                result  = sr.UnpackSingleObject(unsecuredData);

                webRequest.Dispose();
            }
        }
    }
}
