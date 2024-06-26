using ESH.Master.TesteBatch.DataAccess.DB.Dtos;
using ESH.Master.TesteBatch.DataAccess.Repository;
using ESH.Master.TesteBatch.DataModel.models;
using Newtonsoft.Json;

namespace ESH.Master.TesteBatch.App;

public partial class frmUpload : Form
{
    Repository repository = new Repository();

    #region M�todos Do Forms
    public frmUpload()
    {
        InitializeComponent();
        InicializaDadosNoBanco();
    }
    private void btnUpload_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.CheckFileExists = true;
        ofd.Multiselect = false;
        if (ofd.ShowDialog() == DialogResult.OK)
            txtArquivo.Text = ofd.FileName;
    }
    private void btnSalvar_Click(object sender, EventArgs e)
    {
        try
        {
            IList<PessoaJson>? pessoas = LerArquivoJson<PessoaJson>(txtArquivo.Text);

            if (pessoas != null)
                foreach (var item in pessoas)
                {
                    if (repository.Pessoa.ObtemPessoaPorCPF(item.CPF) != null)
                        continue;

                    Pessoa pessoa = NovaPessoa(item);
                    List<Contato> contatos = ContatosPessoa(item, pessoa);
                    Endereco endereco = EnderecoPessoa(item, pessoa);
                    CarteiraConfigurada carteiraConfigurada = NovaCarteiraConfiguracao();

                    repository.Pessoa.Salvar(pessoa);
                    foreach (Contato contato in contatos)
                        repository.Contato.Salvar(contato);
                    repository.Endereco.Salvar(endereco);
                    ValidaSeCampoCotaEstaZerado(carteiraConfigurada.Acoes, pessoa, carteiraConfigurada.ValorPorAcoes);
                    ValidaSeCampoCotaEstaZerado(carteiraConfigurada.Fiis, pessoa, carteiraConfigurada.ValorPorFiis);
                    repository.SaveChanges();
                }

            MessageBox.Show("Salvo");
            txtArquivo.Text = string.Empty;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Aten��o", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
    #endregion

    #region M�todos void
    private void InicializaDadosNoBanco()
    {
        if (repository.TipoContato.ObterTodos().Count.Equals(0))
            SalvaTipoDeContatosNoBanco();
        if (repository.TipoDeAtivo.ObterTodos().Count.Equals(0))
            SalvaTipoDeAtivosNoBanco();
        TipoDeAtivo? acao = repository.TipoDeAtivo.ObterPorId((int)eTipoDeAtivo.Acao);
        if (repository.Ativo.ObtemAtivosPorTipoDeAtivo(acao).Count.Equals(0))
            SalvarAtivosNoBancoDeDados(acao);
        TipoDeAtivo? fii = repository.TipoDeAtivo.ObterPorId((int)eTipoDeAtivo.FundoImobiliario);
        if (repository.Ativo.ObtemAtivosPorTipoDeAtivo(fii).Count.Equals(0))
            SalvarAtivosNoBancoDeDados(fii);
    }
    private void SalvarAtivosNoBancoDeDados(TipoDeAtivo? tipoDeAtivo)
    {
        if (tipoDeAtivo != null)
            if (tipoDeAtivo.ID.Equals((int)eTipoDeAtivo.Acao))
            {
                IList<AtivoJson>? acoes = LerArquivoJson<AtivoJson>(@"..\..\..\CargaDeAtivos\acoes.json");
                if (acoes != null)
                    SalvaListaDeAtivos(acoes.ToList(), tipoDeAtivo);
            }
            else if (tipoDeAtivo.ID.Equals((int)eTipoDeAtivo.FundoImobiliario))
            {
                IList<AtivoJson>? fii = LerArquivoJson<AtivoJson>(@"..\..\..\CargaDeAtivos\fiis.json");
                if (fii != null)
                    SalvaListaDeAtivos(fii.Where(fii => fii.Ultimo != "0").ToList(), tipoDeAtivo);
            }
    }
    private void SalvaListaDeAtivos(List<AtivoJson> ativos, TipoDeAtivo tipoDeAtivo)
    {
        foreach (AtivoJson item in ativos)
        {
            Ativo ativo = NovoAtivo(item, tipoDeAtivo);
            repository.Ativo.Salvar(ativo);
            repository.SaveChanges();
        }
    }
    private void SalvaTipoDeAtivosNoBanco()
    {
        List<TipoDeAtivo> listaTipoDeAtivos = new TipoDeAtivo().CarregaTipoDeAtivo();
        foreach (TipoDeAtivo tipoDeAtivo in listaTipoDeAtivos)
            repository.TipoDeAtivo.Salvar(tipoDeAtivo);
        repository.SaveChanges();
    }
    private void SalvaTipoDeContatosNoBanco()
    {
        List<TipoContato> listadeTipocontatos = new TipoContato().CarregaListaTipoContato();
        foreach (TipoContato tipoContato in listadeTipocontatos)
            repository.TipoContato.Salvar(tipoContato);
        repository.SaveChanges();
    } 
    private void ValidaSeCampoCotaEstaZerado(List<Ativo> Ativos, Pessoa pessoa, double valorPorAtivo)
    {
        foreach (var ativo in Ativos)
        {
            Carteira carteira = NovoAtivoParaCarteira(pessoa, ativo, valorPorAtivo);
            if (carteira.Cota > 0)
                repository.Carteira.Salvar(carteira);
        }
    }
    #endregion

    #region M�todos com return
    private IList<T>? LerArquivoJson<T>(string caminho)
    {
        return JsonConvert.DeserializeObject<IList<T>>(LeitorDeArquivo(caminho));
    }
    private string LeitorDeArquivo(string caminho)
    {
        StreamReader reader = new StreamReader(caminho);
        return reader.ReadToEnd();
    }
    private List<Ativo> ObtemListaDeAtivosPorTipoDeAtivo(int idTipoAtivo)
    {
        return repository.Ativo.ObtemAtivosPorTipoDeAtivo(repository.TipoDeAtivo.ObterPorId(idTipoAtivo));
    }
    private List<Ativo> ListaDeAtivosAleatoriaEComQuantidadeDeAtivo(List<Ativo> listaAtivo, int quantidade, Random ordenaLista)
    {
        return listaAtivo.OrderBy(acoes => ordenaLista.Next()).Take(quantidade).ToList();
    }
    private Carteira NovoAtivoParaCarteira(Pessoa pessoa, Ativo ativo, double valorPorAtivo)
    {
        return new Carteira()
        {
            Pessoa = pessoa,
            Ativo = ativo,
            Cota = Carteira.QuantidadeDeUmAtivo(valorPorAtivo, (double)ativo.UltimaNegociacao),
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now,
        };
    }
    private CarteiraConfigurada NovaCarteiraConfiguracao()
    {
        Random rdm = new Random();
        int valorInicial = Carteira.InicializaValorInicialDaPessoa();
        double valorParaAcoes = Carteira.PorcentagelDoValorParaUmTipoDeAtivo(valorInicial);
        int qtdAcoes = rdm.Next(5, 30);
        double valorPorAcoes = valorParaAcoes / qtdAcoes;
        double valorParaFiis = valorInicial - valorParaAcoes;
        int qtdFiis = rdm.Next(5, 15);
        double valorPorFiis = valorParaFiis / qtdFiis;
        double valorTotal = valorParaAcoes + valorParaFiis;
        List<Ativo> listaAcoes = ObtemListaDeAtivosPorTipoDeAtivo((int)eTipoDeAtivo.Acao);
        List<Ativo> listaFiis = ObtemListaDeAtivosPorTipoDeAtivo((int)eTipoDeAtivo.FundoImobiliario);
        
        return new CarteiraConfigurada()
        {
            ValorInicial = valorInicial,
            ValorParaAcoes = valorParaAcoes,
            QuantidadeDeAcoes = qtdAcoes,
            ValorPorAcoes = valorPorAcoes,
            ValorParaFiis = valorParaFiis,
            QuantidaDeFiis = qtdFiis,
            ValorPorFiis = valorPorFiis,
            ValorTotal = (int)valorTotal,
            Acoes = ListaDeAtivosAleatoriaEComQuantidadeDeAtivo(listaAcoes, qtdAcoes, rdm),
            Fiis = ListaDeAtivosAleatoriaEComQuantidadeDeAtivo(listaFiis, qtdFiis, rdm),
        };
    }
    private Ativo NovoAtivo(AtivoJson? item, TipoDeAtivo? tipoDeAtivo)
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        Ativo ativo = new Ativo()
        {
            TipoDeAtivo = tipoDeAtivo,
            Nome = item.Nome,
            Ticker = item.Ticker,
            UltimaNegociacao = Convert.ToDecimal($"{item.Ultimo},{item.Decimal}"),
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now,
        };
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        ativo.Valida();
        return ativo;
    }
    private Endereco EnderecoPessoa(PessoaJson pessoaJson, Pessoa pessoa)
    {
        Endereco endereo = new Endereco()
        {
            Pessoa = pessoa,
            Bairro = pessoaJson.Bairro,
            CEP = pessoaJson.CEP,
            Cidade = pessoaJson.Cidade,
            Estado = pessoaJson.Estado,
            Logradouro = pessoaJson.Endereco,
            Numero = pessoaJson.Numero,
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now
        };

        endereo.Valida();
        return endereo;
    }
    private Pessoa NovaPessoa(PessoaJson pessoaJson)
    {
        var pessoa = new Pessoa()
        {
            Nome = pessoaJson.Nome,
            CPF = pessoaJson.CPF,
            RG = pessoaJson.RG,
            Sexo = pessoaJson.Sexo,
            DataNascimento = Convert.ToDateTime(pessoaJson.Data_nasc),
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now
        };
        pessoa.Valida();
        return pessoa;
    }
    private List<Contato> ContatosPessoa(PessoaJson pessoaJson, Pessoa pessoa)
    {
        List<Contato> contatos = new List<Contato>()
        {
            new Contato()
            {
                Pessoa = pessoa,
                TipoContato = CarregaTipoContato((int)eTipoContato.Email),
                Valor = pessoaJson.Email,
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            },
             new Contato()
            {
                Pessoa = pessoa,
                TipoContato = CarregaTipoContato((int)eTipoContato.Fixo),
                Valor = pessoaJson.Telefone_fixo,
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            },
            new Contato()
            {
                Pessoa = pessoa,
                TipoContato = CarregaTipoContato((int)eTipoContato.Celular),
                Valor = pessoaJson.Celular,
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now
            }
        };

        foreach (var item in contatos)
            item.Valida();
        return contatos;
    }
    private TipoContato? CarregaTipoContato(int id)
    {
        return repository.TipoContato.ObterPorId(id);
    }
    #endregion
}