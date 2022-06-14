using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleMedicamentos.Infra.BancoDados.Compartilhado
{
    public class DB
    {
        public string EnderecoBanco()
        {
           return
             "Data Source=(LocalDB)\\MSSqlLocalDB;" +
             "Initial Catalog=controleMedicamentosDB;" +
             "Integrated Security=True;" +
             "Pooling=False";
        }
    }
}
