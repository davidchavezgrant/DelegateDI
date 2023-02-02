namespace Bylines.Chat.Contracts;

public static class ChatRoutes
{
	public const string CREATE_OR_UPDATE_REGISTERED_PROFILE            = "/api/registerProfile";
	public const string LIST_MESSAGES_BY_PROFILE_ID_FILTERED_BY_APP_ID = "LIST_MESSAGES_BY_PROFILE_ID_FILTERED_BY_APP_ID";
	public const string LIST_ROOMS_BY_PROFILE_ID_FILTERED_BY_APP_ID    = "/api/filtered/rooms";
	public const string UPDATE_UNREAD_MESSAGES                         = "/api/updateunread";
	public const string GET_PROFILE_BY_OWNER_NAME                      = "/api/profile";
	public const string SEARCH_PROFILES                                = "/api/profiles";
	public const string LIST_CONTACTS                                  = "/api/contacts";
	public const string ADD_CONTACT                                    = "/api/contacts";
	public const string HUB_ENDPOINT                                   = "/hubs/chat";
	public const string LIST_ROOMS_BY_PROFILE_ID                       = "/api/rooms";
	public const string SEND_MESSAGE                                   = "/api/messages";
	public const string LIST_MESSAGES_BY_ROOM_ID                       = "/api/room/messages";
	public const string GET_ROOM_BY_ID                                 = "/api/room";
	public const string LIST_MESSAGES_BY_PROFILE_ID                    = "/api/profilesmessages";
	public const string GET_PROFILES_BY_ID                             = "/api/profiles";
	public const string UPDATE_FLAG_BY_MESSAGE_ID                      = "/api/flag";
	public const string LIST_CHANNELS_BY_PROFILE_ID                    = "/api/channels";
}