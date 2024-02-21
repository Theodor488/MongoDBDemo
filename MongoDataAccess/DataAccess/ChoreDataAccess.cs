using MongoDataAccess.Models;
using MongoDB.Driver;

namespace MongoDataAccess.DataAccess;

public class ChoreDataAccess
{
    private const string ConnectionString = "mongodb://127.0.0.1:27017";
    private const string DatabaseName = "choredb";
    private const string ChoreCollection = "chore_chart";
    private const string UserCollection = "users";
    private const string ChoreHistoryCollection = "chore_history";

    private IMongoCollection<T> ConnectToMongo<T>(in string collection)
    {
        var client = new MongoClient(ConnectionString);
        var db = client.GetDatabase(DatabaseName);
        return db.GetCollection<T>(collection); 
    }

    private async Task<List<UserModel>> GetAllUsers()
    {
        var usersCollection = ConnectToMongo<UserModel>(UserCollection);
        var results = await usersCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<List<ChoreModel>> GetAllChores()
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var results = await choresCollection.FindAsync(_ => true);
        return results.ToList();
    }

    public async Task<List<ChoreModel>> GetAllChoresForAUser(UserModel user)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        var results = await choresCollection.FindAsync(c => c.AssignedTo.Id == user.Id);
        return results.ToList();
    }

    public Task CreateUser(UserModel user)
    {
        var usersCollection = ConnectToMongo<UserModel>(UserCollection);
        return usersCollection.InsertOneAsync(user);
    }

    public Task CreateChor(ChoreModel chore)
    {
        var choresCollection = ConnectToMongo<ChoreModel>(ChoreCollection);
        return choresCollection.InsertOneAsync(chore);
    }
}

