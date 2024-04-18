namespace LendLoopAPI.ModelDto
{
    public class LoanDto
    {
        public DateOnly StartDate { get; set; }
        public int Duration { get; set; }
        public int ItemId { get; set; }
        public int LenderId { get; set; }
        public int BorrowerId { get; set; }
        public string LoanStatus { get; set; } = "asking"; 
    }
}
