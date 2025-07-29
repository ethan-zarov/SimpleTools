using System.Collections.Generic;
using System.Linq;
using EthanZarov.SimpleTools;
using UnityEngine;

namespace EthanZarov.PrefixTries
{
    public class WordDictionary : MonoBehaviour
    {
   
        private PrefixTrie[] _validDictionary;

        [SerializeField, Tooltip("Overall list of alphabetized strings that constitute words. For example, AGORT is on the list for GATOR, but not GATOR itself.")]
        private TextAsset alphabetizedDictionaryTextAsset;
        [SerializeField, Tooltip("List of every acceptable word in the dictionary")]
        private TextAsset validDictionaryTextAsset;
        private List<string> _outputList;
        
        [Header("Difficulty-Based Dictionary")]
        [SerializeField] private bool useDifficultyDictionaries;
        public TextAsset dictionaryEasy;
        public TextAsset dictionaryMedium;
        public TextAsset dictionaryHard;

        [Space] 
        [SerializeField] private bool singularTree;

        private PrefixTrie totalTree;
        private void Awake()
        {
            _matchAnagramsAlloc = new List<string>();
            InitializeDictionary();
        }

        private void InitializeDictionary()
        {
            //Create tries
            _validDictionary = new PrefixTrie[30];
            for (var i = 0; i < 26; i++)
            {
                _validDictionary[i] = new PrefixTrie();
            } 

            //Create alphabetical-string dictionary (for anagrams etc.)
            var dictList = alphabetizedDictionaryTextAsset.text.Split('\n');
            if (singularTree)
            {
                totalTree = new PrefixTrie();
                
            }
            
            foreach (var t in dictList)
            {

                var addedString = t.Replace("\r", "").ToUpper();
                
                
                if (singularTree) totalTree.AddWord(addedString);
                addedString = addedString.Alphabetize();
                var length = addedString.Length;
            }

            //Create valid lookup dictionary
            AddWordsFromTextFile(validDictionaryTextAsset,1);

            if (!useDifficultyDictionaries) return;
            AddWordsFromTextFile(dictionaryEasy, 1);
            AddWordsFromTextFile(dictionaryMedium, 2);
            AddWordsFromTextFile(dictionaryHard, 3);


           
        }

        private void AddWordsFromTextFile(TextAsset textFile, int difficulty)
        {
            var dictList = textFile.text.Split('\n');
            foreach (var t in dictList)
            {
                var addedString = t.Replace("\r", "").ToUpper();
                if (addedString.Length >= 3 && addedString.Length < 25)
                {
                    _validDictionary[addedString.Length - 3].AddWord(addedString, difficulty);
                }
            }
        }


        /// <summary>
        /// Check if a word is valid.
        /// </summary>
        /// <param name="word">Word to target</param>
        /// <returns></returns>
        public bool CheckWord(string word)
        {
            if (word.Length<3) return false;
            return !word.Contains("?") && _validDictionary[word.Length - 3].IsWord(word.ToUpper());
        }
        
        public PrefixTrie GetTrie(int wordLength)
        {
            return _validDictionary[wordLength - 3];
        }


        public bool IsStartToWord(string letters)
        {
            if (!singularTree)
            {
                print("CHECK SINGULAR TREE");
                return false;
            }

            return totalTree.IsStartOfWord(letters);
        }


        /// <summary>
        /// Check if a word is common.
        /// It's recommended to check whether the word you're looking for is valid first!
        /// </summary>
        /// <param name="word">Checked word.</param>
        /// <returns>True if the word has a difficulty of 1.</returns>
        public bool IsCommonWord(string word)
        {
            if (word.Contains("?") || word.Contains(" "))
            {
                return true;
            }

            return _validDictionary[word.Length-3].IsCommonWord(word.ToUpper());
        }

        /// <summary>
        /// Check if a word is rare.
        /// It's recommended to check whether the word you're looking for is valid first!
        /// </summary>
        /// <param name="word">Checked word.</param>
        /// <returns>True if the word has a difficulty of 2 or higher.</returns>
        public bool IsRareWord(string word)
        {
            if (word.Contains("?") || word.Contains(" "))
            {
                return false;
            }

            return _validDictionary[word.Length - 3].GetWordDifficulty(word.ToUpper()) > 1;
        }

    
    
