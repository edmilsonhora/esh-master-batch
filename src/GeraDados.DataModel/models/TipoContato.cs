﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ESH.Master.TesteBatch.DataModel.models;

[Table("TipoContatos")]
public class TipoContato : EntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public new int ID { get; set; }
    public TipoContato()
    {
        Descricao = string.Empty;
    }

    public string Descricao { get; set; }

    public List<TipoContato> CarregaListaTipoContato()
    {
        List<TipoContato> tipoContatos = new List<TipoContato>()
        {
            new TipoContato() {
                ID = 1,
                Descricao = "E-mail",
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now,
            },
            new TipoContato() {
                ID = 2,
                Descricao = "Telefone Fixo",
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now,
            },
            new TipoContato() {
                ID = 3,
                Descricao = "Celular",
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now,
            }
        };
        return tipoContatos;
    }
}