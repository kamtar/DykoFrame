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
                AddScoreEntry en;
                en.name = name;
                en.value = value;

                RequestPayload rq;
                rq.rq = (byte)Requests.AddScore;
                rq.data = MessagePackSerializer.Serialize(en);


                base.HandleRq(rq);
                clEntry = callback;
            }

            public void GetHighScore(int num, Action<string> callback)
            {
                if (num > byte.MaxValue)
                    throw new BadUsageException();

                RequestTopTable tt;
                tt.num = (byte)num;

                RequestPayload rq;
                rq.rq = (byte)Requests.GetTop;
                rq.data = MessagePackSerializer.Serialize(tt);
                base.HandleRq(rq);
                clTable = callback;
  
            }

            void Update()
            {
                if (base.result.HasValue)
                {
                    if(clTable != null)
                    {
                        clTable(MessagePackSerializer.Deserialize<string>(base.result.Value.data));
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
