using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using MessagePack;
using System;

namespace DykoFrame
{
    namespace Network
    {
        public class AnalyticsClient
        {
            public enum Requests : byte
            {
                AddTimeEntry = 0x10
            }

            public enum EventType : byte
            {
                TimePlaying = 0x01
            }

            ServiceClient client;

            public AnalyticsClient() : this(0)
            {

            }

            public AnalyticsClient(int index)
            {
                client = new ServiceClient((GameServicePort)((int)GameServicePort.AnalyticsBase + index));
            }

            public void AddTimeEntry(byte time, Action<bool> callback)
            {
                TimeSpent en;
                en.data = time;

                RequestPayload rq;
                rq.rq = (byte)Requests.AddTimeEntry;
                rq.data = MessagePackSerializer.Serialize(en);

                Task.Run(async () =>
                {
                    GeneralRequestResponse res = await client.HandleRq(rq);
                    callback(res.state == GeneralResponseState.GeneralOk);
                });
            }

        }
    }
}
