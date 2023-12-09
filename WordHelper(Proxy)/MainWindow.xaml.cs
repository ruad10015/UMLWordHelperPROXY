using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WordHelper_Proxy_
{
    public partial class MainWindow : Window
    {
        private Word _word;

        public MainWindow()
        {
            InitializeComponent();

            _word = new Word();

            listBox.Focus();
        }

        private void ReloadListBox()
        {
            string enteredWord = textBox.Text;

            List<string> similarWords = _word.GetSimilarWords(enteredWord);

            listBox.ItemsSource = similarWords;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedWord = listBox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedWord))
            {
                textBlock.Text = textBlock.Text + " " + selectedWord;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReloadListBox();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredWord = textBox.Text;

            _word.AddWord(enteredWord);

            ReloadListBox();
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                string selectedWord = listBox.SelectedItem as string;
                if (!string.IsNullOrEmpty(selectedWord))
                {
                    textBlock.Text = textBlock.Text + " " + selectedWord;
                }
            }
        }
    }

    public class WordHelper
    {
        private List<string> _words;

        public WordHelper()
        {
            var dir = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(dir);
            var currentDir = directoryInfo.Parent.Parent.Parent;
            var path = currentDir.FullName;
            _words = File.ReadAllLines(path+"/Words.txt").ToList();
        }

        public List<string> GetSimilarWords(string enteredWord)
        {
            List<string> similarWords = _words
                .Where(w => w.StartsWith(enteredWord, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return similarWords;
        }

        public void AddWord(string word)
        {
            _words.Add(word);
        }
    }

    public class Word
    {
        private WordHelper _realWord;

        public List<string> GetSimilarWords(string enteredWord)
        {
            if (_realWord == null)
            {   
                _realWord = new WordHelper();
            }

            List<string> similarWords = _realWord.GetSimilarWords(enteredWord);

            return similarWords;
        }

        public void AddWord(string word)
        {
            if (_realWord == null)
            {   
                _realWord = new WordHelper();
            }

            _realWord.AddWord(word);
        }
    }
}
