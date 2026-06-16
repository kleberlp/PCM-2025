using Newtonsoft.Json;

namespace PCM.INTERFACE.ATRIO.Models;

// -------------------------------------------------------
// TOKEN
// -------------------------------------------------------
public class OracleTokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
}

public class OracleTokenState
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; } = DateTime.MinValue;

    public bool IsValid() =>
        !string.IsNullOrEmpty(AccessToken) && DateTime.UtcNow < ExpiresAt;
}

// -------------------------------------------------------
// RESPOSTA GENÉRICA
// -------------------------------------------------------
public class OracleApiResult<T>
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }

    public static OracleApiResult<T> Ok(T data) =>
        new() { Success = true, Data = data };

    public static OracleApiResult<T> Fail(string message) =>
        new() { Success = false, ErrorMessage = message };
}

// -------------------------------------------------------
// RESERVAS — estrutura real retornada pela Oracle OHIP
// -------------------------------------------------------

// Wrapper raiz: { "reservations": { ... }, "links": [...] }
public class ReservationListResponse
{
    [JsonProperty("reservations")]
    public ReservationCollection? Reservations { get; set; }

    [JsonProperty("links")]
    public OracleLink[]? Links { get; set; }
}

// Objeto "reservations" com paginação e array "reservationInfo"
public class ReservationCollection
{
    [JsonProperty("reservationInfo")]
    public ReservationInfo[]? ReservationInfo { get; set; }

    [JsonProperty("totalResults")]
    public int TotalResults { get; set; }

    [JsonProperty("totalPages")]
    public int TotalPages { get; set; }

    [JsonProperty("offset")]
    public int Offset { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("hasMore")]
    public bool HasMore { get; set; }
}

public class ReservationInfo
{
    [JsonProperty("reservationIdList")]
    public ReservationId[]? ReservationIdList { get; set; }

    [JsonProperty("hotelId")]
    public string? HotelId { get; set; }

    [JsonProperty("hotelName")]
    public string? HotelName { get; set; }

    [JsonProperty("reservationStatus")]
    public string? ReservationStatus { get; set; }

    [JsonProperty("computedReservationStatus")]
    public string? ComputedReservationStatus { get; set; }

    [JsonProperty("roomStay")]
    public RoomStay? RoomStay { get; set; }

    [JsonProperty("reservationGuest")]
    public ReservationGuest? ReservationGuest { get; set; }

    [JsonProperty("sharedGuests")]
    public SharedGuest[]? SharedGuests { get; set; }

    [JsonProperty("attachedProfiles")]
    public AttachedProfile[]? AttachedProfiles { get; set; }

    [JsonProperty("reservationIndicators")]
    public ReservationIndicator[]? ReservationIndicators { get; set; }

    [JsonProperty("sourceOfSale")]
    public SourceOfSale? SourceOfSale { get; set; }

    [JsonProperty("createDateTime")]
    public string? CreateDateTime { get; set; }

    [JsonProperty("lastModifyDateTime")]
    public string? LastModifyDateTime { get; set; }

    [JsonProperty("walkInIndicator")]
    public bool WalkInIndicator { get; set; }

    [JsonProperty("preRegistered")]
    public bool PreRegistered { get; set; }

    [JsonProperty("openFolio")]
    public bool OpenFolio { get; set; }

    [JsonProperty("checkInInitiatedBy")]
    public string? CheckInInitiatedBy { get; set; }

    [JsonProperty("paymentMethod")]
    public string? PaymentMethod { get; set; }

    [JsonProperty("commissionPayoutTo")]
    public string? CommissionPayoutTo { get; set; }

    // Atalho para pegar o ID de reserva principal
    public string? ReservationId =>
        ReservationIdList?.FirstOrDefault(x => x.Type == "Reservation")?.Id;

    // Atalho para pegar o número de confirmação
    public string? ConfirmationNumber =>
        ReservationIdList?.FirstOrDefault(x => x.Type == "Confirmation")?.Id;
}

public class ReservationId
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("idExtension")]
    public int? IdExtension { get; set; }
}

public class RoomStay
{
    [JsonProperty("arrivalDate")]
    public string? ArrivalDate { get; set; }

    [JsonProperty("departureDate")]
    public string? DepartureDate { get; set; }

