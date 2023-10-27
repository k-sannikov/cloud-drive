
namespace Domain.AccessService;

public class UserForAccess
{
    public int Id { get; set; }

    public List<Access> Accesses { get; set; }
}
