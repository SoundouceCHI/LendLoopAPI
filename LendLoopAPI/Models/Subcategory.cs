namespace LendLoopAPI.Models
{
    public class Subcategory
    {
        public int SubcategoryId { get; set; }
        public string SubcategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
