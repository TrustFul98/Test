using System.Runtime.Serialization;

namespace Wordle.Service
{
    [Serializable]
    internal class GameNotFoundException : Exception
    {
        public GameNotFoundException() : base($"Game not found!")
        {
        }
    }
}