using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LendLoopAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using LendLoopAPI.ModelDto;

namespace LendLoopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LendLoopContext _context;

        public LoansController(LendLoopContext context)
        {
            _context = context;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            return await _context.Loans.ToListAsync();
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            return loan;
        }

        // PUT: api/Loans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(int id, Loan loan)
        {
            if (id != loan.LoanId)
            {
                return BadRequest();
            }

            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Loans
        [HttpPost]
        public async Task<ActionResult<Loan>> PostLoan(Loan loan)
        {
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLoan", new { id = loan.LoanId }, loan);
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("loanRequest")]
        public async Task<ActionResult> LoanRequest(LoanDto loan)
        {
            var itemAvailability = _context.Loans.Any(x=>x.ItemId == loan.ItemId && x.LoanStatus == "rented");
            if(itemAvailability)
            {
                throw new ArgumentException($"Item {loan.ItemId} already rented.");
            }
            var newLoan = new Loan()
            {
                StartDate= loan.StartDate, 
                Duration = loan.Duration,
                ItemId = loan.ItemId, 
                LenderId = loan.LenderId, 
                BorrowerId = loan.BorrowerId, 
                LoanStatus = loan.LoanStatus
            }; 
            _context.Loans.Add(newLoan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("LoanRequest", new {id= newLoan.LoanId}, newLoan); 
        }
        [HttpPut("{id}/accept")]
        public async Task<ActionResult> AcceptLoan(int id)
        {
            var loan = _context.Loans.FirstOrDefault(x=>x.LoanId== id);
            if(loan == null)
            {
                return BadRequest(); 
            }
            loan.LoanStatus = "rented"; 
            _context.Entry(loan).Property(x=>x.LoanStatus).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); 
        }
        [HttpPut("{id}/loanFinished")]
        public async Task<ActionResult> LoanFinished(int id)
        {
            var loan = _context.Loans.FirstOrDefault(x=> x.LoanId == id);
            if(loan  == null) 
            {
                return BadRequest();
            }

            if(loan.LoanStatus != "rented")
            {
                throw new Exception("Item is not rented"); 
            }

            loan.LoanStatus = "over"; 
            _context.Entry(loan).Property(x=>x.LoanStatus).IsModified=true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return NoContent() ;
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.LoanId == id);
        }
        private string LoanStatus(int id)
        {
            var loan = _context.Loans.FirstOrDefault(x => x.LoanId == id); 
            if(loan == null)
            {
                throw new Exception("Loan not exist"); 
            }
            return loan.LoanStatus;  
        }
    }
}
