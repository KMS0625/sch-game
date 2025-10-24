using UnityEngine;
using System.Threading.Tasks;
using Postgrest.Attributes;
using Postgrest.Models;
using System.Linq;
using Postgrest;
using Newtonsoft.Json;
using System.Collections.Generic;

[Table("User")]
public class User : BaseModel
{
    [PrimaryKey("id", true)]
    public string id { get; set; }

    [Column("name")]
    public string name { get; set; }

    [Column("department")]
    public string department { get; set; }

    [Column("remaining_plays")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int remaining_plays { get; set; }

    [Column("high_score")]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int high_score { get; set; }
}

public class SupabaseClient : MonoBehaviour
{
    private const string URL = "https://bhqwhktgunfyxlioczfw.supabase.co";
    private const string anonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJocXdoa3RndW5meXhsaW9jemZ3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjA5MDMyMjYsImV4cCI6MjA3NjQ3OTIyNn0.CE-9a8P2rH86rbqmIHEUDv15fvz-iMfcnLoVqkARKt0";

    public static SupabaseClient instance { get; private set; }
    public Supabase.Client client;

    private string userID = null;


    void Awake()
    {
        userID = null;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            client = new Supabase.Client(URL, anonKey);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetUserID(string id)
    {
        userID = id;
    }

    public string GetUserID()
    {
        return userID;
    }

    public async Task<Login.LoginState> Login(string id, string name, string department)
    {
        var response = await client.From<User>().Where(x => x.id == id).Single();

        if (response == null)
        {
            return global::Login.LoginState.UserIdNotFound;
        }

        if (response.department != department)
        {
            return global::Login.LoginState.DpartmentMisMatch;
        }
        else if (response.name != name)
        {
            return global::Login.LoginState.NameMismatch;
        }
        else if (response.remaining_plays <= 0)
        {
            return global::Login.LoginState.AccountLocked;
        }

        SetUserID(id);
        return global::Login.LoginState.Success;
    }

    public async Task<bool> Signup(string id, string name, string department)
    {
        Debug.Log("회원가입 시도");
        var response = await client.From<User>().Where(x => x.id == id).Get();
        if (response.Models.Any())
        {
            return false;
        }

        var user = new User
        {
            id = id,
            department = department,
            name = name
        };

        await client.From<User>().Insert(user, new QueryOptions { Returning = QueryOptions.ReturnType.Minimal });

        return true;
    }

    public async Task ScoreUpdate(int currentScore)
    {
        string id = userID;
        var response = await client.From<User>().Where(x => x.id == id).Single();

        if (response.high_score < currentScore)
        {
            response.high_score = currentScore;
            await response.Update<User>();
        }
    }

    public async Task<List<User>> GetAllScore()
    {
        string selectColumns = "id, name, department, high_score";
        var response = await client.From<User>().Select(selectColumns).Order("high_score", Postgrest.Constants.Ordering.Descending).Get();

        if (response.Models != null)
        {
            return response.Models;
        }
        else
        {
            return new List<User>();
        }
    }

    public async void UpdateRemainingPlays()
    {
        var response = await client.From<User>().Where(x => x.id == userID).Select("remaining_plays").Single();
        int currentReamining = response.remaining_plays;
        currentReamining -= 1;
        Debug.Log(currentReamining);

        var update = await client.From<User>().Where(x => x.id == userID).Set(x => x.remaining_plays, currentReamining).Update();
    }

    public async Task<bool> checkRemainingPlays()
    {
        var response = await client.From<User>().Where(x => x.id == userID).Select("remaining_plays").Single();

        if (response.remaining_plays > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

