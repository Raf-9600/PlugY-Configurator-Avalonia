using AvaloniaLocalizationExample.Localizer;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Xml.Linq;


[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields)]
class AppSettings
{
    private static Dictionary<string, string> appSettings = new();
    private static byte[] appSettingsFirst = [];
    private static IEnumerable<MemberInfo>? mmbrs;


    public static bool TryReadAppSettings(string settingsJson)
    {
        if (File.Exists(settingsJson))
        
            try
            {
                appSettingsFirst = File.ReadAllBytes(settingsJson);
                //appSettings = JsonSerializer.Deserialize<Dictionary<string, string>>(appSettingsFirst);

                var appSettings_tmp = JsonSerializer.Deserialize(appSettingsFirst, typeof(Dictionary<string, string>), DictionaryTypeInfoResolver.Default) as Dictionary<string, string>;
                if (appSettings_tmp != null && appSettings_tmp.Count > 0)
                {
                    appSettings = appSettings_tmp;
                    return false;
                }
            }
            catch (Exception) { }
        
        appSettings = new();
        return true;
    }

    public static bool MemberExis(string name)
    {
        string nme;
        if (_attrNames.ContainsKey(name))
            nme = _attrNames[name];
        else
            nme = name;

        return appSettings.ContainsKey(nme);
    }

    //public static List<(string key, object value)> Default = new();
    //private static Dictionary<string, object> _default = new();
    private static void SetDefault(List<(string key, object value)> def)
    {
        foreach (var item in def)
        {
            string nme;
            if (_attrNames.ContainsKey(item.key))
                nme = _attrNames[item.key];
            else
                nme = item.key;

            if (!appSettings.ContainsKey(nme))
                Save(nme, item.value);
            //else
                //_default.Add(nme, item.value);
        }

    }

    private static Dictionary<string, string> _attrNames = new();
    private static void GetNamesFromAttrib()
    {
        Type typeAttribute = typeof(AppSettingsAttribute);

        if (mmbrs != null)
            foreach (var mmbr in mmbrs)
            {
                if (Attribute.IsDefined(mmbr, typeAttribute))
                {
                    string? nme = (mmbr.GetCustomAttribute(typeAttribute) as AppSettingsAttribute)?.Name;

                    if (nme != null)
                        _attrNames[mmbr.Name] = nme;
                }
            }
    }


    public static object LoadSettings(object targetClass)
    {
        //if (appSettings != null && appSettings.Count > 0)
        {
            mmbrs = targetClass.GetType()?.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).
                Where(n => n.MemberType == MemberTypes.Field || n.MemberType == MemberTypes.Property);

            GetNamesFromAttrib();
            //SetDefault(Default);


            if (mmbrs != null)
                foreach (var keyValuePair in appSettings)
                {
                    //var field = targetClass.GetType()?.GetField(keyValuePair.Key, BindingFlags.NonPublic | BindingFlags.Instance);
                    //var prop = targetClass.GetType()?.GetProperty(keyValuePair.Key, BindingFlags.NonPublic | BindingFlags.Instance);

                    foreach (MemberInfo m in mmbrs)
                    {
                        _attrNames.TryGetValue(m.Name, out string? nme); // Если не находим имя у атрибута, то присваиваем истинное имя поля или свойства
                        nme ??= m.Name;

                        if (nme == keyValuePair.Key)
                            try
                            {
                                SetValue(m, keyValuePair.Value);
                            }
                            catch (Exception)
                            {
                                /* if (_default.ContainsKey(keyValuePair.Key))
                                    SetValue(m, _default[keyValuePair.Key]); */
                            }
                    }
                }
            appSettings.Clear();



            void SetValue(MemberInfo mbr, object value)
            {
                switch (mbr.MemberType)
                {
                    case MemberTypes.Field:
                        {
                            FieldInfo fld = (FieldInfo)mbr;
                            Type t = Nullable.GetUnderlyingType(fld.FieldType) ?? fld.FieldType;

                            //if (value == null || value.Equals("null"))
                            //fld.SetValue(targetClass, null);
                            //else
                            fld.SetValue(targetClass, Convert.ChangeType(value, t));
                        }
                        break;


                    case MemberTypes.Property:
                        {
                            PropertyInfo prop = (PropertyInfo)mbr;
                            Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                            //if (value == null || value.Equals("null"))
                            //prop.SetValue(targetClass, null);
                            //else
                            prop.SetValue(targetClass, Convert.ChangeType(value, t));
                        }
                        break;
                }
            }

        }
        return targetClass;
    }

    public static void Save(string name, object? value)
    {
        string? result = Convert.ToString(value);

        if (result != null && value != null && result != GetDefaultValue(value.GetType()))
            appSettings[name] = result;
    }

    public static void WriteSettingsFile(string settingsJson, object? obj)
    {
        if (obj != null && mmbrs != null)
        {
            var options = mmbrs.Where(n => Attribute.IsDefined(n, typeof(AppSettingsAttribute)));
            foreach (MemberInfo? member in options)
            {
                _attrNames.TryGetValue(member.Name, out string? nme);
                nme ??= member.Name;

                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        {
                            FieldInfo fld = (FieldInfo)member;
                            var value = Convert.ToString(fld.GetValue(obj));

                            if (value != null && !value.Equals("null"))
                            {
                                Type t = Nullable.GetUnderlyingType(fld.FieldType) ?? fld.FieldType;
                                SetValue(nme, value, t);
                            }
                        }
                        break;

                    case MemberTypes.Property:
                        {
                            PropertyInfo prop = (PropertyInfo)member;
                            var value = Convert.ToString(prop.GetValue(obj));

                            if (value != null && !value.Equals("null"))
                            {
                                Type t = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                                SetValue(nme, value, t);
                            }
                        }
                        break;
                }
            }

            void SetValue(string name, string value, Type t)
            {
                if (!value.Equals(GetDefaultValue(t)))
                    appSettings[name] = value;
            }
        }

        byte[] appSettingsEnd = JsonSerializer.SerializeToUtf8Bytes(appSettings, typeof(Dictionary<string, string>), DictionaryTypeInfoResolver.Default);
        if (!ByteArraysEqual(appSettingsFirst, appSettingsEnd))
        {
            string? dir = Path.GetDirectoryName(settingsJson);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllBytes(settingsJson, appSettingsEnd);

            appSettingsFirst = new byte[appSettingsEnd.Length];
            Array.Copy(appSettingsEnd, appSettingsFirst, appSettingsEnd.Length);
        }

        bool ByteArraysEqual(ReadOnlySpan<byte> a1, ReadOnlySpan<byte> a2)
        {
            return a1.SequenceEqual(a2);
        }
    }


    static string? GetDefaultValue(Type t)
    {
        if (t == typeof(string))
            return string.Empty;
        else
        if (t == typeof(bool))
            return "False";
        else
        if (t == typeof(int) || t == typeof(double))
            return "0";
        else
        //if (t == typeof(bool?) || t == typeof(int?) || t == typeof(double?))
            return "null";
        //else
            //return Convert.ToString(Activator.CreateInstance(t));
        //return Convert.ToString(FormatterServices.GetUninitializedObject(t));
    }

    

}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
public sealed class AppSettingsAttribute : Attribute
{
    public readonly string? Name;
    public AppSettingsAttribute(string name)
    {
        Name = name;
    }

    public AppSettingsAttribute()
    { }
}
