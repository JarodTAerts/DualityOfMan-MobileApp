using System;
using System.Collections.Generic;
using System.Text;

namespace DualityOfMan.Models
{
    public static class DataContainer
    {
        private static List<string> WordData;

        public static string GetRandomWord()
        {
            Random random = new Random();
            int lengthOfWordList = WordData.Count;
            string word = "";

            // Get words from the random word data and make sure the words are longer than 2 characters
            while (word.Length <= 2)
                word = WordData[random.Next(1, lengthOfWordList)];

            return word;
        }
    }
}
