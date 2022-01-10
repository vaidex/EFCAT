using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace EFCAT.Model.Annotation;

[NotMapped]
[Owned]
public class Image : Document {
    public Image() {
        base.filter = "image";
    }

    public Image(byte[] content, string type) : base(content, type) {
        base.filter = "image";
    }

    public string GetImageSource() => base.GetSource();
}