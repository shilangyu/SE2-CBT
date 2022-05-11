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
    private readonly IUserService userService;
    private readonly ILogger<EvaluationController> logger;
    private readonly UserManager<User> userManager;

    public EvaluationController(IEvaluationService evaluationService, ILogger<EvaluationController> logger, UserManager<User> userManager, IUserService userService) {
        this.evaluationService = evaluationService;
        this.logger = logger;
        this.userManager = userManager;
        this.userService = userService;
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
        var response = await evaluationService.GetResponse(id);

        if (response == null) {
            return NotFound();
        }

        return Ok(response);
    }

    [HttpPost(ApiRoutes.Evaluation.PostEvaluationResponse)]
    public async Task<IActionResult> PostEvaluationResponse([FromBody] EvaluationCreateRequest request) {
        try {
            var contextUser = HttpContext.User;
            var author = await userManager.GetUserAsync(contextUser);

            if (author == null) {
                return Unauthorized();
            }

            await evaluationService.CreateResponse(author, request);
            return Ok();
        } catch (Exception) {
            throw;
        }
    }

    [HttpPut(ApiRoutes.Evaluation.PutEvaluationResponse)]
    public async Task<IActionResult> PutEvaluationResponse([FromRoute] int id, [FromBody] EvaluationUpdateRequest request) {
        try {
            var response = await evaluationService.UpdateResponse(id, request);
            return Ok(response);
        } catch (ResponseNotFoundException) {
            return NotFound();
        }
    }

    [HttpDelete(ApiRoutes.Evaluation.DeleteEvaluationResponse)]
    public async Task<IActionResult> DeleteEvaluationResponse([FromRoute] int id) {
        return BadRequest();
    }
}
