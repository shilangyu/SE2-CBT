using CbtBackend.Attributes;
using CbtBackend.Contracts;
using CbtBackend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Services;

namespace CbtBackend.Controllers;

[ApiController]
[Produces("application/json")]
public class EvaluationController : ControllerBase {
    private readonly IEvaluationService evaluationService;
    private readonly ILogger<EvaluationController> logger;

    public EvaluationController(IEvaluationService evaluationService, ILogger<EvaluationController> logger) {
        this.evaluationService = evaluationService;
        this.logger = logger;
    }

    [HttpGet(ApiRoutes.Evaluation.GetEvaluations)]
    public async Task<IActionResult> GetEvaluations() {
        var evaluations = await evaluationService.GetAllEvaluations();
        return Ok(evaluations);
    }

    [HttpGet(ApiRoutes.Evaluation.GetEvaluation)]
    public async Task<IActionResult> GetEvaluation([FromRoute] int id) {
        var evaluation = await evaluationService.GetEvaluation(id);

        if (evaluation == null) {
            return NotFound();
        }

        return Ok(evaluation);
    }

    [HttpGet(ApiRoutes.Evaluation.GetEvaluationResponse)]
    public async Task<IActionResult> GetEvaluationResponse([FromRoute] int id) {
        return BadRequest();
    }

    [HttpPost(ApiRoutes.Evaluation.PostEvaluationResponse)]
    public async Task<IActionResult> PostEvaluationResponse([FromBody] EvaluationCreateRequest request) {
        return BadRequest();
    }

    [HttpPut(ApiRoutes.Evaluation.PutEvaluationResponse)]
    public async Task<IActionResult> PutEvaluationResponse([FromRoute] int id, [FromBody] EvaluationUpdateRequest request) {
        return BadRequest();
    }

    [HttpDelete(ApiRoutes.Evaluation.DeleteEvaluationResponse)]
    public async Task<IActionResult> DeleteEvaluationResponse([FromRoute] int id) {
        return BadRequest();
    }
}
