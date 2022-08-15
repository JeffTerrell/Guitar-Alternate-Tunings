using GuitarTunings.Models;
using System.Collections.Generic;
using System.Linq;

namespace GuitarTunings.ViewModels
{


    public class AlphabetPagingViewModel<T>
    {
        public List<string> Names { get; set; }
        public List<T> GenericList { get; set; }
        private readonly List<string> _alphabet = Enumerable.Range(65, 26).Select(i => ((char)i).ToString()).ToList();
        public IEnumerable<string> Alphabet { get {return _alphabet;}}

        // public List<string> Alphabet
        // {
        //     get
        //     {
        //         var alphabet = Enumerable.Range(65, 26).Select(i => ((char)i).ToString()).ToList();
        //         alphabet.Insert(0, "All");
        //         alphabet.Insert(1, "0-9");
        //         return alphabet;
        //     }
        // }
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
        public void ClearList()
        {
            _alphabet.Clear();
        }
        public void AddToList(List<string> newList)
        {
            _alphabet.AddRange(newList);
        }
        public void AddToListAllAndNumbers()
        {
            _alphabet.Insert(0, "All");
            _alphabet.Insert(1, "0-9");
        }
    }
}  