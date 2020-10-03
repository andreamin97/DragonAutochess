using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Board")]
public class Board : ScriptableObject
{
    [Serializable]public struct BoardTile
    {
        public GameObject tile;
        public GameObject unit;
    }
}
