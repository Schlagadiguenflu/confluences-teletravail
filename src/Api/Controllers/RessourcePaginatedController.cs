using Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using static Api.Utility.Pagination.DynamicPagination;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RessourcePaginatedController : ControllerBase
    {
        private readonly ConfluencesContext _context;

        public RessourcePaginatedController(ConfluencesContext context)
        {
            _context = context;
        }
        // POST: api/RessourcePaginated
        [HttpPost]
        public object GetHomeworkV2s([FromForm] DataTableAjaxPostModel model)
       {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;

            var res = DSearchFunc(_context, typeof(Ressource), model, out filteredResultsCount, out totalResultsCount);

            return new
            {
                // this is what datatables wants sending back
                draw = model.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            };
        }
    }
}
