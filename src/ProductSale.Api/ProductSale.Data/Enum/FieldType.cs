
namespace ProductSale.Data.Enum { }
public enum FieldType
{
    UserName,
    Email
}

public static class FieldTypeService
{
    public static bool IsValidFieldType(string fieldType)
    {
        return Enum.TryParse(typeof(FieldType), fieldType, true, out _);
    }
}
