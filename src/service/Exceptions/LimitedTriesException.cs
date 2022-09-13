using System.Runtime.Serialization;

namespace Wordle.Service
{
    [Serializable]
    internal class LimitedTriesException : Exception
    {
        public LimitedTriesException() : base($"Amount of Tries reached maximum.")
        {
        }
    }
}