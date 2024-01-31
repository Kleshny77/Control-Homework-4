namespace ConsoleApp;
// Самсонов Артём Арменович БПИ238-2. Вариант 12.

using ClassLibrary;
using System;
using System.Collections.Generic;

/// <summary>
/// Класс с основной программой, включающий точку входа Main().
/// </summary>
class Program
{
    /// <summary>
    /// Точка входа.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        Draw.ChangeGoodForegroundColor();
        Menu.Menu1();
    }
}