namespace EthanZarov.TSVReader
{
    public class TsvReader
    {
        public static string[] SeparateTsvLines(string tsvFull)
        {
            return tsvFull.Split('\n');
        }

        public static string[] SeparateTsvLine(string tsvLine)
        {
            return tsvLine.Split('\t');
        }
    }
}
