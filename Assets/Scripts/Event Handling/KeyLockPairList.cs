using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "KeyLockPairList", menuName = "KeyLockPairList")]
public class KeyLockPairList : ScriptableObject
{
    public List<KeyLockPair> keyLockPairs;
}
