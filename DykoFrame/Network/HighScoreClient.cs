using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using MessagePack;

namespace DykoFrame
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

       public HighScoreClient()
        {
            client = new ServiceClient(GameServicePort.HighScore);
            highScoreTable = new Dictionary<string, int>();
        }

        public async Task<bool> AddScoreEntry(string name, int value)
        {
            AddScoreEntry en;
            en.name = name;
            en.value = value;

            RequestPayload rq;
            rq.rq = (byte)Requests.AddScore;
            rq.data = MessagePackSerializer.Serialize(en);
            GeneralRequestResponse res = await client.HandleRq(rq);

            return res.state == GeneralResponseState.GeneralOk;
        }

    }
}
