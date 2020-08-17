using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DictionaryTechCodeTechSkill))]
[CustomPropertyDrawer(typeof(DictionaryRawFloat))]
[CustomPropertyDrawer(typeof(DictionaryCrystalFloat))]
[CustomPropertyDrawer(typeof(DictionaryEnergyFloat))]
[CustomPropertyDrawer(typeof(DictionaryResFloat))]
[CustomPropertyDrawer(typeof(DictionaryOtherResFloat))]
[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
