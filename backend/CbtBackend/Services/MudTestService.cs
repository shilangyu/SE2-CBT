using CbtBackend.Entities;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CbtBackend.Services;

public class EvaluationService : IEvaluationService {
    private readonly IConfiguration configuration;
    private readonly CbtDbContext dbContext;

    public EvaluationService(IConfiguration configuration, CbtDbContext dbContext) {
        this.configuration = configuration;
        this.dbContext = dbContext;
    }

    public async Task<bool> CreateResponse(EvaluationCreateRequest request) {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteResponse(int id) {
        throw new NotImplementedException();
    }

    public async Task<List<MudTest>> GetAllEvaluations() {
        return await dbContext.Evaluations.ToListAsync();
    }

    public async Task<MudTest?> GetEvaluation(int id) {
        return await dbContext.Evaluations.SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<MudTestResponse> GetResponse(int id) {
        throw new NotImplementedException();
    }

    public async Task<List<MudTestResponse>> GetResponsesByUser(User user) {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateResponse(int id, EvaluationUpdateRequest request) {
        throw new NotImplementedException();
    }
}
