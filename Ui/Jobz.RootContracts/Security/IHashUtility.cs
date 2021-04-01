namespace Jobz.RootContracts.Security
{
    public interface IHashUtility
    {
        string GenerateSalt();
        string Hash(string toBeHashed, string salt);
    }
}