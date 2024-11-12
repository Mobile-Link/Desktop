using MobileLink_Desktop.Enums;

namespace MobileLink_Desktop.Entities;

public class TransferenceChunk
{
    public int IdTransferenceChunk { get; set; }
    public Transference Transference { get; set; }
    public int IdTransference { get; set; }
    public long StartByteIndex { get; set; }
    public EnChunkStatus EnChunkStatus { get; set; }
}