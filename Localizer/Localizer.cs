namespace AvaloniaLocalizationExample.Localizer
{
    using Avalonia;
    using Avalonia.Platform;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Text;

    public class Localizer : INotifyPropertyChanged
    {
        private const string IndexerName = "Item";
        private const string IndexerArrayName = "Item[]";
        private Dictionary<string, string>? m_Strings = null;

        private readonly System.Text.Json.JsonSerializerOptions _jsonOptions = new System.Text.Json.JsonSerializerOptions
        {
            ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public Localizer()
        {

        }

        private string? _nameAssembly = null;
        public bool LoadLanguage(string language)
        {
            Language = language;
            _nameAssembly ??= System.Reflection.Assembly.GetCallingAssembly().GetName().Name;

            Uri uri = new Uri($"avares://{_nameAssembly}/Assets/Translation/{language}.json");
            //if (assets != null && assets.Exists(uri)) 
            {
                using (StreamReader sr = new(AssetLoader.Open(uri), Encoding.UTF8)) 
                {
                    m_Strings = JsonSerializer.Deserialize<Dictionary<string, string>>(sr.ReadToEnd(), _jsonOptions);
                }
                Invalidate();

                return true;
            }
            //return false;
        } // LoadLanguage

        public string Language { get; private set; } = "";

        public string this[string key]
        {
            get
            {
                if (m_Strings != null && m_Strings.TryGetValue(key, out string? res))
                    return res.Replace("\\n", "\n");

                return $"{Language}:{key}";
            }
        }

        public static Localizer Instance { get; set; } = new Localizer();
        public event PropertyChangedEventHandler? PropertyChanged;

        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
        }
    }
}