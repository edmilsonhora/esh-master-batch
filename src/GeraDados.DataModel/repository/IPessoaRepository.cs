using ESH.Master.TesteBatch.DataModel.models;

namespace ESH.Master.TesteBatch.DataModel.repository;

public interface IPessoaRepository: IRepositoryBase<Pessoa>
{
    Pessoa? ObtemPessoaPorCPF(string cpf);
}
