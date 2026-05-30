using Microsoft.EntityFrameworkCore;
using task.rira.Abstractions;
using task.rira.Data;
using task.rira.sdk;

namespace task.rira.Implementations;

public class PersonRepo(AppDbContext ctx) : IPersonRepo
{
    public async Task Delete(int id)
    {
        await ctx.People.Where(a => a.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<PersonDto> Find(int id)
    {
        var ent = await ctx.People.FirstOrDefaultAsync(a => a.Id == id);
        return new PersonDto()
        {
            Id = ent.Id,
            FirstName = ent.FirstName,
            LastName = ent.LastName,
            BirthDate = ent.BirthDate,
            NationalId = ent.NationalId,
        };
    }

    public async Task<int> Insert(PersonDto value)
    {

        var ent = new Person()
        {
            BirthDate = value.BirthDate,
            FirstName = value.FirstName,
            LastName = value.LastName,
            NationalId = value.NationalId,
        };
        ctx.People.Add(ent);
        await ctx.SaveChangesAsync();
        return ent.Id;
    }
    public async Task<bool> Update(PersonDto value)
    {
        var res = await ctx.People
            .Where(a => a.Id == value.Id)
            .ExecuteUpdateAsync(a => a
                .SetProperty(p => p.NationalId, value.NationalId)
                .SetProperty(p => p.FirstName, value.FirstName)
                .SetProperty(p => p.LastName, value.LastName)
                .SetProperty(p => p.BirthDate, value.BirthDate)
            );
        return res > 0;
    }
}