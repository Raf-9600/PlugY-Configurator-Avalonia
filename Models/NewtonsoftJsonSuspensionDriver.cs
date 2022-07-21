using Newtonsoft.Json;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

public class NewtonsoftJsonSuspensionDriver : ISuspensionDriver
{
    private readonly string _file;
    private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All, DefaultValueHandling = DefaultValueHandling.Ignore, Formatting = Formatting.Indented
    };

    public NewtonsoftJsonSuspensionDriver(string file) => _file = file;

    public IObservable<Unit> InvalidateState()
    {
        if (File.Exists(_file))
            File.Delete(_file);
        return Observable.Return(Unit.Default);
    }

    string startJson;
    public IObservable<object> LoadState()
    {
        if (!File.Exists(_file))
            return Observable.Throw<object>(new FileNotFoundException(_file));
        //return null;
        startJson = File.ReadAllText(_file);
        var state = JsonConvert.DeserializeObject<object>(startJson, _settings);
        return Observable.Return(state);
    }

    public IObservable<Unit> SaveState(object state)
    {
        var endJson = JsonConvert.SerializeObject(state, _settings);

        Directory.CreateDirectory(PlugY_Configurator_Avalonia.App.MainSettingsDir);
        if(endJson != startJson)
            File.WriteAllText(_file, endJson);
        return Observable.Return(Unit.Default);
    }
}
