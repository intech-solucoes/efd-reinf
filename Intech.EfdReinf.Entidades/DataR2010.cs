using System.Collections.Generic;

namespace Intech.EfdReinf.Entidades
{
    public class AnoR2010
    {
        public List<MesR2010> Meses { get; set; }
        public int Ano { get; set; }
    }

    public class MesR2010
    {
        public int Ano { get; set; }
        public int Mes { get; set; }

        public string Data => $"{Mes.ToString().PadLeft(2, '0')}/{Ano}";
        public string DataInvertida => $"{Ano}-{Mes.ToString().PadLeft(2, '0')}";
    }
}