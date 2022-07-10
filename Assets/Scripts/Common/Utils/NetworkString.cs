using Unity.Collections;
using Unity.Netcode;

public struct NetworkString : INetworkSerializable
{
    private FixedString32Bytes info;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref info);
    }

    public override string ToString()
    {
        return info.ToString();
    }

    // override the cast operator
    public static implicit operator string(NetworkString s) => s.ToString();
    // override the default constructor
    public static implicit operator NetworkString(string s) =>
        new NetworkString() { info = new FixedString32Bytes(s) };
}
