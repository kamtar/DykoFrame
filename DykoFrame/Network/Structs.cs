using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace DykoFrame
{
    public enum GameServicePort : UInt16
    {
        Info = 0,
        HighScore = 0x24
    }

    public enum GeneralResponseState : byte
    {
        GeneralFail = 0x00,
        GeneralOk = 0xFF
    }

    [MessagePackObject]
    public struct GeneralRequestResponse
    {
        [Key(0)]
        public GeneralResponseState state;
        [Key(1)]
        public byte[] data;
    }

    [MessagePackObject]
    public struct RequestPayload
    {
        [Key(0)]
        public byte rq;
        [Key(1)]
        public byte[] data;
    }

    [MessagePackObject]
    public struct RequestTopTable
    {
        [Key(0)]
        public byte num;
    }

    [MessagePackObject]
    public struct AddScoreEntry
    {
        [Key(0)]
        public string name;
        [Key(1)]
        public int value;
    }
}
