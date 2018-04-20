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
        public class HighScoreClient
        {
            public enum Requests : byte
            {
                GetTop = 0x10,
                AddScore = 0x11
            }

            Dictionary<string, int> highScoreTable;
            ServiceClient client;

            public HighScoreClient() : this(0)
            {

            }

            public HighScoreClient(int index)
            {
                client = new ServiceClient((GameServicePort)((int)GameServicePort.HighScoreBase + index));
                highScoreTable = new Dictionary<string, int>();
            }

            public void AddScoreEntry(string name, int value, Action<bool> callback)
            {
                AddScoreEntry en;
                en.name = name;
                en.value = value;

                RequestPayload rq;
                rq.rq = (byte)Requests.AddScore;
                rq.data = MessagePackSerializer.Serialize(en);

                Task.Run(async () =>
                {
                    GeneralRequestResponse res = await client.HandleRq(rq);
                    callback(res.state == GeneralResponseState.GeneralOk);
                });
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
                Task.Run(async () =>
                {
                    GeneralRequestResponse res = await client.HandleRq(rq);
                    callback(MessagePackSerializer.Deserialize<string>(res.data));
                });
            }

        }
    }
}
