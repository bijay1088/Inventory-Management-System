using System.Security.Cryptography;

namespace IMS.Data;

public static class Utils
{
    private const char _segmentDelimiter = ':';

    public static string HashSecret(string input) // hashing password, encryption
    {
        var saltSize = 16; // size of data
        var iterations = 100_000; // number of iterations
        var keySize = 32; // size of key
        HashAlgorithmName algorithm = HashAlgorithmName.SHA256; // algorithm used to hash
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

        return string.Join(
            _segmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            iterations,
            algorithm
        );
    }

    public static bool VerifyHash(string input, string hashString) //verifying password
    {
        string[] segments = hashString.Split(_segmentDelimiter);  //splitting hash string to array
        byte[] hash = Convert.FromHexString(segments[0]); // converting hash from hexadecimal to byte array
        byte[] salt = Convert.FromHexString(segments[1]); // converting salt from hexadecimal to byte array
        int iterations = int.Parse(segments[2]); // getting number of iterations
        HashAlgorithmName algorithm = new(segments[3]); //creating new hash algorithm object
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2( //storing hash to inputHash
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }

    public static string GetAppDirectoryPath() //getting app directory
    {
        return "../../userData";
    }

    public static string GetAppUsersFilePath() //getting user file directory
    {
        return Path.Combine(GetAppDirectoryPath(), "user.json");
    }

    public static string GetItemFilePath() // getting item file directory
    {
        return Path.Combine(GetAppDirectoryPath(), "items.json");
    }

    public static string GetItemWithdrawFilePath() //getting withdraw file directory
    {
        return Path.Combine(GetAppDirectoryPath(), "withdraw.json");
    }
} 
