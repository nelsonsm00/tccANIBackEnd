using HtmlAgilityPack;

namespace ANI.Arquitetura
{
    public class GerenciadorHTML
    {
        public static string[] splitTD(HtmlNode node)
        {
            string tdNodes = node.InnerHtml.Replace("<td>", "");
            return tdNodes.Split("</td>");
        }

        public static string getInfoLink(string info)
        {
            return info.Split('>')[1].Split('<')[0];
        }

        public static string[] splitTH(HtmlNode node)
        {
            var tr = node.SelectNodes("th");
            List<string> trNode = new List<string>();
            foreach(HtmlNode t in tr)
                trNode.Add(t.InnerText);
            return trNode.ToArray();
        }
    }
}