    [JsonProperty("roomType")]
    public string? RoomType { get; set; }

    [JsonProperty("roomClass")]
    public string? RoomClass { get; set; }

    [JsonProperty("roomId")]
    public string? RoomId { get; set; }

    [JsonProperty("ratePlanCode")]
    public string? RatePlanCode { get; set; }

    [JsonProperty("rateAmount")]
    public RateAmount? RateAmount { get; set; }

    [JsonProperty("adultCount")]
    public int AdultCount { get; set; }

    [JsonProperty("childCount")]
    public int ChildCount { get; set; }

    [JsonProperty("numberOfRooms")]
    public int NumberOfRooms { get; set; }

    [JsonProperty("marketCode")]
    public string? MarketCode { get; set; }

    [JsonProperty("sourceCode")]
    public string? SourceCode { get; set; }

    [JsonProperty("sourceCodeDescription")]
    public string? SourceCodeDescription { get; set; }

    [JsonProperty("guarantee")]
    public Guarantee? Guarantee { get; set; }

    [JsonProperty("balance")]
    public RateAmount? Balance { get; set; }

    [JsonProperty("fixedRate")]
    public bool FixedRate { get; set; }
}

public class RateAmount
{
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("currencyCode")]
    public string? CurrencyCode { get; set; }
}

public class Guarantee
{
    [JsonProperty("guaranteeCode")]
    public string? GuaranteeCode { get; set; }

    [JsonProperty("shortDescription")]
    public string? ShortDescription { get; set; }
}

public class ReservationGuest
{
    [JsonProperty("id")]
    public string? Id { get; set; }

    [JsonProperty("givenName")]
    public string? GivenName { get; set; }

    [JsonProperty("surname")]
    public string? Surname { get; set; }

    [JsonProperty("phoneNumber")]
    public string? PhoneNumber { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("birthDate")]
    public string? BirthDate { get; set; }

    [JsonProperty("language")]
    public string? Language { get; set; }

    [JsonProperty("address")]
    public GuestAddress? Address { get; set; }

    // Nome completo formatado
    public string FullName => $"{GivenName} {Surname}".Trim();
}

public class GuestAddress
{
    [JsonProperty("streetAddress")]
    public string? StreetAddress { get; set; }

    [JsonProperty("cityName")]
    public string? CityName { get; set; }

    [JsonProperty("state")]
    public string? State { get; set; }

    [JsonProperty("postalCode")]
    public string? PostalCode { get; set; }

    [JsonProperty("country")]
    public Country? Country { get; set; }
}

public class Country
{
    [JsonProperty("code")]
    public string? Code { get; set; }
}

public class SharedGuest
{
    [JsonProperty("profileId")]
    public ReservationId? ProfileId { get; set; }

    [JsonProperty("firstName")]
    public string? FirstName { get; set; }

    [JsonProperty("lastName")]
    public string? LastName { get; set; }

    [JsonProperty("fullName")]
    public string? FullName { get; set; }
}

public class AttachedProfile
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("reservationProfileType")]
    public string? ReservationProfileType { get; set; }

    [JsonProperty("profileIdList")]
    public ReservationId[]? ProfileIdList { get; set; }
}

public class ReservationIndicator
{
    [JsonProperty("indicatorName")]
    public string? IndicatorName { get; set; }

    [JsonProperty("count")]
    public int? Count { get; set; }
}

public class SourceOfSale
{
    [JsonProperty("sourceType")]
    public string? SourceType { get; set; }

    [JsonProperty("sourceCode")]
    public string? SourceCode { get; set; }
}

public class OracleLink
{
    [JsonProperty("href")]
    public string? Href { get; set; }

    [JsonProperty("rel")]
    public string? Rel { get; set; }

    [JsonProperty("method")]
    public string? Method { get; set; }

    [JsonProperty("operationId")]
    public string? OperationId { get; set; }
}

// -------------------------------------------------------
// HOUSEKEEPING
// Estrutura real retornada pela Oracle OHIP:
// { "housekeepingRoomInfo": { "housekeepingRooms": { "room": [...] }, "totalResults": 136, ... } }
// -------------------------------------------------------

// Raiz
public class HousekeepingOverviewResponse
{
    [JsonProperty("housekeepingRoomInfo")]
    public HousekeepingRoomCollection? HousekeepingRoomInfo { get; set; }

