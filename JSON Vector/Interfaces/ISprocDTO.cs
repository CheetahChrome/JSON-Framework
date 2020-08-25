namespace JSON_Vector.Interfaces
{
    public interface ISprocDTO
    {
        string StoredProcedureName { get; }

        /// <summary>
        /// Is the sproc being used, uses table types instead
        /// of just raw parameters?
        /// </summary>
        bool UsesTableTypes { get; }

    }
}