        private List<string> _matchAnagramsAlloc; //Permanent storage are for list for below function
        /// <summary>
        /// Given a handful of letters, can a valid English word be formed with them to make a word that fits a template?
        /// See parameter descriptions for example.
        /// </summary>
        /// <param name="providedLetters">The letters available to fit into the template. Ex: AELPPS</param>
        /// <param name="template">A template that the alphaString must try and fit into. Ex: ?P??ES</param>
        /// <param name="bestWord">If returns true, provides the simplest word that can fit in the given template. Ex: AELPPS -> ?P??ES = APPLES</param>
        /// <returns>Returns true if a fit into template was found with providedLetters.
        /// Returns false if providedLetters does not have a letter needed by the template, or no valid words can fit the template.</returns>
        public bool CheckForWordsMatchingAlphabeticalString(string providedLetters, string template, out string bestWord)
        {
            bestWord = "";

            var output = false;
            var mostEasyValue = int.MaxValue;

            var alphaChars = providedLetters.ToList();
            //Check to make sure that alpha-string has all needed letters to fulfill template
            foreach (var t in template)
            {
                if (t.Equals('?')) continue;


                //If there is a needed letter, and the alphaChars list has no letter to match it, then there is no anagram of those letters that can fit into template.
                var foundMatch = false;
                for (var j = 0; j < alphaChars.Count; j++)
                {
                    if (!alphaChars[j].Equals(t)) continue;
                    alphaChars.RemoveAt(j);
                    foundMatch = true;
                }

                if (!foundMatch) return false;
            }
            _matchAnagramsAlloc.Clear();
            _matchAnagramsAlloc = GetAnagrams(providedLetters);

            foreach (var word in _matchAnagramsAlloc)
            {
                var possibleSolution = !template.Where((t, i) => !t.Equals('?') && !word[i].Equals(t)).Any();


                if (!possibleSolution) continue;
                var wd = _validDictionary[word.Length - 3].GetWordDifficulty(word);
                if (wd < mostEasyValue)
                {

                    bestWord = word;
                    mostEasyValue = wd;
                }

                output = true;

            }


            return output;
        }
    
        // /// <summary>
        // /// Checks whether a string of characters
        // /// </summary>
        // /// <param name="providedLetters">Letters to rearrange.</param>
        // /// <returns></returns>
        // public bool HasAnagram(string providedLetters)
        // {
        //     if (providedLetters.Length < 3) return false;
        //     if (providedLetters.Contains("?"))
        //     {
        //         return false;
        //     }
        //
        //     var upper = providedLetters.ToUpper();
        //
        //     //If the alphabetical dictionary has the same list of character
        //     return _alphaDictionary[providedLetters.Length - 3].IsWord(upper.Alphabetize());
        // }
    
        /// <summary>
        /// Get one possible anagram of a string of letters.
        /// </summary>
        /// <param name="letters"></param>
        /// <returns>Returns "!" if no anagram exists. Otherwise, returns a random anagram of all possible anagrams.</returns>
        public string GetAnagram(string letters)
        {
            return GetAnagrams(letters).GetRandomItem();
        }

        /// <summary>
        /// Get all possible anagrams provided a string of letters.
        /// </summary>
        /// <param name="letters">Letters to rearrange.</param>
        /// <returns>Returns list of all possible anagrams.</returns>
        public List<string> GetAnagrams(string letters)
        {
            return _validDictionary[letters.Length - 3].GetAnagrams(letters);
        }

    
        /// <summary>
        /// Get a list of anagrams excluding certain words.
        /// </summary>
        /// <param name="input">String to find anagrams of.</param>
        /// <param name="excludedWords">Words to exclude from results.</param>
        /// <returns></returns>
        public List<string> GetAllAnagramsExclusive(string input, List<string> excludedWords)
        {
            return _validDictionary[input.Length-3].GetAnagramsExclusive(input, excludedWords);
        }
    
    
        // Helpers
        /// <summary>
        /// Convert a string to an array of characters.
        /// </summary>
        /// <param name="input">String to convert.</param>
        /// <returns>Character array with letters of string in order.</returns>
        private char[] StringToCharArray(string input)
        {
            var returnArray = new char[input.Length];
            for (var i = 0; i < input.Length; i++)
            {
                returnArray[i] = input[i];
            }
            return returnArray;
        }

    }
}