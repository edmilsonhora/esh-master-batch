using ESH.Master.TesteBatch.DataAccess.DB;
using ESH.Master.TesteBatch.DataModel.models;
using ESH.Master.TesteBatch.DataModel.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESH.Master.TesteBatch.DataAccess.Repository
{
    public class EnderecoRepository : RepositoryBase<Endereco>, IEnderecoRepository
    {
        ContextoDataBase ctx = new ContextoDataBase();
        public EnderecoRepository(ContextoDataBase contexto) : base(contexto)
        {
            ctx = contexto;
        }

        public Endereco? ObtemEnderecoPorIdPessoa(int idPessoa)
        {
            return ctx.Enderecos.FirstOrDefault(endereco => endereco.Pessoa.ID.Equals(idPessoa));
        }
    }
}
