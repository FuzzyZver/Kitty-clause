using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "CatsCharConfig", menuName = "Configs/CatsCharConfig")]
public class CatsCharConfig : ScriptableObject
{
    public int CatsCount;
    public List<string> CatsNames;
    public List<Sprite> CatsSprites;
    public List<Sprite> GiftsSprites;

    public List<LetterSprites> LettersSpites;
}

[System.Serializable]
public class LetterSprites
{
    public Sprite CloseEnvelope;
    public Sprite OpenEnvelope;
    public Sprite Letter;
}
