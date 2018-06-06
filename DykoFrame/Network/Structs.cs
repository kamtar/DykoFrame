using System;
using System.Collections.Generic;
using System.Text;
using MsgPack;

namespace DykoFrame
{
    namespace Network
    {

        public enum GameServicePort : UInt16
        {
            Info = 0,
            AnalyticsBase = 0x10,
            HighScoreBase = 0x20
        }

        public enum GeneralResponseState : byte
        {
            GeneralFail = 0x00,
            GeneralOk = 0xFF
        }
        
        
        public struct GeneralRequestResponse
        {
    
            public GeneralResponseState state;
        
            public byte[] data;
        }

        public struct RequestPayload
        {
       
            public byte rq;
      
            public byte[] data;
        }

      
        public struct RequestTopTable
        {
        
            public byte num;
        }

  
        public struct AddScoreEntry
        {
    
            public string name;
  
            public int value;
        }

        public struct TimeSpent
        {
      
            public UInt64 data;
        }
    }
}
