using System;
using System.Collections.Generic;

namespace FicaEmCasaFunctionApp.Entities
{
    public class Produto
    {
        public static readonly List<Produto> Itens = new List<Produto>
        {
            new Produto {Nome = "Mouse"},
            new Produto {Nome = "Teclado"},
            new Produto {Nome = "Camera"}
        };

        private int _estoque;

        public Produto()
        {
            Id = Guid.NewGuid();
            Estoque = 5000;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }

        public int Estoque
        {
            get => _estoque;
            private set => _estoque = Math.Max(0, value);
        }

        public void AtualizarEstoque(int quantidade)
        {
            Estoque += quantidade;
        }
    }
}