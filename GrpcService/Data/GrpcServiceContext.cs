using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace GrpcService.Data
{
    public class GrpcServiceContext(DbContextOptions<GrpcServiceContext> options) : DbContext(options)
    {
        public DbSet<ExampleDto> ExampleDtos { get; set; }
    }
}