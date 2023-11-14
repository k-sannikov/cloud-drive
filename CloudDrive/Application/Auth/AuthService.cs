using Domain.Auth;
using System.Text;
using System.Security.Cryptography;
using Application.FileSystem;
using Domain.FileSystem;
using Application.AccessSystem;
using Domain.AccessSystem;

namespace Application.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IFileSystemRepository _fileSystemRepository;
    private readonly IAccessService _accessService;

    public AuthService(IUserRepository userRepository,
        IFileSystemRepository fileSystemRepository,
        IAccessService accessService)
    {
        _userRepository = userRepository;
        _fileSystemRepository = fileSystemRepository;
        _accessService = accessService;
    }

    public async Task RegisterUser(User user)
    {
        user.Password = Hash(user.Password);
        user.Id = Guid.NewGuid().ToString();
        await _userRepository.AddUser(user);

        Node rootNode = new()
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Мой диск",
            Type = NodeType.Folder.ToString(),

        };
        await _fileSystemRepository.AddNode(rootNode);

        Access rootAccess = new()
        {
            UserId = user.Id,
            NodeId = rootNode.Id,
            IsOwner = true,
            IsRoot = true,
        };

        await _accessService.AddAccess(rootAccess);
    }

    public async Task<User> GetUser(string username, string password)
    {
        password = Hash(password);
        return await _userRepository.GetByUsernameAndPassword(username, password);
    }

    public async Task<User> GetUser(string token)
    {
        return await _userRepository.GetByRefreshToken(token);
    }

    private static string Hash(string str)
    {
        SHA256 sha256 = SHA256.Create();
        byte[] hashValue;
        UTF8Encoding objUtf8 = new();
        hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));

        return Convert.ToBase64String(hashValue);
    }
}
