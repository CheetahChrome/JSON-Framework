namespace JSON_Enumerate.Operation
{
    public interface IJsonSettings
    {
        bool AddDTOSuffix { get; set; }

        bool AddTableTypeConstraint { get; set; }

        bool IsSortProperties { get; set; }

    }
}