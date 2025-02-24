using CustomForms.Application.DTOs;
using System.Security.Cryptography;

namespace CustomForms.Application.Repositories.Interfaces
{
    public interface IFormRepository
    {
        Task Create(FormDTO dto, CancellationToken cancellationToken);
    }
}
