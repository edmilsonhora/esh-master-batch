using ESH.Master.TesteBatch.DataAccess.DB;
using ESH.Master.TesteBatch.DataModel.models;
using ESH.Master.TesteBatch.DataModel.repository;

namespace ESH.Master.TesteBatch.DataAccess.Repository;

public class PessoaRepository : RepositoryBase<Pessoa>, IPessoaRepository
{
    ContextoDataBase ctx = new ContextoDataBase();
    public PessoaRepository(ContextoDataBase contexto) : base(contexto)
    {
        ctx = contexto;
    }

    public Pessoa? ObtemPessoaPorCPF(string cpf)
    {
        return ctx.Pessoas.FirstOrDefault(pessoa => pessoa.CPF.Equals(cpf));
    }
}