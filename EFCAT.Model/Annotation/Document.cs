using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace EFCAT.Model.Annotation;

[NotMapped]
[Owned]
public class Document {
    private byte[]? _content;
    public byte[] Content { get => _content ?? new byte[0]; set => _content = value; }

    private string? _type;
    public string Type { get => _type ?? ""; set => _type = value; }

    protected string filter = "";

    public Document() { }

    public Document(byte[] content, string type) {
        _content = content;
        _type = type;
    }

    public async Task SetStream(Stream stream) {
        using(var memorystream = new MemoryStream()) {
            await stream.CopyToAsync(memorystream);
            _content = memorystream.ToArray();
            memorystream.Close();
        }
    }

    public void SetType(string type) {
        if (filter == "" || new Regex($"^{filter}").IsMatch(type)) _type = type;
        else throw new Exception("Invalid Format");
    }

    public async Task Set(Stream stream, string type) {
        SetType(type);
        await SetStream(stream);
    }
    
    public MemoryStream GetStream() => new MemoryStream(Content);
    public string GetSource() => $"data:{Type};base64,{Convert.ToBase64String(Content)}";

    public override string ToString() => GetSource();
}