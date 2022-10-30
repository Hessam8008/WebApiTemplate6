using Domain.Entities;
using Domain.Enums;

namespace Application.Contract.Queries;

/// <summary>
///     Output of contact.
/// </summary>
public sealed record ContactResponse
{
    /// <summary>
    ///     Constructor for ContactResponse.
    /// </summary>
    /// <param name="contact"></param>
    public ContactResponse(Contact contact)
    {
        Id = contact.Id;
        Caption = contact.Caption;
        Title = contact.Title;
        InternalNumber = contact.InternalNumber;
        Building = contact.Building;
        BackColor = contact.BackColor.ToArgb();
        EmployeeCode = contact.EmployeeCode!;
        CreateTime = contact.CreateTime;
        IsActive = contact.IsActive;
    }

    public Guid Id { get; }
    public string Caption { get; }
    public string Title { get; }
    public int InternalNumber { get; }
    public Building Building { get; }
    public int BackColor { get; }
    public int EmployeeCode { get; }
    public DateTime CreateTime { get; }
    public bool IsActive { get; }
}