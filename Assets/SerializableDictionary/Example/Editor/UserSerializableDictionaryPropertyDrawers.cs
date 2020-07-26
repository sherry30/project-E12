using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DictionaryTech))]
[CustomPropertyDrawer(typeof(DictionaryRaw))]
[CustomPropertyDrawer(typeof(DictionaryEnergy))]
[CustomPropertyDrawer(typeof(DictionaryRes))]
[CustomPropertyDrawer(typeof(StringStringDictionary))]
[CustomPropertyDrawer(typeof(ObjectColorDictionary))]
[CustomPropertyDrawer(typeof(StringColorArrayDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ColorArrayStorage))]
public class AnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
