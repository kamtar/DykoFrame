using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MsgPack;
using MsgPack.Serialization;
using System;
using System.IO;
namespace DykoFrame
{
    namespace Network
    {
        public class HighScoreClient : ServicePostClient
        {
            public enum Requests : byte
            {
                GetTop = 0x10,
                AddScore = 0x11
            }

            ServicePostClient client;

            Action<string> clTable = null;
            Action<bool> clEntry = null;

            public HighScoreClient() : this(0)
            {

            }

            public HighScoreClient(int index) : base ((GameServicePort)((int)GameServicePort.HighScoreBase + index))
            {

            }

            public void AddScoreEntry(string name, int value, Action<bool> callback)
            {
                MessagePackSerializer.PrepareType<AddScoreEntry>();
                AddScoreEntry en;
                en.name = name;
                en.value = value;
                var s = MessagePackSerializer.Get<AddScoreEntry>();
                RequestPayload rq;
                rq.rq = (byte)Requests.AddScore;
                rq.data = s.PackSingleObject(en);
                base.HandleRq(rq);
                clEntry = callback;
            }

            public void GetHighScore(int num, Action<string> callback)
            {
                if (num > byte.MaxValue)
                    throw new BadUsageException();

                MessagePackSerializer.PrepareType<byte>();
                MessagePackSerializer.PrepareType<RequestTopTable>();
                RequestTopTable tt;
                tt.num = (byte)num;
                var s = MessagePackSerializer.Get<RequestTopTable>();
                RequestPayload rq;
                rq.rq = (byte)Requests.GetTop;
                rq.data = s.PackSingleObject(tt);
                base.HandleRq(rq);
               clTable = callback;
  
            }

            

            void Update()
            {
                if (base.result.HasValue)
                {
                    if(clTable != null)
                    {
                        MessagePackSerializer.PrepareType<string>();
                        var s = MessagePackSerializer.Get<string>();
                        clTable(s.UnpackSingleObject(result.Value.data));
                        clTable = null;
                    }

                    else if (clEntry != null)
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
