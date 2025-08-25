using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class CellResponse 
{
    [JsonProperty("stage_id")]
    public int id { get; set; }

    [JsonProperty("x")]
    public float x { get; set; }

    [JsonProperty("y")]
    public float y { get; set; }

    [JsonProperty("object_id")]
    public int object_id { get; set; }
}
