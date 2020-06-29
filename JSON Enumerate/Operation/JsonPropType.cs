namespace JSON_Enumerate.Operation
{
    /// <summary>
    /// Used for the property type in `IProperty`. Undefined can mean failure in processing
    /// either on the parsers side or the WalkThrough code.
    /// </summary>
    public enum JsonPropType
    {
        Undefined,
        StrType,
        NumberType,
        BoolType,
        UserType
    }
}
