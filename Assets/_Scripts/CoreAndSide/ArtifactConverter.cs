using UnityEngine;
using Newtonsoft.Json;
using System;

public class ArtifactConverter : JsonConverter<Artifact>
{
    public override Artifact ReadJson(JsonReader reader, Type objectType, Artifact existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, Artifact value, JsonSerializer serializer)
    {
        //writer. 
    }
}