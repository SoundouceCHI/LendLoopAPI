using System.Reflection.Metadata;

namespace LendLoopAPI.Models
{
    public class QRCodeImage
    {
        public int QRCodeId { get; set; }
        public Blob Image { get; set; }
        public int ItemId { get; set; }
    }
}
