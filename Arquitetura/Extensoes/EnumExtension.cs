using System.ComponentModel;

namespace ANI.Arquitetura.Extensoes
{
    public static class EnumExtension
    {
        public static string ObterDescricao(this Enum valor)
        {
            DescriptionAttribute[] atributos = (DescriptionAttribute[])valor
                .GetType()
                .GetField(valor.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);

            return atributos.Length > 0 ? atributos[0].Description : "";
        }

        public static char ObterDescricao(this Enum valor, bool descricaoChar)
        {
            return ObterDescricao(valor)[0];
        }
    }
}
