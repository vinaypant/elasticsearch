using AutoMapper;
using ElasticSearchDemo.Models;
using ElasticSearchDemo.Models.Elastic;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
using Nest;
using Serilog;
using System;
using System.Threading.Tasks;

namespace ElasticSearchDemo.Controllers
{
    //[ControllerName]
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ElasticController(ILogger logger, IElasticClient elasticClient, IMapper mapper)
        {
            this._elasticClient = elasticClient;
            this._logger = logger;
            _mapper = mapper;
        }

        [HttpGet("getproducts/{keyword}")]
        public async Task<IActionResult> GetProducts(string keyword)
        {
            try
            {
                throw new Exception(message: "Oops what happened");
                //var results = await _elasticClient.SearchAsync<Product>(q => q.Query(
                //      q => q.QueryString(q => q.Query('*' + keyword + '*'))));
                // return Ok(results.Documents.ToList());
                return Ok();

            }
            catch (Exception ex)
            {
                _logger.Information(ex, messageTemplate: "Something bad happened");
                return new StatusCodeResult(500);
            }


        }

        private bool CheckIndex()
        {
            return _elasticClient.Indices.Exists("employee").Exists;
        }


        //if same id is given then then it behaves as httpput (i.e update data)
        [HttpPost("addemployee")]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            if (!CheckIndex())
                return NotFound("Index does not exist");
            //var response = await _elasticClient.IndexDocumentAsync(employee);   //make use of default index

            var elasticEmployee = _mapper.Map<ElasticEmployee>(employee);

            var response = await _elasticClient.IndexAsync<ElasticEmployee>(elasticEmployee, q => q.Index("employee")
            .Refresh(Elasticsearch.Net.Refresh.True));


            if (response.IsValid)
            {
                return Ok();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut()]
        public async Task<IActionResult> UpdateEmployee(Employee employee)
        {
            if (!CheckIndex())
                return NotFound("Index does not exist");

            var elasticEmployee = _mapper.Map<ElasticEmployee>(employee);

            var response = await _elasticClient.UpdateAsync<ElasticEmployee>(employee.Id, q => q.Index("employee").Doc(elasticEmployee));

            if (response.IsValid)
            {
                return Ok();
            }
            else
            {
                return Ok(response);
            }
        }
        [HttpDelete("{empId}")]

        public async Task<IActionResult> DeleteEmployee(Guid empId)
        {

            if (!CheckIndex())
                return NotFound("Index does not exist");

            var response = await _elasticClient.DeleteAsync<ElasticEmployee>(empId, q => q.Index("employee"));

            if (response.IsValid)
            {
                return Ok();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search()
        {

            var response = await _elasticClient.SearchAsync<ElasticEmployee>(q =>
            q.From(0)
            .Size(10)
            .Index("employee")
            .Query(q =>
               q.Bool(
                   q =>
                   q.Must(

                         q => q.Bool(q => q.Should(q => q.Term(q => q.Age, 25), q => q.Term(q => q.Age, 33))),
                         q => q.Bool(q => q.Filter(q => q.Range(q => q.Field(q => q.Salary).GreaterThanOrEquals(5000))))
                       )

               ))
            //.Sort(q => q.Field(q => q.Salary, SortOrder.Ascending)));
            .Sort(q => q.Ascending(q => q.Name.Suffix("keyword"))));

            return Ok(response.Documents);

        }


    }
}
