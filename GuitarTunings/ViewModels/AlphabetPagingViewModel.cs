using System.Collections.Generic;
using System.Linq;

namespace GuitarTunings.ViewModels 
{

    public class AlphabetPagingViewModel
    {
        public List<string> Names { get; set; }
        public List<int> IDs { get; set; }
        public Dictionary<int, string> Dict { get; set; }
        public List<string> Alphabet
        {
            get
            {
                var alphabet = Enumerable.Range(65, 26).Select(i => ((char)i).ToString()).ToList();
                alphabet.Insert(0, "All");
                alphabet.Insert(1, "0-9");
                return alphabet;
            }
        }
        public List<string> FirstLetters { get; set; }
        public string SelectedLetter { get; set; }
        public bool NamesStartWithNumbers
        {
            get
            {
                var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());
                return FirstLetters.Intersect(numbers).Any();
            }
        }
    }
}  