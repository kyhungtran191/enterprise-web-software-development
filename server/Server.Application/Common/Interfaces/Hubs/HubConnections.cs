namespace Server.Application.Common.Interfaces.Hubs;

public static class HubConnections
{

    // userid == connectionid
    public static Dictionary<string, List<string>> Users = new();

    public static bool HasUserConnection(string userId, string connectionId)
    {
        try
        {
            if (Users.ContainsKey(userId))
            {
                return Users[userId].Any(p => p.Contains(connectionId));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return false;
    }

    public static bool HasUser(string userId)
    {
        try
        {
            if (Users.ContainsKey(userId))
            {
                return Users[userId].Any();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return false;
    }

    public static void AddUserConnection(string userId, string connectionId)
    {
        if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(connectionId))
        {
            if (Users.ContainsKey(userId))
            {
                Users[userId].Add(connectionId);
            }
            else
            {
                Users.Add(userId, new List<string> { connectionId });
            }
        }
    }

    public static List<string> GetOnlineUsers()
    {
        return Users.Keys.ToList();
    }

}