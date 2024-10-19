using ConMirellaApi.Data;
using ConMirellaApi.Data.Models;
using ConMirellaApi.Endpoints.Valuates.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ConMirellaApi.Endpoints.Valuates;

public class CreateValuateEndpoint : Endpoint<CreateValuateRequest, Results<Created<Valuate>, Conflict, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public CreateValuateEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/valuates");
        AllowAnonymous();
    }

    public override async Task<Results<Created<Valuate>, Conflict, ProblemDetails>> ExecuteAsync(CreateValuateRequest req, CancellationToken ct)
    {
        if(_dbContext.Groups.FirstOrDefault(x => x.Id == req.GroupId) is null)
            AddError("Given group does not exists");
        
        ThrowIfAnyErrors();

        if (_dbContext.Valuates.Any(x => x.GroupId == req.GroupId && x.IP == req.IP))
            return TypedResults.Conflict();
        
        var valuate = new Valuate 
        { 
            Punctuation = req.Punctuation, 
            Description = req.Description, 
            IP = req.IP, 
            GroupId = req.GroupId 
        };

        await _dbContext.Valuates.AddAsync(valuate, ct);
        await _dbContext.SaveChangesAsync(ct);

        return TypedResults.Created($"/valuates/{valuate.Id}", valuate);
    }
}