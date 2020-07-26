using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DictionaryTech: SerializableDictionary<TechCode,TechSkill>{}
[System.Serializable] 
public class TechTree
{  
    public DictionaryTech techSkills;

    
}
