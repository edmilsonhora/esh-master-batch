using ESH.Master.TesteBatch.DataAccess.DB;
using ESH.Master.TesteBatch.DataModel.models;
using ESH.Master.TesteBatch.DataModel.repository;

namespace ESH.Master.TesteBatch.DataAccess.Repository;

public class ContatoRepository : RepositoryBase<Contato>, IContatoRepository
{
    ContextoDataBase ctx = new ContextoDataBase();
    public ContatoRepository(ContextoDataBase contexto) : base(contexto)
    {
        ctx = contexto;
    }

    IList<Contato> IContatoRepository.ObtemContatosPorIdPessoa(int idPessoa)
    {
        return ctx.Contatos.Where(contato => contato.Pessoa.ID.Equals(idPessoa)).ToList();
    }
}