using System.ComponentModel.DataAnnotations;

namespace EFCAT.Model.Annotation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class Unique : ValidationAttribute {

    public Unique() {

    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        /*var context = validationContext.GetService(typeof(DbContext)) as DbContext;
        if (context == null) return new ValidationResult("Database connection not possible.");
        using(var command = context.Database.GetDbConnection().CreateCommand()) {
            command.CommandText = "SELECT COUNT(*) FROM @table WHERE @variable=@value;";
            command.CommandType = System.Data.CommandType.Text;
            command.Parameters.Add(new SqlParameter("@table", validationContext.ObjectType));
            command.Parameters.Add(new SqlParameter("@variable", validationContext.DisplayName));
            command.Parameters.Add(new SqlParameter("@value", value));

            context.Database.OpenConnection();

            using(var result = command.ExecuteReader()) {
                if (result.Read()) return new ValidationResult(ErrorMessage);
                return ValidationResult.Success;
            }
        }*/
        return null;
    }
}
