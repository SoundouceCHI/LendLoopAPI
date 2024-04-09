
namespace LendLoopAPI.Models
{
    public class Loan
    {
        public int LoanId { get; set; }
        public DateOnly StartDate { get; set; }
        public int Duration { get; set; }
        public int ItemId { get; set; }
        public int LenderId { get; set; }
        public int BorrowerId { get; set; }
        public int StatusId { get; set; }

    }
}
