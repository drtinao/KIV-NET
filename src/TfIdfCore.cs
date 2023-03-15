using System;
using System.Collections.Generic;
using System.Linq;

namespace DrSearch
{
    /// <summary>
    /// Class which is used for calculating tf and idf scores => tf-idf.
    /// </summary>
    class TfIdfCore
    {
        /// <summary>
        /// Used for retrieving TF score.
        /// </summary>
        /// <param name="wordOccuranceInDoc">Dictionary which represent content present in the one doc (key: word, value: number of occurance of the given word in the doc)</param>
        /// <param name="word">term which is explored</param>
        /// <returns>TF score</returns>
        public double retrieveTf(Dictionary<String, Int32> wordOccuranceInDoc, String word)
        {
            if (!wordOccuranceInDoc.ContainsKey(word.ToLower()))
            { //word not present at all in given doc
                return 0;
            }
            else
            { //word is present atleast once, determine actual number
                return 1 + Math.Log10(wordOccuranceInDoc[word.ToLower()]);
            }
        }

        /// <summary>
        /// Used for retrieving IDF score.
        /// </summary>
        /// <param name="validDocumentsNums">key: word present in atleast one document; value: index 0 = total occ num across docs, index >= 1 = document number, which countains the given word</param>
        /// <param name="word">term which is explored</param>
        /// <param name="totalDocsSize">total nuber of indexed docs</param>
        /// <returns>IDF score</returns>
        public double retrieveIdf(Dictionary<String, List<Int32>> validDocumentsNums, String word, int totalDocsSize)
        {
            int count = 0;

            if (validDocumentsNums.ContainsKey(word))
            {
                count = validDocumentsNums[word].Count - 1; //first is total num of occ, we dont need that - interested in number of docs which contains the word at least once..
            }

            if (count == 0)
            {
                return 0;
            }
            else
            {
                return Math.Log10((double)totalDocsSize / (double)count);
            }
        }

        /// <summary>
        /// Used for combining TF and IDF scores, results in TF-IDF.
        /// </summary>
        /// <param name="docNum">number of the document in wordOccuranceInDocs collection</param>
        /// <param name="validDocumentsNums">key: word present in atleast one document; value: index 0 = total occ num across docs, index >= 1 = document number, which countains the given word</param>
        /// <param name="individualDocsCounts">represents content of indiv. docs - key: word present in the doc, value: number of occ of the word in the doc</param>
        /// <param name="word">term which is explored</param>
        /// <returns>calculated tf-idf value</returns>
        public double retrieveTfIdf(int docNum, Dictionary<String, List<Int32>> validDocumentsNums, List<Dictionary<String, Int32>> individualDocsCounts, String word)
        {
            double tfRes = retrieveTf(individualDocsCounts.ElementAt(docNum), word);
            double idfRes = retrieveIdf(validDocumentsNums, word, individualDocsCounts.Count);
            double tfIdfRes = tfRes * idfRes;
            return tfIdfRes;
        }
    }
}
