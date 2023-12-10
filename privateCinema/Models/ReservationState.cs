using System.Text.Json.Serialization;

namespace privateCinema.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ReservationState
    {
       Waiting=1,
       Confirmed,
       Denied,
       Done
    }
}
