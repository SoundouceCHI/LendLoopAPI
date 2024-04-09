using System.Reflection.Metadata;

namespace LendLoopAPI.Models
{
    public class QRCodeImage
    {
        public int QRCodeImageId { get; set; }
        public byte[] Image { get; set; }
        public int ItemId { get; set; }
    }
}
