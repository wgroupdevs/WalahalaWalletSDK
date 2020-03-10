using System.IO;

namespace WalaSDK.Cryptography
{
    public interface ISerializable
    {
        int Size { get; }

        void Serialize(BinaryWriter writer);

        void Deserialize(BinaryReader reader);
    }
}
