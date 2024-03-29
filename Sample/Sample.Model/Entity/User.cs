﻿
namespace Sample.Model.Entity;

[Table("USERS")]
public class User {
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    [Unique]
    [NotNull(ErrorMessage = "Email can not be null.")]
    [Varchar(255, Min = 5)]
    public string Email { get; set; }

    [Unique]
    [NotNull(ErrorMessage = "Name can not be null.")]
    [Varchar(16, Min = 3, Max = 16, Pattern = "^[A-z]+$", ErrorMessage = "You need to have between @min and @max characters.")]
    public string Name { get; set; }

    [NotNull]
    public Crypt<SHA256> Password { get; set; }

    // Number from 0-100 saved as DECIMAL(5, 2) which is equal to 999.99
    [Min(10, ErrorMessage = "Balance needs to have a Minimum of 10.")]
    [Max(20, ErrorMessage = "Balance needs to have a Maximum of 20.")]
    [Number(3, 2, Min = 2)]
    public decimal Balance { get; set; } = 15;

    [Nullable]
    public Image? Image { get; set; }

    [Range(1, 5, ErrorMessage = "The List needs to have between 1 and 5 elements.")]
    public ICollection<Code>? Codes { get; set; }

    public ZMail? Mail { get; set; }

    [Implement]
    public Implemented? Impl { get; set; }

    public ICollection<Role> Roles { get; set; }
}
