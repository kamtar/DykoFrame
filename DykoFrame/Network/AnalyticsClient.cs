using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MsgPack;
using MsgPack.Serialization;
using System;

namespace DykoFrame
{
    namespace Network
    {
        public class AnalyticsClient : ServicePostClient
        {
            public enum Requests : byte
            {
                AddTimeEntry = 0x10
            }

            public enum EventType : byte
            {
                TimePlaying = 0x01
            }

            //ServiceTcpClient client;
            Action<bool> clEntry;
            public AnalyticsClient() : this(0)
            {

            }

            public AnalyticsClient(int index) : base((GameServicePort)((int)GameServicePort.AnalyticsBase + index))
            {
                //client = new ServiceTcpClient((GameServicePort)((int)GameServicePort.AnalyticsBase + index));
            }

            public void AddTimeEntry(ulong time, Action<bool> callback)
            {
                TimeSpent en;
                en.data = time;

                var s = MessagePackSerializer.Get<TimeSpent>();

                RequestPayload rq;
                rq.rq = (byte)Requests.AddTimeEntry;
                rq.data = s.PackSingleObjectAsBytes(en).Array;

                //Task.Run(async () =>
                //{
                    /*GeneralRequestResponse res = await*/ base.HandleRq(rq);
                    clEntry = callback;
                //});
            }

            void Update()
            {
                if (base.result.HasValue)
                {

                    if (clEntry != null)
                    {
                        clEntry((base.result.Value.state == GeneralResponseState.GeneralOk));
                        clEntry = null;
                    }

                    base.result = null;
                }
            }

        }
    }
}
