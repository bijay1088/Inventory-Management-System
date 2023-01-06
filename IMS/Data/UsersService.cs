using System.Text.Json;

namespace IMS.Data;

public static class UsersService
{
    public const string SeedUsername = "admin";
    public const string SeedPassword = "admin";

    private static void SaveAll(List<User> users) // save list in json file
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appUsersFilePath = Utils.GetAppUsersFilePath();

        if (!Directory.Exists(appDataDirectoryPath)) // if direcotry doesn't exist
        {
            Directory.CreateDirectory(appDataDirectoryPath);  //create directory
        }

        var json = JsonSerializer.Serialize(users); // Serialize the object to a JSON string and store in json
        File.WriteAllText(appUsersFilePath, json); // write json in file
    }

    public static List<User> GetAll() //getting all user list
    {
        string appUsersFilePath = Utils.GetAppUsersFilePath();
        if (!File.Exists(appUsersFilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(appUsersFilePath);

        return JsonSerializer.Deserialize<List<User>>(json);
    }

    public static List<User> Create(Guid userId, string username, string password, Role role) //creating user
    {
        List<User> users = GetAll();
        bool usernameExists = users.Any(x => x.Username == username);

        if (usernameExists)
        {
            throw new Exception("Username already exists.");
        }

        users.Add(
            new User
            {
                Username = username,
                PasswordHash = Utils.HashSecret(password),
                Role = role,
                CreatedBy = userId
            }
        );
        SaveAll(users);
        return users;
    }

    public static void SeedUsers() //default user
    {
        var users = GetAll().FirstOrDefault(x => x.Role == Role.Admin);

        if (users == null)
        {
            Create(Guid.Empty, SeedUsername, SeedPassword, Role.Admin);
        }
    }

    public static User GetById(Guid id) //getting id of user
    {
        List<User> users = GetAll();
        return users.FirstOrDefault(x => x.Id == id);
    }

    public static List<User> Delete(Guid id) //deleteing user
    {
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        //UsersService.DeleteByUserId(id);
        users.Remove(user);
        SaveAll(users);

        return users;
    }

    public static User Login(string username, string password) //logging in
    {
        var loginErrorMessage = "Invalid username or password.";
        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Username == username);

        if (user == null)
        {
            throw new Exception(loginErrorMessage);
        }

        bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash); //verifying password

        if (!passwordIsValid)
        {
            throw new Exception(loginErrorMessage);
        }

        return user;
    }

    public static User ChangePassword(Guid id, string currentPassword, string newPassword) //changing password
    {
        if (currentPassword == newPassword) //if new and old password is same
        {
            throw new Exception("New password must be different from current password.");
        }

        List<User> users = GetAll();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null) //if user is empty
        {
            throw new Exception("User not found.");
        }

        bool passwordIsValid = Utils.VerifyHash(currentPassword, user.PasswordHash); //verifying password if it is valid or not

        if (!passwordIsValid)
        {
            throw new Exception("Incorrect current password.");
        }

        user.PasswordHash = Utils.HashSecret(newPassword); //getting new encrypted password
        user.HasInitialPassword = false;
        SaveAll(users);

        return user;
    }
}
