using ESH.Master.TesteBatch.DataModel.models;

namespace ESH.Master.TesteBatch.DataModel.repository;

public interface IEnderecoRepository: IRepositoryBase<Endereco>
{
    Endereco? ObtemEnderecoPorIdPessoa(int idPessoa);
}
