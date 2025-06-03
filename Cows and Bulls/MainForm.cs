using System;
using System.Collections.Generic;
using System.IO; // Required for FileNotFoundException
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetSpell;

namespace Cows_and_Bulls
{
    public partial class MainForm : Form
    {
        private Random randomGenerator = new Random();
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

            try
            {
                NetSpell.SpellChecker.Dictionary.WordDictionary oDict = new NetSpell.SpellChecker.Dictionary.WordDictionary();
                oDict.DictionaryFile = "en-US.dic";
                oDict.Initialize(); // This can throw FileNotFoundException or other exceptions

                spelCheck = new NetSpell.SpellChecker.Spelling();
                spelCheck.Dictionary = oDict;

                OriginalWord = GenerateWord(); // This should only be called if dictionary loaded successfully
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show($"Error: The dictionary file 'en-US.dic' could not be found. Please ensure it is in the application directory.\nDetails: {ex.Message}", "Dictionary Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Close the application
            }
            catch (Exception ex) // Catch any other exceptions during dictionary loading
            {
                MessageBox.Show($"An unexpected error occurred while loading the dictionary 'en-US.dic'.\nDetails: {ex.Message}", "Dictionary Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close(); // Close the application
            }
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
            Iterations[Iterations.Count - 1].Text = EnterText.ToLower();
            CheckWord(OriginalWord, EnterText, out bulls, out cows);
            Bulls[Bulls.Count - 1].Text = bulls.ToString();
            Cows[Cows.Count - 1].Text = cows.ToString();
            Chances[Chances.Count - 1].Text = (MaxChances - Iterations.Count + 1).ToString();
            textBox1.Text = "";

            if (bulls == 4)
            {
                MessageBox.Show("Correct Word predicted. Success!!", "Game Over");
                DialogResult playAgainResult = MessageBox.Show("Do you want to play again?", "Play Again?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (playAgainResult == DialogResult.Yes)
                {
                    ResetGame();
                }
                else
                {
                    this.Close(); // Or disable further input
                }
                return; // Important to return so the loss condition isn't also checked
            }

            if (Iterations.Count == MaxChances + 1 && bulls != 4)
            {
                MessageBox.Show("The real word is: " + OriginalWord, "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult result = MessageBox.Show("All chances are used. Do you want to Try again?", "Game Over", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                if (result == DialogResult.Retry)
                {
                    ResetGame();
                }
                else
                {
                    this.Close(); // Or disable further input
                }
            }
        }

        private void ResetGame()
        {
            DisposeControls();

            // Re-initialize arrays
            Iterations = new WordsArray(this);
            Bulls = new BullsArray(this);
            Cows = new CowsArray(this);
            Chances = new ChancesArray(this);

            // Generate a new word
            OriginalWord = GenerateWord();

            // Clear input
            textBox1.Text = "";

            // Optional: Reset any other game-specific state.
            // For this game, the Chances label is part of the ChancesArray which gets re-initialized.
            // If there were a separate counter label for chances remaining, it would be reset here.
            // e.g., lblChancesRemaining.Text = MaxChances.ToString();
            // However, the current structure with ChancesArray handles this implicitly by adding a new row
            // which will display the initial chance count.
            // We need to ensure the first row of ChancesArray displays the correct initial value.
            // The WordsArray, BullsArray, CowsArray constructors add an initial row.
            // The ChancesArray constructor also adds an initial row.
            // Let's check if the text for that initial row needs to be explicitly set here.
            // Chances[0].Text should be MaxChances.
            // The AddRow in constructor of ChancesArray sets Text = (owner.MaxChances - owner.Iterations.Count + 1).ToString();
            // At this point of ResetGame, Iterations.Count would be 1 (due to its own re-initialization and AddRow).
            // So, Chances[0].Text will be (MaxChances - 1 + 1) = MaxChances. This seems correct.
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
            // Use the class-level randomGenerator instance
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * randomGenerator.NextDouble() + 65)));
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
