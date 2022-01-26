using UnityEngine;
using System.Collections.Generic;
using EthanZarov.SimpleTools;
using System.Text;


namespace EthanZarov.PrefixTries
{
    public class PrefixTrieNode
    {
        private const int AlphabetSize = 26;
        public readonly PrefixTrieNode[] Children = new PrefixTrieNode[26];
        public bool EndOfPath;
        public float WordDifficultyValue;
        public List<string> ActualWords;

        public enum WordDifficulty
        {
            Easy,
            Medium,
            Hard
        }
        public WordDifficulty Difficulty;

        public PrefixTrieNode()
        {
            ActualWords = new List<string>();
            EndOfPath = false;
            for (var i = 0; i < AlphabetSize; i++)
            {
                Children[i] = (null);
            }

        }

        public bool IsCommon => WordDifficultyValue <= 1;
    }

    public class PrefixTrie
    {
        private readonly PrefixTrieNode _root;

        public PrefixTrie()
        {
            _root = GetNode();
        }

        private PrefixTrieNode GetNode()
        {
            return new PrefixTrieNode();
        }

        /// <summary>
        /// Adds the target word to the prefix trie.
        /// </summary>
        /// <param name="word">Target word to add.</param>
        /// <returns>Returns the end node of the word.</returns>
        public PrefixTrieNode AddWord(string word)
        {
            int level;
            var length = word.Length;
            var checkedNode = _root;
 
            
            for (level = 0; level < length; level++)
            {
                var nodeIndex = word[level] - 'A';
                if (nodeIndex < 0) {
                    
                    Debug.LogWarning("Word " + word + " has character " + word[level] + "at index " + level + " that doesn't fit into prefix trie: " + nodeIndex);
                    continue;
                    //Break if out of array bounds
                }

                if (checkedNode.Children[nodeIndex] == null) checkedNode.Children[nodeIndex] = GetNode();
                checkedNode = checkedNode.Children[nodeIndex];
            }

            checkedNode.EndOfPath = true;
            
            
            return checkedNode;
        }

        public void AddWord(string word, float difficulty)
        {
            var checkedNode = AddWord(word);
            checkedNode.WordDifficultyValue = difficulty;
            checkedNode.Difficulty = (PrefixTrieNode.WordDifficulty) (difficulty - 1);
        }

        public void AddAlphabetizedWord(string alphaWord, string actualWord)
        {
            var checkedNode = AddWord(alphaWord);
            if (checkedNode == null)
            {
                return;
            }
            checkedNode.ActualWords = new List<string>();
            checkedNode.ActualWords.Add(actualWord);
        }

        public string AlphabeticToActualWord(string alphaWord)
        {
            var node = GetEndNodeAt(alphaWord);
            if (node == null) return "!";
            string output = "";
            foreach (var t in node.ActualWords)
            {
                output += t + "  ";
            }

            return output;
        }
        
        public int GetWordDifficulty(string word)
        {
            var checkedNode = GetEndNodeAt(word);
            if (checkedNode == null) return -1;
            return (int) checkedNode.Difficulty;
        }

        public bool IsWord(string word)
        {
            var checkedNode = GetEndNodeAt(word);
            return checkedNode != null && checkedNode.EndOfPath;
        }

        public bool IsCommonWord(string word)
        {
            var checkedNode = GetEndNodeAt(word);
            if (checkedNode != null)
            {
                return !checkedNode.EndOfPath || checkedNode.IsCommon;
            }

            return true;
        }

        private PrefixTrieNode GetEndNodeAt(string word)
        {
            int level;
            var length = word.Length;
            var checkedNode = _root;
            for (level = 0; level < length; level++)
            {
                var nodeIndex = word[level] - 'A';
                if (nodeIndex < 0 || nodeIndex > 25) return null;
                if (checkedNode.Children[nodeIndex] == null) return null;
                checkedNode = checkedNode.Children[nodeIndex];
            }

            return checkedNode;
        }

