namespace TextCorvid
{
    public static class Utility
    {
        /// <summary>
        /// Sets the array to a new size and reads as many of the old items into the new array as possible.
        /// </summary>
        /// <typeparam name="T">The type of Array you wish to resize</typeparam>
        /// <param name="_oldArray">The old Array you wish to resize</param>
        /// <param name="_newSize">The size for the new Array</param>
        /// <returns>A new Array with as many of the old items in it as was allowed.</returns>
        public static T[] ResizeArray<T>(T[] _oldArray, int _newSize)
        {
            T[] _newArray = new T[_newSize];
            for (int i = 0; i < _newSize; i++)
            {
                if (i > _oldArray.Length - 1)
                    return _newArray;
                _newArray[i] = _oldArray[i];
            }
            return _newArray;
        }
    }

}