using System.ComponentModel.DataAnnotations;

namespace Application.Users.Commands.RegisterUser;

public record RegisterUserDto(
    string Name, 
    string Surname, 
    string Pin,
    string JobTitle,
    string Phone,
    bool Active,
    Guid BranchId,
    Guid StaffingId
);