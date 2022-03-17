
using EFCAT.Model.Annotation;

namespace Sample.Model.Entity;

[NotGenerated]
public class Person {
    [Varchar(32)]
    public string FirstName { get; set; }

    [Varchar(32)]
    public string LastName { get; set; }
}

[Table("NICE_PEOPLE")]
public class NicePerson : Person {
    [AutoIncrement]
    [PrimaryKey]
    public int Id { get; set; }
}

[Table("BAD_PEOPLE")]
public class BadPerson {
    [AutoIncrement]
    [PrimaryKey]
    public int Id { get; set; }

    [Implement]
    public Person Person { get; set; }
}