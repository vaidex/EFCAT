﻿using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCAT.Model.Data;

[NotMapped]
public class Json<TObject> where TObject : class, new() {
    public TObject Root { get; set; }
    public Json() => Root = new TObject();
    public Json(string json) => Root = JsonConvert.DeserializeObject<TObject>(json) ?? new TObject();
    public override string ToString() => JsonConvert.SerializeObject(Root);
}