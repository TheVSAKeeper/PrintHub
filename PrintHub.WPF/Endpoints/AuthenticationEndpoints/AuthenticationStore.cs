﻿using System.Text.Json;
using Calabonga.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PrintHub.Infrastructure;
using PrintHub.WPF.Properties;

namespace PrintHub.WPF.Endpoints.AuthenticationEndpoints;

public class AuthenticationStore(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
{
    public ApplicationUser? User { get; private set; }

    public bool IsLoggedIn => User != null;

    public string Username => User?.UserName ?? "Unknown";

    public bool IsInRole(string roleName)
    {
        return User != null && Task.Run(async () => await userManager.IsInRoleAsync(User, roleName)).Result;
    }

    public async Task Initialize()
    {
        string userIdJson = Settings.Default.User;

        if (string.IsNullOrEmpty(userIdJson))
            return;

        Guid? userId = JsonSerializer.Deserialize<Guid>(userIdJson);

        if (userId == null)
            return;

        User = await userManager.FindByIdAsync(userId.ToString()!);

        SaveAuthenticationState();
    }

    public async Task<SignInResult> SignInAsync(string userName, string password)
    {
        ApplicationUser? user = await userManager.FindByNameAsync(userName);

        if (user == null)
            return SignInResult.Failed;

        bool result = await userManager.CheckPasswordAsync(user, password);

        if (result == false)
            return SignInResult.Failed;

        User = user;
        SaveAuthenticationState();

        return SignInResult.Success;
    }

    public void SignOut()
    {
        User = null;
        ClearAuthenticationState();
    }

    private void SaveAuthenticationState()
    {
        if (User == null)
            return;

        string userIdJson = JsonSerializer.Serialize(User.Id);
        Settings.Default.User = userIdJson;
        Settings.Default.Save();
    }

    private static void ClearAuthenticationState()
    {
        Settings.Default.User = null;
        Settings.Default.Save();
    }

    public async Task<IdentityResult> CreateUserAsync(string username, string password, string roleName)
    {
        ApplicationRole? role = await roleManager.FindByNameAsync(roleName);

        if (role == null)
            return IdentityResult.Failed(new IdentityError { Description = $"Role {roleName} dont found" });

        EntityEntry<Client> client = await unitOfWork.GetRepository<Client>()
            .InsertAsync(new Client
            {
                FirstName = username,
                LastName = string.Empty,
                Address = string.Empty,
                Phone = string.Empty,
                Email = string.Empty
            });

        ApplicationUser user = new()
        {
            UserName = username,
            NormalizedUserName = username.ToUpper(),
            FirstName = string.Empty,
            LastName = string.Empty,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            Roles = new List<ApplicationRole> { role },
            ClientId = client.Entity.Id
        };

        IdentityResult createResult = await userManager.CreateAsync(user, password);

        if (createResult.Succeeded == false)
            return createResult;

        IdentityResult addToRoleResult = await userManager.AddToRoleAsync(user, roleName);

        return addToRoleResult.Succeeded == false ? addToRoleResult : createResult;
    }

    public async Task UpdateUserAsync(Guid id)
    {
        User = await userManager.FindByIdAsync(id.ToString());
    }
}