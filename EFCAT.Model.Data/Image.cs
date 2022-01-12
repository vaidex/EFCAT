
namespace EFCAT.Model.Data;

public class Image : Document {
    public Image() {
        base.filter = "image";
    }

    public Image(byte[] content, string type) : base(content, type) {
        base.filter = "image";
    }

    public string GetImageSource() => base.GetSource();
}