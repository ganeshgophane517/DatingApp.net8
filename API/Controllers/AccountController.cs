using System;
using System.Security.Cryptography;
using System.Text;
using API.Entites;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using System.Security.Principal;

namespace API.Controllers;

public class AccountController(DataContext context,ITokenService tokenService): BaseApiController
{
    [HttpPost("register")]  //account/register
    public async Task<ActionResult<UserDto>>Register(RegisterDto registerDto)
{
    if( await UserExit(registerDto.username))  return BadRequest("Username is taken");
    return Ok();
    // using var hmac= new HMACSHA512();
    // var user= new AppUser
    // {
        
    //     UserName=registerDto.username.ToLower(),
    //     PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.password)),
    //     PasswordSalt=hmac.Key
    // };
    // context.Users.Add(user);
    // await context.SaveChangesAsync();
    
    // return new UserDto{
    //     username=user.UserName,
    //     Token=tokenService.CreateToken(user)
    // };
}

[HttpPost("login")] //account/login
public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
{
    var user = await context.Users.FirstOrDefaultAsync(x=>
    x.UserName == loginDto.Username.ToLower());
    //  x.UserName == loginDto.Username.Trim().ToLower());

    if(user == null) return Unauthorized("Invalid username");

    using var hmac =new HMACSHA512 (user.PasswordSalt);

    var computeHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

    for(int i=0;i<computeHash.Length;i++)
    {
       if(computeHash[i] != user.PasswordHash[i]) return Unauthorized ("Invalid Password");
    }

        return new UserDto
    {
      username=user.UserName,
      Token=tokenService.CreateToken(user)
    };


    }
private async Task<bool> UserExit(string username)
{
    return await context.Users.AnyAsync(x=> x.UserName.ToLower()==username.ToLower());
}
}
