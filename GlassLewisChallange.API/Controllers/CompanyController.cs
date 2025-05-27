using GlassLewisChallange.Application.Companies.Create;
using GlassLewisChallange.Application.Companies.GetAll;
using GlassLewisChallange.Application.Companies.GetById;
using GlassLewisChallange.Application.Companies.GetCompanyByIsin;
using GlassLewisChallange.Application.Companies.Update;
using GlassLewisChallange.Infrastructure.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GlassLewisChallange.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CreateCompanyDto>>> Create([FromBody] CreateCompanyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<CreateCompanyDto>.SuccessResponse(result, "Company created successfully."));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetCompanyByIdDto>>> GetById(string id)
        {
            var result = await _mediator.Send(new GetCompanyByIdQuery { Id = id });
            return Ok(ApiResponse<GetCompanyByIdDto>.SuccessResponse(result));
        }

        [HttpGet("isin/{isin}")]
        public async Task<ActionResult<ApiResponse<GetCompanyByIdQuery>>> GetByIsin(string isin)
        {
            var result = await _mediator.Send(new GetCompanyByIsinQuery { Isin = isin });
            return Ok(ApiResponse<GetCompanyByIsinDto>.SuccessResponse(result));
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<GetAllCompaniesDto>>>> GetAll()
        {
            var result = await _mediator.Send(new GetAllCompaniesQuery());
            return Ok(ApiResponse<List<GetAllCompaniesDto>>.SuccessResponse(result));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<string>>> Update([FromBody] UpdateCompanyCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<string>.SuccessResponse(null, "Company updated successfully."));
        }
    }
}
