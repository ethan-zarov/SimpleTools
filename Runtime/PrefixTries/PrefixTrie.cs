using UnityEngine;
using System.Collections.Generic;
using EthanZarov.SimpleTools;
using System.Text;
using JetBrains.Annotations;


namespace EthanZarov.PrefixTries
{
    public class PrefixTrieNode
    {
        private const int AlphabetSize = 26;
        public readonly PrefixTrieNode[] Children = new PrefixTrieNode[26];
        public bool EndOfPath;
        public float WordDifficultyValue;
        public int TotalWords;
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

        public int TotalWords()
        {
            //Traverse to all leafs and add the ActualWords at it
            return TotalWords(_root);
        }
        
        private int TotalWords(PrefixTrieNode node)
        {
            int total = 0;
            if (node.EndOfPath)
            {
                total++;
            }

            for (int i = 0; i < 26; i++)
            {
                if (node.Children[i] != null)
                {
                    total += TotalWords(node.Children[i]);
                }
            }

            return total;
        }
        
        public string GetWordAt(int index)
        {
            return GetWordAt(_root, index, "");
        }
        
        //Loop using total words to skip certain char indices and get to the right location
        private string GetWordAt(PrefixTrieNode node, int index, string currentString)
        {
            if (node.EndOfPath)
            {
                return currentString;
            }

            for (int i = 0; i < 26; i++)
            {
                var child = node.Children[i];
                if (child != null)
                {
                    int childWordCount = child.TotalWords;
                    if (index - childWordCount <= 0)
                    {
                        return GetWordAt(child, index, currentString + (char) (i + 'A'));
                    }
                    else
                    {
                        index -= childWordCount;
                    }
                }
            }

            return "";
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
 
            checkedNode.TotalWords++;
            
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
                checkedNode.TotalWords++;
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

        public bool IsStartOfWord(string letters)
        {
            return GetEndNodeAt(letters) != null;
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

        public List<string> GenerateWordExtensions(string letterString, int wordLength, int extensionLength)
        {
            float bestScore = -99;
            List<string> bestExtensions = new List<string>();

            List<string> testExtensions = new List<string>();
            int initialLength = wordLength - extensionLength;
            for (int i = 0; i < 1000; i++)
            {
                string progString = letterString;
                testExtensions.Clear();
                for (int j = 0; j < 3; j++)
                {
                    
                    string ext = (GetRandomWordExtension(progString,initialLength, extensionLength));
                    if (i > 800 && Random.Range(0f, 1f) > .4f) ext = GenerateRandomWordStart(extensionLength);
                    
                    if (ext.Length > 0)
                    {
                        testExtensions.Add(ext);
                        progString += ext;
                    }
                }
                
                if (testExtensions.Count < 3) continue; //If we didn't get enough extensions, skip this iteration
                float score = EvalueWordExtensions(letterString, wordLength, testExtensions);
                
                if (score > bestScore)
                {
                    bestScore = score;
                    bestExtensions.Clear();
                    bestExtensions.AddRange(testExtensions);
                }
            }
            
            if (bestExtensions.Count < 3)
            {
                Debug.LogWarning("Not enough extensions found for " + letterString + " with word length " + wordLength);
                return new List<string>();
            }
            //Debug.Log("Best extensions for " + letterString + " with word length " + wordLength + ": " + string.Join(", ", bestExtensions) + " with score: " + bestScore);
            return bestExtensions;
        }

        private float EvalueWordExtensions(string letterString, int wordLength, List<string> extensions)
        {
            if (extensions[0] == extensions[2] || extensions[1] == extensions[2] || extensions[0] == extensions[1])
            {
                return 0;
            }
            
            
            int score = 0;
            int solution1 = EvaluateSolution(letterString, wordLength, extensions, 0, 1, 2);
            int solution2 = EvaluateSolution(letterString, wordLength, extensions, 0, 2, 1);
            int solution3 = EvaluateSolution(letterString, wordLength, extensions, 1, 0, 2);
            int solution4 = EvaluateSolution(letterString, wordLength, extensions, 1, 2, 0);
            int solution5 = EvaluateSolution(letterString, wordLength, extensions, 2, 0, 1);
            int solution6 = EvaluateSolution(letterString, wordLength, extensions, 2, 1, 0);

            int totalGoodPaths = 0;
            if (solution1 != 0) totalGoodPaths++;
            if (solution2 != 0) totalGoodPaths++;
            if (solution3 != 0) totalGoodPaths++;
            if (solution4 != 0) totalGoodPaths++;
            if (solution5 != 0) totalGoodPaths++;
            if (solution6 != 0) totalGoodPaths++;
            
            int totalScore = solution1 + solution2 + solution3 + solution4 + solution5 + solution6;

            if (totalGoodPaths >= 5) totalScore *= 3;
            else if (totalGoodPaths >= 3) totalScore *= 2;


            int lettersScore = 0;
            foreach (var ext in extensions)
            {
                string extUp = ext.ToUpper();
                foreach (var extChar in extUp)
                {
                    lettersScore += GetScrabbleWorth(extChar)^2;
                }
            }

            float mult = 1f;
            if (letterString.Contains(extensions[0])) mult *= .3f;
            if (letterString.Contains(extensions[1])) mult *= .3f;
            if (letterString.Contains(extensions[2])) mult *= .3f;
            foreach (var extension in extensions)
            {
                foreach (var extChar in extension)
                {
                    if (letterString.Contains(extChar) == false) mult *= 2;
                }
            }
            return totalScore;

        }

        private int EvaluateSolution(string baseLetterString, int wordLength, List<string> extensions, int order1, int order2, int order3)
        {
            int wordsFound = 0;
            string fullWord = baseLetterString + extensions[order1];
            string subWord1 = fullWord.Substring(fullWord.Length - wordLength);
            if (IsWord(subWord1))
            {
                wordsFound++;
            }
            fullWord += extensions[order2];
            string subWord2 = fullWord.Substring(fullWord.Length - wordLength);
            if (IsWord(subWord2))
            {
                wordsFound++;
            }
            fullWord += extensions[order3];
            string subWord3 = fullWord.Substring(fullWord.Length - wordLength);
            if (IsWord(subWord3))
            {
                wordsFound++;
            }

            if (wordsFound == 3) return 2000;
            else if (wordsFound == 2) return 20;
            else if (wordsFound == 1) return 5;
            else return 0;
        }

        public string GetRandomWordExtension(string letters, int initialLength, int extensionLength)
        {
            string startLetters = letters.Substring(letters.Length - initialLength);
            var checkedNode = GetEndNodeAt(startLetters);
            if (checkedNode == null)
            {
                return GenerateRandomWordStart(extensionLength);
            }
            StringBuilder sb = new StringBuilder();
            int count = 0;
            //Pick a random child if possible extensionLength times
            while (count < extensionLength)
            {
                int randomIndex = Random.Range(0, 26);
                int attempts = 100;
                //If the random index is not available, try again
                while (checkedNode.Children[randomIndex] == null && attempts > 0)
                {
                    randomIndex = Random.Range(0, 26);
                    attempts--;
                }
                
                if (attempts <= 0) break; //If no child is available, break
                
                
                sb.Append((char) (randomIndex + 'A'));
                checkedNode = checkedNode.Children[randomIndex];
                count++;
            }
            
            if (checkedNode == null || !checkedNode.EndOfPath)
            {
                return "";
            }
            
            return sb.ToString();
        }
        
        public string GenerateRandomWordStart(int length)
        {
            if (length == 1)
            {
                int randomIndex = Random.Range(0, 26);
                return ((char)(randomIndex + 'A')).ToString();
            }
            
            int bestCount = 0;
            string bestWord = "SPRAIN".Substring(0, length);

            for (int i = 0; i < 1000; i++)
            {
                
                //Starting from root, pick a random two letters.
                StringBuilder sb = new StringBuilder();
                PrefixTrieNode currentNode = _root;
                for (int j = 0; j < length; j++)
                {
                    int randomIndex = Random.Range(0, 26);
                    while (currentNode.Children[randomIndex] == null)
                    {
                        randomIndex = (randomIndex + 1) % 26;
                    }
                
                    sb.Append((char) (randomIndex + 'A'));
                    currentNode = currentNode.Children[randomIndex];
                }

                int c = currentNode.TotalWords;
                if (c > bestCount)
                {
                    bestWord = sb.ToString();
                    bestCount = c;

                    if (bestCount > 20) return bestWord;
                }
            }


            return bestWord;
        }
        public static int GetScrabbleWorth(char letter)
        {
            return letter switch
            {
                'A' => 1,
                'B' => 3,
                'C' => 3,
                'D' => 2,
                'E' => 1,
                'F' => 4,
                'G' => 3,
                'H' => 4,
                'I' => 1,
                'J' => 8,
                'K' => 5,
                'L' => 1,
                'M' => 3,
                'N' => 1,
                'O' => 1,
                'P' => 3,
                'Q' => 12,
                'R' => 1,
                'S' => 1,
                'T' => 1,
                'U' => 2,
                'V' => 4,
                'W' => 4,
                'X' => 8,
                'Y' => 4,
                'Z' => 10,
                '?' => 0,
                _ => 1
            };
        }
    }
    

}
