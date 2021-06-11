using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Default
{
    public static class JsonPersonal
    {
        public static CustomContractResolver ContractResolver = new CustomContractResolver();
        public class CustomContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);

                if (IsUnityType(member.DeclaringType))
                {
                    if (member.MemberType == MemberTypes.Property)
                    {
                        var property = member as PropertyInfo;

                        if (HasGetterAndSetter(property) && IsUnityProperty(property))
                        {

                        }
                        else
                        {
                            jsonProperty.ShouldSerialize = NO;
                        }
                    }
                }
                else
                {
                    var defaults = member.DeclaringType.GetCustomAttribute<JsonPersonalDefaultsAttribute>();

                    if(defaults != null)
                    {
                        if (member.MemberType == MemberTypes.Property)
                        {
                            var property = member as PropertyInfo;

                            if (HasGetterAndSetter(property))
                            {

                            }
                            else
                            {
                                jsonProperty.ShouldSerialize = NO;
                            }
                        }
                    }
                }

                return jsonProperty;
            }

            protected static bool IsUnityType(Type type)
            {
                if (type.Namespace.ToLower().Contains("unityengine"))
                    return true;

                if (type.Namespace.ToLower().Contains("unityeditor"))
                    return true;

                return false;
            }

            public static bool HasGetterAndSetter(PropertyInfo property)
            {
                return property.GetSetMethod() != null && property.GetGetMethod() != null;
            }

            public static bool IsUnityProperty(PropertyInfo property)
            {
                var fields = property.DeclaringType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                for (int i = 0; i < fields.Length; i++)
                    if (fields[i].Name.ToLower().Contains(property.Name.ToLower()))
                        return true;

                return false;
            }

            protected static bool NO(object obj)
            {
                return false;
            }
        }

        public static JsonSerializerSettings Settings { get; private set; } = new JsonSerializerSettings()
        {
            ContractResolver = JsonPersonal.ContractResolver
        };

        public static JsonSerializer Serializer { get; private set; } = JsonSerializer.Create(Settings);

        public static void TestSeriazation<T>(T value)
        {
            bool passed = false;

            var log = "";
            log += Environment.NewLine + "Value:" + value;

            var json = JsonConvert.SerializeObject(value, Formatting.Indented, JsonPersonal.Settings);
            log += Environment.NewLine + "Json:" + json;

            var constructedValue = JsonConvert.DeserializeObject<T>(json);
            log += Environment.NewLine + "Constructed Value:" + constructedValue;

            if (typeof(T).IsValueType)
            {
                if (constructedValue.Equals(value)) passed = true;
            }
            else
            {
                var constructedJson = JsonConvert.SerializeObject(constructedValue, Formatting.Indented, JsonPersonal.Settings);

                log += Environment.NewLine + "Constructed Json:" + constructedJson;

                if (constructedJson == json) passed = true;
            }

            if (passed)
            {
                Debug.Log("Success Serializing " + typeof(T).Name + log);
            }
            else
                Debug.LogError("Failed To Serialize + " + typeof(T).Name + log);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = false)]
    sealed class JsonPersonalDefaultsAttribute : Attribute
    {

    }
}