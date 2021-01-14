using System;

namespace Task_3
{
    public interface INote
    {
        int Id { get; } //Identificator
        string Title { get; } //Title of Note
        string Text { get; } //Text of Note
        DateTime CreatedOn { get; } //Creation Date
    }
}