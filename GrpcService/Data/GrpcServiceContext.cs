using Microsoft.EntityFrameworkCore;
using Shared.DTOs;

namespace GrpcService.Data
{
    public class GrpcServiceContext : DbContext
    {
        public GrpcServiceContext(DbContextOptions<GrpcServiceContext> options) : base(options) { }

        public DbSet<ExampleDto> ExampleDtos { get; set; }
    }
}