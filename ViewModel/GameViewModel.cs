using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wordle.Model;

//Manages game logic, user input, and interactions
namespace Wordle.ViewModel
{
    public partial class GameViewModel : ObservableObject
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string FilePath => System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "words.txt");
        private HttpClient httpClient;
        private List<string> ListofWords;
        public List<string> Words => ListofWords;
        private List<string> words = new List<string>();
        private bool isBusy;
        private bool isValidInput;
        public char[] correctAnswer;
        private Random random = new Random();
        private string SaveFilePath => System.IO.Path.Combine(FileSystem.Current.AppDataDirectory, "savefile.txt");
        private string chosenWord { get; set; }

        // 0 - 5 
        int rowIndex;

        // 0 - 4
        int columnIndex;

        public char[] KeyboardRow1 { get; }
        public char[] KeyboardRow2 { get; }
        public char[] KeyboardRow3 { get; }

        [ObservableProperty]
        private Row[] rows;
        private bool validInput;

        public GameViewModel()
        {
            //constructor initialise httpClient & list for words, checks if file exists.
            httpClient = new HttpClient();
            ListofWords = new List<string>();

            if (File.Exists(FilePath))
            {
                ReadFromFile();
            }
            GetWordsFromVM();

            chosenWord = PickWordFromList();
            correctAnswer = chosenWord.ToCharArray();

            rows = new Row[6]
            {
            new Row(),
            new Row(),
            new Row(),
            new Row(),
            new Row(),
            new Row()
            };


            KeyboardRow1 = "ABCDEFGHIJ".ToCharArray();
            KeyboardRow2 = "KLMNOPQRST".ToCharArray();
            KeyboardRow3 = "<UVWXYZ>".ToCharArray();

        }

        private async Task ReadFromFile()
        {
            try
            {
                ListofWords = File.ReadAllLines(FilePath).ToList();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error reading file", ex.Message, "OK");
            }
        }

        private async Task GetWords()
        {
            //clears any existing list, retrieves data from URL, splits words into array, assigns array to list, saves the words to a file.
            ListofWords.Clear();
            var response = await httpClient.GetAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
            string content = await response.Content.ReadAsStringAsync();
            string[] individualWords = content.Split(new[] { '\n' });
            ListofWords.AddRange(individualWords);

            //write to file
            SaveWordsFile(content);

            chosenWord = PickWord();

        }
        public string PickWord()
        {
            /*
                Picks a word from the list based on a random number & returns it.
            */
            if (words.Count > 0)
            {
                int randomNumber = random.Next(0, words.Count);
                return words[randomNumber];
            }//if list is populated / not empty
            else
            {
                return null;
            }//else list is empty
        }
        private string PickWordFromList()
        {
            if (ListofWords.Count > 0)
            {
                int randomNumber = random.Next(0, ListofWords.Count);
                return ListofWords[randomNumber];
            }
            else
            {
                return null;
            }
        }

        private async Task SaveWordsFile(string data)
        {
            //saves words to a file.
            try
            {
                File.WriteAllText(FilePath, data);
            }//try
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error writing file", ex.Message, "OK");

            }//catch
        }
        public async Task MakeList()
        {
            //if not busy, begins making list from file.
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                if (File.Exists(FilePath))
                    ReadFromFile();
                else
                    await GetWords();
            }//try
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error making list", ex.Message, "OK");
            }//catch
            finally
            {
                IsBusy = false;
            }//finally
        }

        public async Task GetWordsFromVM()
        {
            //function to be used in main page
            await MakeList();
        }//GetWordsFromVM

        public bool IsBusy
        {
            //assigns / gets IsBusy
            get => isBusy;
            set
            {
                if (isBusy == value)
                    return;
                isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotBusy));
            }
        }
        public bool IsNotBusy => !IsBusy;

        public void Enter()
        {

            if (columnIndex != 5)
                return;

            var userGuess = new string(Rows[rowIndex].Letters.Select(letter => letter.Input).ToArray());

            ValidWord(userGuess);

            var correct = Rows[rowIndex].Validate(chosenWord);

            if (correct)
            {

                new StatsViewModel().IncrementGamesWon();
                new StatsViewModel().IncrementGamesPlayed();
                new StatsViewModel().UpdateAverageAttempt(rowIndex + 1);
                App.Current.MainPage.DisplayAlert("You Win!", "You are so smart!", "OK");
                return;
            }

            if (rowIndex == 5)
            {
                new StatsViewModel().IncrementGamesPlayed();
                App.Current.MainPage.DisplayAlert("Game Over!", "You are out of turns. The answer is " + chosenWord, "OK");
            }
            else
            {
                rowIndex++;
                columnIndex = 0;
            }
        }

        private void ValidWord(string userGuess)
        {
            validInput = ListofWords.Contains(userGuess.ToLower());
            if (!validInput)
            {
                App.Current.MainPage.DisplayAlert("Invalid Word", "Please enter a valid word.", "OK");
            }
        }

        [RelayCommand]
        public void EnterLetter(char letter)
        {
            if (letter == '>')
            {
                Enter();
                return;
            }

            if (letter == '<')
            {
                if (columnIndex == 0)
                    return;
                columnIndex--;
                Rows[rowIndex].Letters[columnIndex].Input = ' ';

                return;
            }

            if (columnIndex == 5)
                return;

            Rows[rowIndex].Letters[columnIndex].Input = letter;
            columnIndex++;
        }

        public void ResetGame()
        {
            // Reset game-related properties
            rowIndex = 0;
            columnIndex = 0;
            chosenWord = PickWordFromList();
            correctAnswer = chosenWord.ToCharArray();
            validInput = false;

            // Clear user inputs
            foreach (var row in rows)
            {
                foreach (var letter in row.Letters)
                {
                    letter.Input = ' ';
                    letter.Color = Colors.Black;
                }
            }

            OnPropertyChanged(nameof(Rows)); // Notify that Rows have changed
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}