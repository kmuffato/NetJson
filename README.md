# NetJson
#### *JSON parsing, generation and reflective class-insertion library for .NET*

[![Gitter](https://img.shields.io/gitter/room/jonasfx/NetJson.svg)](https://gitter.im/JonasFX_NetJson/)
[![Issues](https://img.shields.io/github/issues/jonasfx21/NetJson.svg)](https://github.com/jonasfx21/NetJson/issues)
[![Nuget](https://img.shields.io/nuget/v/JonasFX.NetJson.svg)](https://www.nuget.org/packages/JonasFX.NetJson/)
[![Donate](https://www.paypalobjects.com/en_US/DK/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=ENF6YH2W5WND4&source=url)

You can use this library in everything you want, for free. (Even commercially!)
You don't even need to credit me.

## Example using insertion
```c#
using NetJson;

// Simple data structure
class TestClassA
{
    [JsonField] public TestClassB[] Array;
    [JsonField] public List<TestClassB> Collection;
    [JsonField] public TestClassB Object;
    class TestClassB
    {
        [JsonField("named_field")] public string NamedField;
    }
}

class Program
{
    static TestClassA Test;
    static void Main(string[] args)
    {
        // You can use JSON.ParseTo<T>(JSON) to create an object instance out of JSON.
        Test = JSON.ParseTo<TestClassA>(System.IO.File.ReadAllText("values.json"));
        
        // You can use JSON.StringifyFrom<T>(Instance) to create JSON out of an object instance.
        JSON.StringifyFrom<TestClassA>(Test);
    }
}
```

## Example without insertion
```c#
using NetJson;

class Program
{
    static TestClassA Test;
    static void Main(string[] args)
    {
        // You can use JSON.Parse(Object) to create an object out of JSON. (Possible: JsonObject, JsonArray, ...)
        JsonObject obj = JSON.Parse(System.IO.File.ReadAllText("values.json"));
        Console.WriteLine("Name: " + (string)obj["name"]);
        
        // You can use JSON.Stringify(Object) to create JSON out of a JSON-compatible type.
        obj["name"] = Console.ReadLine();
        JSON.Stringify(obj);
    }
}
```
