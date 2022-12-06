using System.Drawing;
using Domain.DomainEvents;
using Domain.Enums;
using Domain.Errors;
using Domain.Primitives;
using Domain.Primitives.Result;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Contact : AggregateRoot
{
    protected Contact()
    {
    }

    private Contact(Guid id) : base(id)
    {
    }

    public ContactTitle Title { get; private set; }
    public ContactCaption Caption { get; private set; }
    public InternalNumber InternalNumber { get; private set; }
    public Building Building { get; private set; }
    public Color BackColor { get; private set; }
    public EmployeeCode? EmployeeCode { get; private set; }
    public DateTime CreateTime { get; private set; }
    public bool IsActive { get; private set; }


    public static Result<Contact> Create(ContactTitle title, ContactCaption caption, InternalNumber internalNumber,
        Building building, EmployeeCode? employeeCode, bool existsInternalNumber)
    {
        if (existsInternalNumber)
            return DomainErrors.Contact.DuplicateInternalNumber(internalNumber);

        var result = new Contact
        {
            Title = title,
            Caption = caption,
            InternalNumber = internalNumber,
            Building = building,
            BackColor = Color.Empty,
            EmployeeCode = employeeCode,
            CreateTime = DateTime.Now,
            IsActive = true
        };
        result.AddDomainEvent(new ContactCreated(result));
        return result;
    }


    public void Inactive()
    {
        IsActive = false;
    }

    public void Active()
    {
        IsActive = true;
    }

    public void ChangeTitle(ContactTitle newTitle)
    {
        AddDomainEvent(new ContactTitleChanged(Title, newTitle));
        Title = newTitle;
    }

    public void ChangeCaption(ContactCaption newCaption)
    {
        AddDomainEvent(new ContactCaptionChanged(Caption, newCaption));
        Caption = newCaption;
    }

    public void ChangeInternalNumber(InternalNumber newNumber)
    {
        InternalNumber = newNumber;
    }

    public void ChangeEmployee(EmployeeCode newEmployee)
    {
        EmployeeCode = newEmployee;
    }
}