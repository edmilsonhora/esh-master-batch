using ESH.Master.TesteBatch.DataAccess.DB;
using ESH.Master.TesteBatch.DataModel.models;
using ESH.Master.TesteBatch.DataModel.repository;

namespace ESH.Master.TesteBatch.DataAccess.Repository;

public class TipoContatoRepository : RepositoryBase<TipoContato>, ITipoContatoRepository
{
    ContextoDataBase ctx = new ContextoDataBase();
    public TipoContatoRepository(ContextoDataBase contexto) : base(contexto)
    {
        ctx = contexto;
    }
}
