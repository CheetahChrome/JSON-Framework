namespace JSON_Enumerate.Operation
{
    /// <summary>
    /// Within each IJsonOperation instance, it will have properties. This
    /// contract defines how the properties operate. 
    /// </summary>
    public interface IProperty
    {
        string Name { get; set; }
        int Size { get; set; }

        JsonPropType JsonType { get; set; }

    }
}