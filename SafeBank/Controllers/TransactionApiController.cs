using Microsoft.AspNetCore.Mvc;
using SafeBank.Data;
using SafeBank.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SafeBank.Controllers
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionApiController : ControllerBase
    {
        // GET: api/<TransactionApiController>
        private readonly TransactionRepository transactionRepository;
        private readonly ApplicationDbContext appDbContext;

        public TransactionApiController(TransactionRepository transactionRepository, ApplicationDbContext appDbContext)
        {
            this.transactionRepository = transactionRepository;
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var result = appDbContext.Transactions.ToList();
            return Ok(result);
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int tranId)
        {
            if (tranId.GetType() != typeof(int))
            {
                return BadRequest();
            }
            if (!appDbContext.Transactions.Any(tran => tran.Id == tranId))
            {
                return NotFound();
            }
            return Ok(appDbContext.Transactions.FirstOrDefault(i => i.Id == tranId));
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult Delete(int tranId)
        {
            if (tranId.GetType() != typeof(int))
            {
                return BadRequest();
            }
            if (!appDbContext.Transactions.Any(tran => tran.Id == tranId))
            {
                return NotFound();
            }
            var tran = transactionRepository.GetTransactionsById(tranId);
            return Ok();
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] Transaction transaction)
        {
            transaction.Id = 0;
            if (!appDbContext.Transactions.Any(tran => tran.Id == transaction.Id))
            {
                return BadRequest();
            }
            transactionRepository.RegisterTransaction(transaction);
            return Ok(transaction);
        }

        [HttpPut]
        [Route("Put")]
        public IActionResult Put([FromBody] Transaction transaction)
        {
            if (transaction.Id.GetType() != typeof(int))
            {
                return BadRequest();
            }
            if (!appDbContext.Transactions.Any(tran => tran.Id == transaction.Id))
            {
                return BadRequest();
            }
            if (!appDbContext.Transactions.Any(tran => tran.Id == transaction.Id))
            {
                return NotFound();
            }
            transactionRepository.RegisterTransaction(transaction);
            return Ok(transaction);
        }
    }
}
