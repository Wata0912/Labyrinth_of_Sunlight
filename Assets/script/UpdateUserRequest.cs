using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateUserRequest
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("level")]
    public int Level { get; set; }
}
