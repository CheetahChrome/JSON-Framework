namespace JSON_Enumerate.Operation
{
    public interface IJsonSettings
    {
        string Name { get; set; }
        string ClassName { get; set; }
        bool AddDTOSuffix { get; set; }
        bool AddJsonProperty { get; set; }
        bool AddJsonPropertyName { get; set; }
        bool AddTableTypeConstraint { get; set; }
        bool IsSortProperties { get; set; }
        bool IsNameUndefined { get; set; }
    }
}