using ESH.Master.TesteBatch.DataModel.models;

namespace ESH.Master.TesteBatch.DataModel.repository;

public interface IAtivoRepository : IRepositoryBase<Ativo>
{
    List<Ativo> ObtemAtivosPorTipoDeAtivo(TipoDeAtivo? tipoDeAtivo);
}