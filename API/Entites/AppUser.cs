using System;
using API.Extensions;

namespace API.Entites;

public class AppUser
{
public int id { get; set; }
public required string UserName { get; set; }
public  byte[] PasswordHash { get; set; } = [];
public byte[] PasswordSalt { get; set; } = [];
public DateOnly DateOfBirth {get;set;}
public required string Knowns {get; set;}
public DateTime Created {get;set;} = DateTime.UtcNow;
public DateTime LastActive {get;set;} = DateTime.UtcNow;
public required string Gender {get;set;}
public string? Introduction {get;set;}
public string? Interests {get;set;}
public string? LookingFor {get;set;}
public required string City {get;set;}
public string? Country {get;set;}
public List<Photo> Photos {get; set;} =[];

// public int GetAge()
// {
//     return DateOfBirth.CalculateAge();
// }



}
