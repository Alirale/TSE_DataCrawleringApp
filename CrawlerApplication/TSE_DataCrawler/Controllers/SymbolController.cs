using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace TSE_DataCrawler.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("[controller]/v{version:ApiVersion}")]
    public class SymbolController : Controller
    {
        private readonly ISymbolService _symbolService;

        public SymbolController(ISymbolService symbolService)
        {
            _symbolService = symbolService;
        }

        [HttpGet]
        [Route("Get")]
        public List<Symbol> Get()
        {
            return _symbolService.GetSymbols().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}