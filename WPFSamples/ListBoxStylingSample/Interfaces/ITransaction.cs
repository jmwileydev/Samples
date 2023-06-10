using System;

namespace ListBoxStylingSample.Interfaces;

public interface ITransaction
{
    DateTime Date { get; }
    String Description { get; }

    double Amount { get; }

    String Category { get; }
}
