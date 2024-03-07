using Microsoft.EntityFrameworkCore;
using HipicaFacil.Pages;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

}