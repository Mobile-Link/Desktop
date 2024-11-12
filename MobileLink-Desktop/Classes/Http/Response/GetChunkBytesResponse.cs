using MobileLink_Desktop.Entities;

namespace MobileLink_Desktop.Classes.Http.Response;

public class GetChunkBytesResponse
{
    public TransferenceChunk Chunk { get; set; }
    public byte[] Bytes { get; set; }
}