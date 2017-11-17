using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace CrossLib
{
    public class CrosswalkModel
    {
        public string Rua { get; set; }

        public double EsquinaDireitaDistancia { get; set; }

        public double EsquinaEsquedaDistancia { get; set; }

        public double FaixaEsquedaDistancia { get; set; }

        public double FaixaDireitaDistancia { get; set; }

    }
}