using System;

namespace API.Entites;

public class AppUser
{
public int id { get; set; }
public required string UserName { get; set; }
public required byte[] PasswordHash { get; set; }
public required byte[] PasswordSalt { get; set; }

}
