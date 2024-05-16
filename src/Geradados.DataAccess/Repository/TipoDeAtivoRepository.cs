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
    public class TipoDeAtivoRepository : RepositoryBase<TipoDeAtivo>, ITipoDeAtivoRepository
    {
        ContextoDataBase ctx = new ContextoDataBase();
        public TipoDeAtivoRepository(ContextoDataBase contexto) : base(contexto)
        {
            ctx = contexto;
        }
    }
}
