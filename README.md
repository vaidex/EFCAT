# EFCAT
A simple tool for your sql entity framework core project.

## Features
 - Multiple Primary Keys
 - Easier Foreign Columns
 - Attribute Validation (Min, Max, Pattern, ...)
 - Uniqueness
 - Automatic Encryption
 - Data Types (Documents, Images, ...)

## Usage
### Implementation
First of all, to implement EFCAT you need to change the inheritance of your MyDbContext from DbContext to DatabaseContext.
After that the new features get registered by your project.
```C#
public class MyDbContext : DatabaseContext {
    // DbSet ...

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
    }

}
```

### Attributes
***[PrimaryKey]*** Sets a property to a primary key. <br />
***[AutoIncrement]*** Auto generates the next possible id.
```C#
[PrimaryKey]
[AutoIncrement]
public int Id { get; set; }
```

***[ForeignColumn(TYPE, KEYS)]*** Creates a connection to the given entity.
 - **TYPE**
   - MANY_TO_ONE
   - ONE_TO_ONE
```C#
[ForeignColumn(ForeignType.MANY_TO_ONE, "COLUMN_ID")]
public Column ColumnName { get; set; }
```
***[TypeAttribute(SIZE)]*** Creates a Column with the given type and sets it to the specified size.
 - **Types**
   - String Data Types
     - Char
     - Varchar
     - Binary
     - Varbinary
     - Tinyblob
     - Tinytext
     - Text
     - Blob
     - Mediumtext
     - Mediumblob
     - Longtext
     - Longblob
   - Numeric Data Types
     - Bit
     - Tinyint
     - Bool / Boolean
     - Smallint
     - Mediumint
     - Int / Integer
     - Bigint
     - Float
     - Double
     - Decimal
 - **Attributes**
   - _Max_ sets the maximum size of the value.
   - _Min_ sets the minimum size of the value.
   - _Nullable_ sets if the value is allowed to be nullable.
   - _ErrorMessage_ is the message that gets returned if a requirement is not met.

_Example:_ A Varchar which needs to have between 3 and 16 characters and is not nullable.
```C#
[Varchar(16, Min = 3, Max = 16, Nullable = false, ErrorMessage = "You need to have between @min and @max characters.")]
public string Name { get; set; }
```

***[Unique]*** Checks if the value is unique.
```C#
[Unique]
public string Name { get; set; }
```


### Data Types
***Crypt\<Algorithm\>*** Encrypts a string with the given algorithm.
```C#
public Crypt<SHA256> CryptedValue { get; set; }
```

***Document*** Create a sql environment to save Documents.
```C#
public Document Doc { get; set; }
```
