using DelegateDI.Bug.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace DelegateDI.Bug.DAL;

internal sealed class BylinesIdentityDbContext: IdentityDbContext<BylinesAccount>
{
	public BylinesIdentityDbContext(DbContextOptions<BylinesIdentityDbContext> options): base(options) {}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema("accounts");

		modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

		modelBuilder.Entity<BylinesAccount>()
					.OwnsOne(a => a.EncryptedPrivateKey,
							 key =>
							 {
								 key.Property(k => k.Ciphertext).HasColumnName("BYL_Private_Ciphertext");

								 key.Property(k => k.Nonce).HasColumnName("BYL_Private_Nonce");

								 key.Property(k => k.Version).HasColumnName("BYL_Private_Version");

								 key.Property(k => k.EphemPublicKey).HasColumnName("BYL_Private_EphemPublicKey");
							 });

		base.OnModelCreating(modelBuilder);
	}
}