using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
/* содержит классы, определяющие сведения, относящиеся к культуре, такие как 
язык, название страны, а также порядок сортировки строк */
using System.Text;
using System.Windows.Forms;

namespace Lab1
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            List<string> ControlCharacters = new List<string> { "\0", "\a", "\b", "\f", "\n", "\r", "\t", "\v", "\\", "\'", /*"\"",*/ "(", ")", "{", "}" };

            try
            {
                string text = (string)Clipboard.GetDataObject().GetData(DataFormats.UnicodeText);

                if (text == null)
                {
                    throw new NullReferenceException("Clipboard is empty");
                }
                if (text == "")
                {
                    throw new Exception ("Clipboard doesn't contain any text!");
                }

                foreach (string character in ControlCharacters)
                {
                    text = text.Replace(character, ".");
                }

                

                var words = text.Split('.', '!', '?', ':', ';', ',', ' ');

                List<string> wordsList = new List<string>();

                for (int i = 0; i < words.Length; i++)
                {
                    wordsList.Add(words[i]);
                }

                wordsList.RemoveAll(Predicate);

                wordsList = new List<string>(wordsList.Distinct());

                wordsList.Sort(StringComparer.CurrentCultureIgnoreCase); // сортировка строк без учёта регистра с учётом языка

                wordsList.Take(1000);

                for (int j = 0; j < wordsList.Count - 1; j++)
                {

                    int lastIndex = wordsList[j].Length - 1;

                    if (wordsList[j][0] == '\"')
                    {
                        if (!(wordsList[j].Contains("\"-"))) // для случая, когда слово пишется через дефис и первая часть взята в скобки
                            wordsList[j] = wordsList[j].Remove(0,1);
                    }

                    lastIndex = wordsList[j].Length - 1;

                    if (wordsList[j][lastIndex] == '\"')
                    {
                        if (!(wordsList[j].Contains("-\""))) // для случая, когда слово пишется через дефис и вторая часть взята в скобки
                            wordsList[j] = wordsList[j].Remove(lastIndex);
                    }

                }


                    foreach (string word in wordsList)
                        Console.WriteLine(word);

                Console.ReadLine();

            }

            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                Console.ReadLine();
            }

        }

        /// <summary>
        /// задаёт условие для удаления элементов из списка wordsList метода Main
        /// </summary>
        /// <param name="s">строка, которая проверяется</param>
        /// <returns></returns>
        private static bool Predicate(string s)
        {
            return s == "" || s == "-" || s=="\"";
        }
    }
}
