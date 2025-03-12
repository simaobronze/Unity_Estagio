using System.Collections.Generic;

[System.Serializable]
public class Team
{
    public int id;
    public string name;
    public string description;
    public List<TeamUser> users;
}

[System.Serializable]
public class TeamUser
{
    public int user_id;
    public string user_name;
}