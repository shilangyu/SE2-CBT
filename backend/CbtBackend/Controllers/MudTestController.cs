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
public class EvaluationController : UserAwareController {
    private readonly IEvaluationService evaluationService;
    private readonly ILogger<EvaluationController> logger;

    public EvaluationController(IEvaluationService evaluationService, ILogger<EvaluationController> logger, UserManager<User> userManager) : base(userManager) {
        this.evaluationService = evaluationService;
        this.logger = logger;
    }

    [Authorize(Roles = UserRoles.EvaluationRead)]
    [HttpGet(ApiRoutes.Evaluation.GetEvaluations)]
    public async Task<IActionResult> GetEvaluations() {
        var evaluations = await evaluationService.GetAllEvaluations();
        return Ok(evaluations);
    }

    [Authorize(Roles = UserRoles.EvaluationRead)]
    [HttpGet(ApiRoutes.Evaluation.GetEvaluation)]
    public async Task<IActionResult> GetEvaluation([FromRoute] int id) {
        var evaluation = await evaluationService.GetEvaluation(id);

        if (evaluation == null) {
            return NotFound();
        }

        return Ok(evaluation);
    }

    [Authorize(Roles = UserRoles.EvaluationRead)]
    [HttpGet(ApiRoutes.Evaluation.GetEvaluationResponse)]
    public async Task<IActionResult> GetEvaluationResponse([FromRoute] int id) {
        var response = await evaluationService.GetResponse(id);
        if (response == null) {
            return NotFound();
        }

        var contextUser = await this.ContextUser();
        if (contextUser == null || !contextUser.IsAdmin && contextUser.Id != response.UserId) {
            return Forbid();
        }

        return Ok(response);
    }

    [Authorize(Roles = UserRoles.EvaluationRead + "," + UserRoles.EvaluationWrite)]
    [HttpPost(ApiRoutes.Evaluation.PostEvaluationResponse)]
    public async Task<IActionResult> PostEvaluationResponse([FromBody] EvaluationCreateRequest request) {
        try {
            var author = await this.ContextUser();

            if (author == null) {
                return Unauthorized("the context user was null, please try again later");
            }

            var response = await evaluationService.CreateResponse(author, request);
            return Ok(response);
        } catch (EvaluationNotFoundException) {
            return BadRequest(string.Format("evaluation with id {0} does not exist", request.TestId));
        }
    }

    [Authorize(Roles = UserRoles.EvaluationRead + "," + UserRoles.EvaluationWrite)]
    [HttpPut(ApiRoutes.Evaluation.PutEvaluationResponse)]
    public async Task<IActionResult> PutEvaluationResponse([FromRoute] int id, [FromBody] EvaluationUpdateRequest request) {
        try {
            var response = await evaluationService.UpdateResponse(id, request);
            return Ok(response);
        } catch (ResponseNotFoundException) {
            return NotFound();
        }
    }

    [Authorize(Roles = UserRoles.EvaluationRead + "," + UserRoles.EvaluationWrite)]
    [HttpDelete(ApiRoutes.Evaluation.DeleteEvaluationResponse)]
    public async Task<IActionResult> DeleteEvaluationResponse([FromRoute] int id) {
        try {
            await evaluationService.DeleteResponse(id);
            return Ok();
        } catch (ResponseNotFoundException) {
            return NotFound();
        }
    }

    [Authorize(Roles = UserRoles.EvaluationRead)]
    [HttpGet(ApiRoutes.Evaluation.GetEvaluationResponseById)]
    public async Task<IActionResult> GetResponsesByUserId([FromQuery(Name = "userId")] int userId) {
        var contextUser = await this.ContextUser();
        if (contextUser == null || !contextUser.IsAdmin && contextUser.Id != userId) {
            return Forbid();
        }

        var queryUser = await UserManager.FindByIdAsync(userId);

        if (queryUser == null) {
            return NotFound();
        }

        var responseList = await evaluationService.GetResponsesByUser(queryUser);
        return Ok(responseList);
    }
}
