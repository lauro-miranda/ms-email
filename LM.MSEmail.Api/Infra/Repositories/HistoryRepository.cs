using LM.Infra.Repositories;
using LM.MSEmail.Api.Domain.Models;
using LM.MSEmail.Api.Domain.Repositories;
using LM.MSEmail.Api.Infra.Context;

namespace LM.MSEmail.Api.Infra.Repositories
{
    public class HistoryRepository : Repository<History, EmailContext>, IHistoryRepository
    {
        public HistoryRepository(EmailContext context) : base(context) { }
    }
}