using CbtBackend.Entities;
using CbtBackend.Migrations;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;

namespace CbtBackend.Services;

public interface IEvaluationService {
    Task<List<MudTest>> GetAllEvaluations();
    Task<MudTest?> GetEvaluation(int id);

    // response handling
    Task<List<MudTestResponse>> GetResponsesByUser(User user);
    Task<MudTestResponse?> GetResponse(int id);
    Task<MudTestResponse> UpdateResponse(int id, EvaluationUpdateRequest request);
    Task<MudTestResponse> CreateResponse(User user, EvaluationCreateRequest request);
    Task<bool> DeleteResponse(int id);
}
