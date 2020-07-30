using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DictionaryTechCodeTechSkill))]
[CustomPropertyDrawer(typeof(DictionaryRawInt))]
[CustomPropertyDrawer(typeof(DictionaryEnergyInt))]
[CustomPropertyDrawer(typeof(DictionaryResInt))]
[CustomPropertyDrawer(typeof(DictionaryOtherResInt))]
[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
