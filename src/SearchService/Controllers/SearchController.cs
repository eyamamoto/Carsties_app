using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems(string searchTerm, int pageNumber = 1, int pageSize = 4)
        {
            //busca os itens e ordena pelo campo Make
            var query = DB.PagedSearch<Item>();
            query.Sort(x => x.Ascending(a => a.Make));

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query.Match(Search.Full, searchTerm).SortByTextScore();
            }

            query.PageNumber(pageNumber);
            query.PageSize(pageSize);

            var result = await query.ExecuteAsync();

            return Ok(new
            {
                results = result.Results,
                pageCount = result.PageCount,
                totalCount = result.TotalCount
            });
        }
    }
}
