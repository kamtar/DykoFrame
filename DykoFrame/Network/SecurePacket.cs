using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using MessagePack;

namespace DykoFrame
{
    namespace Network
    {
        public class SecurePacket
        {
            const byte PacketVer = 0x01;
            static readonly byte[] SystemSeed = { 0x0D, 0x12, 0x4E, 0x08 };


            [MessagePackObject]
            public struct SecureFrame
            {
                [Key(0)]
                public byte ver;
                [Key(1)]
                public UInt16 id;
                [Key(3)]
                public byte[] data;
                [Key(4)]
                public byte[] sha;
            }

            private byte[] originalData;
            private UInt16 mID;

            public SecurePacket(UInt16 id, byte[] data)
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
