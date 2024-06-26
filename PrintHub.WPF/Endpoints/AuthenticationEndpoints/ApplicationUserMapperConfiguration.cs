﻿using PrintHub.Infrastructure;
using PrintHub.WPF.Endpoints.AuthenticationEndpoints.Update;

namespace PrintHub.WPF.Endpoints.AuthenticationEndpoints;

public class ApplicationUserMapperConfiguration : Profile
{
    public ApplicationUserMapperConfiguration()
    {
        CreateMap<ApplicationUser, ApplicationUserUpdateDto>();

        CreateMap<ApplicationUserUpdateDto, ApplicationUser>()
            .ForMember(user => user.Roles, expression => expression.Ignore())
            .ForMember(user => user.Client, expression => expression.Ignore())
            .ForMember(user => user.ClientId, expression => expression.Ignore())
            .ForMember(user => user.NormalizedUserName, expression => expression.Ignore())
            .ForMember(user => user.Email, expression => expression.Ignore())
            .ForMember(user => user.NormalizedEmail, expression => expression.Ignore())
            .ForMember(user => user.EmailConfirmed, expression => expression.Ignore())
            .ForMember(user => user.PasswordHash, expression => expression.Ignore())
            .ForMember(user => user.SecurityStamp, expression => expression.Ignore())
            .ForMember(user => user.ConcurrencyStamp, expression => expression.Ignore())
            .ForMember(user => user.PhoneNumber, expression => expression.Ignore())
            .ForMember(user => user.PhoneNumberConfirmed, expression => expression.Ignore())
            .ForMember(user => user.TwoFactorEnabled, expression => expression.Ignore())
            .ForMember(user => user.LockoutEnd, expression => expression.Ignore())
            .ForMember(user => user.LockoutEnabled, expression => expression.Ignore())
            .ForMember(user => user.AccessFailedCount, expression => expression.Ignore());
    }
}