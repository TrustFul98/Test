using System.Runtime.Serialization;

namespace Wordle.Service
{
    [Serializable]
    internal class GuessWordNotValidException : Exception
    {
        public GuessWordNotValidException() : base($"GuessWord ist not valid!")
        {
        }
    }
}