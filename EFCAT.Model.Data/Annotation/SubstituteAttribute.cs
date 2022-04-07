
namespace EFCAT.Model.Data.Annotation;

public class SubstituteAttribute : Attribute {
    public string[] ColumnNames { get; set; }

    public SubstituteAttribute(params string[] columnNames) {
        ColumnNames = columnNames;
    }
    public SubstituteAttribute(string columnName) : this((!String.IsNullOrWhiteSpace(columnName) ? columnName.Replace(" ", "").Split(",") : new string[0])) { }
}
