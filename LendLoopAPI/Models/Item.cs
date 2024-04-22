namespace LendLoopAPI.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        
        public string Description { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public byte[] ItemImage { get; set; }
        public int UserId { get; set; }
        public int SubcategoryId { get; set; }
        public int StatusId { get; set; }
    }
}
