using ESH.Master.TesteBatch.DataModel.models;

namespace ESH.Master.TesteBatch.DataModel.repository;

public interface IContatoRepository : IRepositoryBase<Contato>
{
    IList<Contato> ObtemContatosPorIdPessoa(int idPessoa);
}
