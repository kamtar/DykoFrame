using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization;


namespace DykoFrame
{
    namespace Network
    {
        public class SecurePacket
        {
            const byte PacketVer = 0x01;

            static byte[] SystemSeed = { 0x00, 0x00, 0x00, 0x00 };
            
            public static void SetSeed (byte[] key)
            {
                SystemSeed = key;
            }

            public struct SecureFrame
            {
                public byte ver;
                public UInt16 id;
                public byte[] data;
                public byte[] sha;

            }

            private byte[] originalData;
            private UInt16 mID;

            public SecurePacket(UInt16 id, byte[] data)
            {
                mID = id;
                originalData = data;
            }

            public SecurePacket(UInt16 id, byte[] data, byte[] key)
            {
                mID = id;
                originalData = data;              
            }
        
            public SecureFrame GetSecuredPacket()
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                SecureFrame o = new SecureFrame();

                o.ver = PacketVer;
                o.id = mID;
                var shaData = new byte[originalData.Length + SystemSeed.Length];

                originalData.CopyTo(shaData, 0);
                SystemSeed.CopyTo(shaData, originalData.Length);

                o.sha = sha.ComputeHash(shaData);
                o.data = originalData;

                return o;
            }

            public static byte[] GetData(SecureFrame p)
            {
                SHA1 sha = new SHA1CryptoServiceProvider();

                var shaData = new byte[p.data.Length + SystemSeed.Length];

                p.data.CopyTo(shaData, 0);
                SystemSeed.CopyTo(shaData, p.data.Length);

                var tSha = sha.ComputeHash(shaData);

                for (int i = 0; i < tSha.Length; i++)
                {
                    if (tSha[i] != p.sha[i])
                        return null;
                }

                return p.data;

            }
        }
    }
}
