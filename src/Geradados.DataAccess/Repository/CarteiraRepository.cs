using ESH.Master.TesteBatch.DataAccess.DB;
using ESH.Master.TesteBatch.DataModel.models;
using ESH.Master.TesteBatch.DataModel.repository;

namespace ESH.Master.TesteBatch.DataAccess.Repository
{
    public class CarteiraRepository : RepositoryBase<Carteira>, ICarteiraRepository
    {
        ContextoDataBase ctx = new ContextoDataBase();
        public CarteiraRepository(ContextoDataBase contexto) : base(contexto)
        {
            ctx = contexto;
        }
    }
}