using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using Wordle.ViewModel;

namespace Wordle.Model
{
    public class Row
    {
        // Enumeration representing the possible states of a Letter
        enum State
        {
            Gray = 0, // Initial state
            Yellow = 1, // Indicates a correct letter in the wrong position
            Green = 2 // Indicates a correct letter in the correct position
        }

        // Constructor initializes an array of five Letter objects
        public Row()
        {

            Letters = new Letter[5];

            for (int i = 0; i < Letters.Length; i++)
            {
                Letters[i] = new Letter();
            }
        }

        // Array property representing the five Letter objects in the row
        public Letter[] Letters { get; set; }

        public bool Validate(string correctAnswer)
        {
            // Convert the string to a char array
            char[] correctAnswerArray = correctAnswer.ToUpper().ToCharArray();

            int count = 0;

            // loops through stuff.
            for (int i = 0; i < Letters.Length; i++)
            {
                var letter = Letters[i];
                char inputCharacter = char.ToUpper(letter.Input);

                if (inputCharacter == correctAnswerArray[i])
                {
                    letter.Color = Colors.Green;
                    count++;
                }
                else if (correctAnswerArray.Contains(inputCharacter))
                {
                    letter.Color = Colors.Yellow;
                }
                else
                {
                    letter.Color = Colors.Gray;
                }
            }

            return count == 5;
        }
    }
    public partial class Letter : ObservableObject
    {
        public Letter()
        {
            Color = Colors.Black;
            Input = '\0';
        }

        [ObservableProperty]
        private char input;

        [ObservableProperty]
        private Color color;
    }
}