﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ESH.Master.TesteBatch.DataModel.models;

[Table("Enderecos")]
public class Endereco : EntityBase
{
    public Endereco()
    {
        Pessoa = new Pessoa();
        CEP = string.Empty;
        Logradouro = string.Empty;
        Numero = 0;
        Bairro = string.Empty;
        Cidade = string.Empty;
        Estado = string.Empty;
    }

    public Pessoa Pessoa { get; set; }
    public string CEP { get; set; }
    public string Logradouro { get; set; }
    public int Numero { get; set; } = 0;
    public string  Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }

    public override void Valida()
    {
        ValidaCampoTexto(CEP, "CEP");
        ValidaCampoTexto(Logradouro, "Logradouro");
        ValidaCampoNumerico(Numero, "Número");
        ValidaCampoTexto(Bairro, "Bairro");
        ValidaCampoTexto(Cidade, "Cidade");
        ValidaCampoTexto(Estado, "Estado");
        base.Valida();
    }
}