namespace cShipment.Models
{
    /// <summary>
    /// Represents a response from a business service operation (not HTTP).
    /// Used to return operation status, created IDs, and error messages.
    /// </summary>
    public class ServiceResponse
    {
        /// <summary>
        /// Possible outcomes of a service-layer operation.
        /// </summary>
        public enum ServiceStatus
        {
            NotFound,
            Created,
            Updated,
            Deleted,
            Error
        }

        /// <summary>
        /// The status of the operation (e.g., Created, Error).
        /// </summary>
        public ServiceStatus Status { get; set; }

        /// <summary>
        /// The ID of the newly created entity, if applicable.
        /// </summary>
        public int CreatedId { get; set; }

        /// <summary>
        /// Any logic or validation messages returned by the service.
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();
    }
}
