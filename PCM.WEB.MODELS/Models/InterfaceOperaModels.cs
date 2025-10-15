namespace PCM.WEB.MODELS
{


    public class AuthenticationOpera
    {
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string oracle_tk_context { get; set; }
        public string refresh_token { get; set; }
        public string oracle_grant_type { get; set; }
        public string access_token { get; set; }
    }


    public class HotelRoomsOpera
    {
        public Hotelroomsdetails hotelRoomsDetails { get; set; }
        public int totalPages { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public bool hasMore { get; set; }
        public int totalResults { get; set; }
        public object[] links { get; set; }
    }

    public class Hotelroomsdetails
    {
        public Room[] room { get; set; }
        public string hotelId { get; set; }
    }

    public class Room
    {
        public Roomtype roomType { get; set; }
        public string floor { get; set; }
        public string floorDescription { get; set; }
        public string smokingPreference { get; set; }
        public string smokingPreferenceDescription { get; set; }
        public string roomId { get; set; }
        public Housekeeping housekeeping { get; set; }
    }

    public class Roomtype
    {
        public bool pseudo { get; set; }
        public string roomClass { get; set; }
        public string roomType { get; set; }
    }

    public class Housekeeping
    {
        public Roomstatus roomStatus { get; set; }
    }

    public class Roomstatus
    {
        public string roomStatus { get; set; }
        public string frontOfficeStatus { get; set; }
    }

}
