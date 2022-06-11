using CbtBackend.Entities;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CbtBackend.Services;

public class ResponseNotFoundException : Exception { }
public class EvaluationNotFoundException : Exception { }

public class EvaluationService : IEvaluationService {
    private readonly CbtDbContext dbContext;

    public EvaluationService(CbtDbContext dbContext) {
        this.dbContext = dbContext;
    }

    public async Task<MudTestResponse> CreateResponse(User user, EvaluationCreateRequest request) {
        var evaluation = await GetEvaluation(request.TestId);

        if (evaluation == null) {
            throw new EvaluationNotFoundException();
        }

        var response = new MudTestResponse() {
            Author = user,
            Evaluation = evaluation,
            Submitted = DateTime.UtcNow,
            Response1 = request.Response1,
            Response2 = request.Response2,
            Response3 = request.Response3,
            Response4 = request.Response4,
            Response5 = request.Response5
        };

        await dbContext.EvaluationResponses.AddAsync(response);

        var registered = await dbContext.SaveChangesAsync();
        if (registered <= 0) {
            throw new Exception("adding response to db failed");
        }

        return response;
    }

    public async Task<bool> DeleteResponse(int id) {
        var response = await GetResponse(id);

        if (response == null) {
            throw new ResponseNotFoundException();
        }

        dbContext.Remove(response);
        var updated = await dbContext.SaveChangesAsync();

        if (updated <= 0) {
            throw new Exception("deleting response in db failed");
        }

        return true;
    }

    public Task<List<MudTest>> GetAllEvaluations() {
        return dbContext.Evaluations
            .Include(e => e.ResultsTable)
            .Include(e => e.ResultsTable.Entries)
            .ToListAsync();
    }

    public Task<MudTest?> GetEvaluation(int id) {
        return dbContext.Evaluations
            .Include(e => e.ResultsTable)
            .Include(e => e.ResultsTable.Entries)
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public Task<MudTestResponse?> GetResponse(int id) {
        return dbContext.EvaluationResponses
            .Include(e => e.Evaluation)
                .Include(e => e.Evaluation.ResultsTable)
                .Include(e => e.Evaluation.ResultsTable.Entries)
            .Include(e => e.Author)
            .SingleOrDefaultAsync(e => e.Id == id);
    }

    public Task<List<MudTestResponse>> GetResponsesByUser(User user) {
        return dbContext.EvaluationResponses
            .Include(e => e.Evaluation)
                .Include(e => e.Evaluation.ResultsTable)
                .Include(e => e.Evaluation.ResultsTable.Entries)
            .Where(e => e.Author.Id == user.Id).ToListAsync();
    }

    public async Task<MudTestResponse> UpdateResponse(int id, EvaluationUpdateRequest request) {
        var response = await GetResponse(id);
        if (response == null) {
            throw new ResponseNotFoundException();
        }

        if (request.Response1 is int res1) {
            response.Response1 = res1;
        }

        if (request.Response2 is int res2) {
            response.Response2 = res2;
        }

        if (request.Response3 is int res3) {
            response.Response3 = res3;
        }

        if (request.Response4 is int res4) {
            response.Response4 = res4;
        }

        if (request.Response5 is int res5) {
            response.Response5 = res5;
        }

        dbContext.ChangeTracker.Clear();
        dbContext.Update(response);
        var updated = await dbContext.SaveChangesAsync();

        if (updated <= 0) {
            throw new Exception("updating response in db failed");
        }

        return response;
    }
}
