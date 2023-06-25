namespace Livemedy.Core.Base.Entities;

/// <summary>
/// IHardDelete interface ile Veritabanından silmek istediğimiz modellerimizi işaretliyoruz.
/// </summary>
public interface IHardDelete { }

/// <summary>
/// ISearchableName interface ile işaretlediğimiz modelimizde Name alanı olmasını zorluyor ve bu alanda arama yapılacağını anlıyoruz.
/// </summary>
public interface ISearchableName
{
    public string Name { get; set; }
}
public interface ILockedEntity
{
    public int Id { get; set; }
    public bool IsLocked { get; set; }
    string LockedBy { get; }
}
