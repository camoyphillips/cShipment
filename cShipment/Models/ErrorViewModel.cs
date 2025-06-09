namespace cShipment.Models
{
    /// <summary>
    /// ViewModel to capture and relay error details in MVC views.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Unique request identifier to trace error occurrences.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Flag that determines whether to show the RequestId.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
