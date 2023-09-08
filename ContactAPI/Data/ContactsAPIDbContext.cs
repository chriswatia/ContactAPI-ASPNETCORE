using ContactAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Data
{
    public class ContactsAPIDbContext : IdentityDbContext
    {
        public ContactsAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        //DB Tables
        public DbSet<Contact> Contacts { get; set; }
    }
}
