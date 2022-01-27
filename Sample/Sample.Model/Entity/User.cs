using EFCAT.Model.Annotation;
using EFCAT.Model.Data;

namespace Sample.Model.Entity;

[Table("USERS")]
public class User {
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    [Unique]
    [Varchar(255, Min = 5, Nullable = false)]
    public string Email { get; set; }

    [Unique]
    [Varchar(16, Min = 3, Max = 16, Nullable = false, ErrorMessage = "You need to have between @min and @max characters.")]
    public string Name { get; set; }

    public Crypt<SHA256> Password { get; set; }

    // Number from 0-100 saved as DECIMAL(5, 2) which is equal to 999.99
    [Decimal(3, 2, Min = 10, Max = 20)]
    public decimal Balance { get; set; } = 15;

    public Image? Image { get; set; }

    public ICollection<Code>? Codes { get; set; }

    public ZMail Mail { get; set; }
}
