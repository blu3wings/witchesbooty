using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Booty.Tools
{
    public class Randomiser : MonoBehaviour
    {
        /// <summary>
        /// Use this to get one single random number.
        /// </summary>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="onCallback"></param>
        public void GetSingleNumber(int top,int bottom,Action<int> onCallback)
        {
            int selectedNumber = UnityEngine.Random.Range(top, bottom);

            if (onCallback != null)
                onCallback(selectedNumber);
        }

        /// <summary>
        /// Use this to get more than 1 random number with multiple range.
        /// </summary>
        /// <param name="numberRange"></param>
        /// <param name="onCallback"></param>
        public void GetMultipleNumber(Dictionary<int,int[]> numberRange,
            Action<Dictionary<int,int>> onCallback)
        {
            Dictionary<int, int> selectedNumbers = new Dictionary<int, int>();

            foreach(KeyValuePair<int,int[]> kvp in numberRange)
            {
                int selectedNumber = UnityEngine.Random.Range(kvp.Value[0], kvp.Value[1]);
                selectedNumbers.Add(kvp.Key, selectedNumber);
            }

            if (onCallback != null)
                onCallback(selectedNumbers);
        }
    }
}