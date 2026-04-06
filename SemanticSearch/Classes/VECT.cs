using SemanticSearch.Structures;
using System.Text;

namespace SemanticSearch.Classes
{
    public class VECT
    {
        string word;
        public List<Prefix> wordPrefixes;
        public List<Root> roots;
        public List<Suffix> wordSuffixes;
        public List<Ending> wordEndings;
    }
}
