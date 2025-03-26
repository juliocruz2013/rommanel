namespace RommanelTeste.Domain.Entities;

public abstract class BaseEntity
{
	public DateTime CreatedAt { get; set; }
	public DateTime? ModifiedAt { get; set; }

    public void SetCreatedAt() => CreatedAt = DateTime.Now;
    public void SetModifiedAt() => ModifiedAt = DateTime.Now;
}
