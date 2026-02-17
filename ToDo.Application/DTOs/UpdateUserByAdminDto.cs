using ToDo.Domain.Enums;

namespace ToDo.Application.DTOs;

public class UpdateUserByAdminDto : UpdateUserDto
{
    public Role? Role { get; set; }
}