        public List<string> GetAnagrams(string baseString)
        {
            List<string> returnList = new List<string>();
            GetAnagramPerm(baseString.ToCharArray(), 0, baseString.Length - 1, returnList);


            return returnList;
        }

        public List<string> GetAnagramsExclusive(string baseString, List<string> excludedWords)
        {
            List<string> returnList = new List<string>();
            GetAnagramPermExclusive(baseString.ToCharArray(), 0, baseString.Length - 1, returnList, excludedWords);

            return returnList;
        }


        private static void SwapCharacters(ref char a, ref char b)
        {
            if (a == b) return;
            (a, b) = (b, a);
        }

        private void AddPermutation(string permutation, ICollection<string> outputList)
        {
            outputList.Add(permutation);
        }

        public void GetPerm(char[] list, int k, int m, List<string> outputList)
        {

            if (k == m)
            {
                outputList.Add(ArrayToString(list));
            }
            else
            {
                for (int i = k; i <= m; i++)
                {
                    SwapCharacters(ref list[k], ref list[i]);
                    GetPerm(list, k + 1, m, outputList);
                    SwapCharacters(ref list[k], ref list[i]);
                }
            }

        }

        private void GetAnagramPerm(char[] list, int k, int m, ICollection<string> outputList)
        {

            if (k == m)
            {
                var word = ArrayToString(list);
                if (IsWord(word) && !outputList.Contains(word))
                {
                    outputList.Add(ArrayToString(list));
                }
            }
            else
            {
                for (var i = k; i <= m; i++)
                {
                    SwapCharacters(ref list[k], ref list[i]);
                    GetAnagramPerm(list, k + 1, m, outputList);
                    SwapCharacters(ref list[k], ref list[i]);
                }
            }

        }

        private void GetAnagramPermExclusive(char[] list, int k, int m, ICollection<string> outputList,
            ICollection<string> previousWords) //
        {
            var word = ArrayToString(list);
            if (k == m)
            {
                if (IsWord(word) && !outputList.Contains(word) && !previousWords.Contains(word))
                {
                    outputList.Add(ArrayToString(list));
                }
            }
            else
            {
                for (var i = k; i <= m; i++)
                {
                    SwapCharacters(ref list[k], ref list[i]);
                    GetAnagramPermExclusive(list, k + 1, m, outputList, previousWords);
                    SwapCharacters(ref list[k], ref list[i]);
                }
            }
        }

        public void GetAnagramCountPerm(char[] list, int k, int m, List<string> outputList)
        {

            if (k == m)
            {
                string word = ArrayToString(list);
                if (IsWord(word) && !outputList.Contains(word))
                {
                    outputList.Add(word);
                }
            }
            else
            {
                for (int i = k; i <= m; i++)
                {
                    SwapCharacters(ref list[k], ref list[i]);
                    GetAnagramCountPerm(list, k + 1, m, outputList);
                    SwapCharacters(ref list[k], ref list[i]);
                }
            }

        }

        private static string ArrayToString(char[] input)
        {
            StringBuilder sb = new StringBuilder();
            sb = sb.Append(input);
            return sb.ToString();
        }


        public bool HasAnagrams(string baseString)
        {
            var hasAnagram = false;
            HasAnagramPerm(baseString.ToCharArray(), 0, baseString.Length - 1, ref hasAnagram);

            return hasAnagram;
        }

        private void HasAnagramPerm(char[] list, int k, int m, ref bool hasAnagram)
        {

            if (k == m)
            {
                var word = ArrayToString(list);
                if (IsWord(word))
                {
                    hasAnagram = true;

                }
            }
            else
            {
                for (var i = k; i <= m; i++)
                {
                    SwapCharacters(ref list[k], ref list[i]);
                    HasAnagramPerm(list, k + 1, m, ref hasAnagram);
                    SwapCharacters(ref list[k], ref list[i]);
                }
            }

        }
        
        

    }
}
