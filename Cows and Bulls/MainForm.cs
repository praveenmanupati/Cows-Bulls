using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetSpell;

namespace Cows_and_Bulls
{
    public partial class MainForm : Form
    {
        public int MaxChances = 10;
        public string EnterText;
        public string OriginalWord;

        public NetSpell.SpellChecker.Spelling spelCheck;

        WordsArray Iterations;
        BullsArray Bulls;
        CowsArray Cows;
        ChancesArray Chances;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Iterations = new WordsArray(this); 
            
            Bulls = new BullsArray(this); 

            Cows = new CowsArray(this); 

            Chances = new ChancesArray(this); 

            NetSpell.SpellChecker.Dictionary.WordDictionary oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();

            oDict.DictionaryFile = "en-US.dic";

            oDict.Initialize();

            spelCheck = new NetSpell.SpellChecker.Spelling();

            spelCheck.Dictionary = oDict;

            OriginalWord = GenerateWord();
        }

        private void Check_Click(object sender, EventArgs e)
        {
            EnterText = textBox1.Text.Trim().ToLower();

            if (!isValid(EnterText))
            {
                MessageBox.Show("Enter Valid/Correct Word of only 4 letters with no repitition of letters", "Word Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!spelCheck.TestWord(EnterText))
            {
                MessageBox.Show("Word doesn't exists... Please Check the spelling", "Spelling Mistake", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int bulls, cows;
            Iterations.AddRow(); Bulls.AddRow(); Cows.AddRow(); Chances.AddRow();
            Iterations[Iterations.Count - 1 - 1].Text = EnterText.ToLower();
            CheckWord(OriginalWord, EnterText, out bulls, out cows);
            Bulls[Bulls.Count - 1 - 1].Text = bulls.ToString();
            Cows[Cows.Count - 1 - 1].Text = cows.ToString();
            Chances[Chances.Count - 1 - 1].Text = (MaxChances - Iterations.Count + 1).ToString();
            textBox1.Text = "";

            if (bulls == 4) MessageBox.Show("Correct Word predicted. Success!!", "Game Over");

            if (Iterations.Count == MaxChances + 1 && bulls != 4)
            {
                MessageBox.Show("The real word is: " + OriginalWord, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult result = MessageBox.Show("All chances are used. Do you want to Try again?", "Game Over", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Retry)
                {
                    DisposeControls();
                    MainForm_Load(sender, e);
                }
            }
        }

        private bool isValid(string enteredText)
        {
            bool valid = false;
            enteredText = enteredText.Replace(" ", "");
            if (enteredText.Length == 4 && !HasRepetitions(enteredText))
            {
                valid = true;
            }

            return valid;
        }

        private bool HasRepetitions(string toCheck)
        {
            bool RepititionExists = false;
            var result = new Dictionary<char, int>();
            foreach (var chr in toCheck)
            {
                if (result.ContainsKey(chr))
                {
                    result[chr]++;
                    continue;
                }
                result.Add(chr, 1);
            }

            foreach (var item in result)
            {
                if (item.Value != 1)
                {
                    RepititionExists = true;
                    break;
                }
            }

            return RepititionExists;
        }

        private string RandomString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            //Random random = new Random();
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private string GenerateWord()
        {
            string word = "";
            do
            {             
                word = RandomString(4);
                if (HasRepetitions(word))
                {
                    word = "";
                }
                else
                {
                    bool correct = spelCheck.TestWord(word);
                    if (!correct)
                    {
                        word = "";
                    }
                }              

            } while (word == "");

            return word;
        }

        private void CheckWord(string OriginalWord, string TestWord, out int bulls, out int cows)
        {
            bulls = 0;
            cows = 0;
            OriginalWord = OriginalWord.ToLower();
            TestWord = TestWord.ToLower();

            foreach (var chr in OriginalWord)
            {
                if (TestWord.Contains(chr))
                {
                    if (OriginalWord.IndexOf(chr) == TestWord.IndexOf(chr)) bulls++;
                    else cows++;
                }
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            ActiveForm.AcceptButton = Check;
        }

        private void DisposeControls()
        {
            try
            {
                List<Control> itemsToRemove = new List<Control>();
                foreach (Control ctr in this.Controls)
                {                   

                    if (ctr.Tag != null && ctr.Tag.ToString().Contains("dynamic"))
                        itemsToRemove.Add(ctr);
                }

                foreach (Control ctr in itemsToRemove)
                {
                    Controls.Remove(ctr);
                    ctr.Dispose();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