    [JsonProperty("links")]
    public OracleLink[]? Links { get; set; }
}

// Nível "housekeepingRoomInfo" — contém paginação + wrapper dos quartos
public class HousekeepingRoomCollection
{
    [JsonProperty("housekeepingRooms")]
    public HousekeepingRoomsWrapper? HousekeepingRooms { get; set; }

    [JsonProperty("totalResults")]
    public int TotalResults { get; set; }

    [JsonProperty("totalPages")]
    public int TotalPages { get; set; }

    [JsonProperty("offset")]
    public int Offset { get; set; }

    [JsonProperty("limit")]
    public int Limit { get; set; }

    [JsonProperty("hasMore")]
    public bool HasMore { get; set; }

    // Atalho direto para o array de quartos
    public HousekeepingRoomInfo[] Rooms =>
        HousekeepingRooms?.Room ?? [];
}

// Nível "housekeepingRooms" — contém o array "room" e o hotelId
public class HousekeepingRoomsWrapper
{
    [JsonProperty("room")]
    public HousekeepingRoomInfo[]? Room { get; set; }

    [JsonProperty("hotelId")]
    public string? HotelId { get; set; }
}

// Cada quarto individual
public class HousekeepingRoomInfo
{
    [JsonProperty("roomId")]
    public string? RoomId { get; set; }

    [JsonProperty("floor")]
    public string? Floor { get; set; }

    [JsonProperty("smokingPreference")]
    public string? SmokingPreference { get; set; }

    [JsonProperty("roomType")]
    public HousekeepingRoomType? RoomType { get; set; }

    [JsonProperty("housekeeping")]
    public HousekeepingDetail? Housekeeping { get; set; }

    [JsonProperty("outOfOrder")]
    public OutOfOrderInfo[]? OutOfOrder { get; set; }

    // Atalhos para campos mais usados
    public string? RoomTypeCode      => RoomType?.RoomTypeCode;
    public string? RoomClass         => RoomType?.RoomClass;
    public string? HousekeepingStatus => Housekeeping?.HousekeepingRoomStatus?.HousekeepingRoomStatus;
    public string? FrontOfficeStatus  => Housekeeping?.HousekeepingRoomStatus?.FrontOfficeStatus;
    public bool    IsOccupied         => FrontOfficeStatus == "Occupied";
    public bool    IsOutOfOrder       => OutOfOrder?.Length > 0;
    public string[]? ReservationStatusList =>
        Housekeeping?.HousekeepingRoomStatus?.ReservationStatusList;
}

public class HousekeepingRoomType
{
    [JsonProperty("roomType")]
    public string? RoomTypeCode { get; set; }

    [JsonProperty("roomClass")]
    public string? RoomClass { get; set; }

    [JsonProperty("pseudoRoom")]
    public bool PseudoRoom { get; set; }

    [JsonProperty("houseKeeping")]
    public bool HouseKeeping { get; set; }
}

public class HousekeepingDetail
{
    [JsonProperty("housekeepingRoomStatus")]
    public HousekeepingRoomStatusOpera? HousekeepingRoomStatus { get; set; }

    [JsonProperty("roomPersons")]
    public RoomPersons? RoomPersons { get; set; }
}

public class HousekeepingRoomStatusOpera
{
    [JsonProperty("housekeepingRoomStatus")]
    public string? HousekeepingRoomStatus { get; set; }

    [JsonProperty("frontOfficeStatus")]
    public string? FrontOfficeStatus { get; set; }

    [JsonProperty("housekeepingStatus")]
    public string? HousekeepingStatus { get; set; }

    [JsonProperty("reservationStatusList")]
    public string[]? ReservationStatusList { get; set; }
}

public class RoomPersons
{
    [JsonProperty("frontOfficePersons")]
    public int FrontOfficePersons { get; set; }

    [JsonProperty("houseKeepingPersons")]
    public int HouseKeepingPersons { get; set; }
}

public class OutOfOrderInfo
{
    [JsonProperty("roomRepairId")]
    public RoomRepairId? RoomRepairId { get; set; }
}

public class RoomRepairId
{
    [JsonProperty("id")]
    public string? Id { get; set; }
}
