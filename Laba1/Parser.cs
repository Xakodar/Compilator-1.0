using System;
using System.Collections.Generic;
using System.Linq;

public class ParseError
{
    public string Message { get; set; }   // Описание ошибки
    public char Symbol { get; set; }        // Символ, вызвавший ошибку (или '\0', если конец строки)
    public int Position { get; set; }       // Позиция символа в строке

    public ParseError(string message, char symbol, int position)
    {
        Message = message;
        Symbol = symbol;
        Position = position;
    }

    public override string ToString()
    {
        // Для удобства позицию можно вывести как Position+1 (если хотим 1-based)
        return $"[Позиция {Position + 1}] {Message} (символ '{Symbol}')";
    }
}

public class ListParser
{
    private string _input;
    private int _pos;                     // Текущая позиция в строке
    public List<ParseError> Errors { get; private set; }

    // Константа для обозначения конца строки
    private const char EOF = '\0';

    public ListParser()
    {
        Errors = new List<ParseError>();
    }

    /// <summary>
    /// Основной метод парсера: разбирает строку по грамматике
    /// List -> <ID> '=' '[' <ELEMS> ']' ';'
    /// </summary>
    public void Parse(string input)
    {
        _input = input ?? "";
        _pos = 0;
        Errors.Clear();

        SkipSpaces();

        ParseIdentifier();      // Парсим идентификатор (должен состоять только из букв)
        SkipSpaces();

        if (!TryReadChar('='))
        {
            AddError("Ожидался символ '='", CurrentChar());
            // Попытаемся восстановиться: синхронизация по символу '['
            Recover(new HashSet<char> { '[' });
        }
        SkipSpaces();

        if (!TryReadChar('['))
        {
            AddError("Ожидался символ '['", CurrentChar());
            Recover(new HashSet<char> { ']' });
        }
        SkipSpaces();

        ParseElements();        // Парсинг элементов списка
        SkipSpaces();

        if (!TryReadChar(']'))
        {
            AddError("Ожидался символ ']'", CurrentChar());
            Recover(new HashSet<char> { ';' });
        }
        SkipSpaces();

        if (!TryReadChar(';'))
        {
            AddError("Ожидался символ ';' в конце", CurrentChar());
            // Восстанавливаемся до конца строки
            Recover(new HashSet<char> { EOF });
        }
        SkipSpaces();

        // Если после ';' ещё остались символы, считаем их ошибкой
        if (!IsEOF())
        {
            AddError("Лишние символы после завершения списка", CurrentChar());
            while (!IsEOF()) AdvancePos();
        }
    }

    /// <summary>
    /// Парсит идентификатор: последовательность букв
    /// </summary>
    private void ParseIdentifier()
    {
        if (!IsLetter(CurrentChar()))
        {
            AddError("Идентификатор должен начинаться с буквы", CurrentChar());
            // Восстанавливаемся до символа '=' или другого разделителя
            Recover(new HashSet<char> { '=' });
            return;
        }

        // Читаем первую букву
        AdvancePos();

        // Читаем оставшиеся буквы идентификатора
        while (IsLetter(CurrentChar()))
        {
            AdvancePos();
        }
    }

    /// <summary>
    /// Парсит список элементов: <ELEMS> -> (пусто) | <ELEM> { ',' <ELEM> } [ ',' ]
    /// </summary>
    private void ParseElements()
    {
        SkipSpaces();

        // Если сразу встречаем закрывающую скобку, значит список пустой.
        if (CurrentChar() == ']')
            return;

        // Парсим первый элемент
        ParseElement();

        // Цикл: ожидаем запятую, затем следующий элемент
        while (true)
        {
            SkipSpaces();
            if (CurrentChar() == ',')
            {
                AdvancePos(); // Пропускаем запятую
                SkipSpaces();
                // Если после запятой сразу ']', это допустимая лишняя запятая (как в Python)
                if (CurrentChar() == ']')
                    break;
                ParseElement();
            }
            else break;
        }
    }

    /// <summary>
    /// Парсит один элемент: <ELEM> -> <NUMBER> | <STRING>
    /// </summary>
    private void ParseElement()
    {
        SkipSpaces();
        char c = CurrentChar();
        if (c == '"')
        {
            ParseString();
        }
        else if (c == '+' || c == '-' || IsDigit(c))
        {
            ParseNumber();
        }
        else
        {
            AddError("Ожидался элемент (число или строка в кавычках)", c);
            // Восстанавливаемся до разделителя: запятая, ']' или ';'
            Recover(new HashSet<char> { ',', ']', ';' });
        }
    }

    /// <summary>
    /// Парсит строку вида: '"' {любые символы, кроме '"'} '"'
    /// </summary>
    private void ParseString()
    {
        if (!TryReadChar('"'))
        {
            AddError("Ожидался символ '\"' в начале строки", CurrentChar());
            Recover(new HashSet<char> { ',', ']', ';' });
            return;
        }

        // Читаем символы до закрывающей кавычки
        while (!IsEOF() && CurrentChar() != '"')
        {
            AdvancePos();
        }

        if (CurrentChar() == '"')
        {
            AdvancePos(); // Пропускаем закрывающую кавычку
        }
        else
        {
            AddError("Строка не закрыта кавычкой", CurrentChar());
            Recover(new HashSet<char> { ',', ']', ';' });
        }
    }

    /// <summary>
    /// Парсит число: [ '+' | '-' ] <DIGITS> [ '.' <DIGITS> ]
    /// </summary>
    private void ParseNumber()
    {
        // Обрабатываем возможный знак
        if (CurrentChar() == '+' || CurrentChar() == '-')
        {
            AdvancePos();
        }

        if (!IsDigit(CurrentChar()))
        {
            AddError("После знака числа должна идти цифра", CurrentChar());
            Recover(new HashSet<char> { ',', ']', ';' });
            return;
        }

        // Читаем цифры
        while (IsDigit(CurrentChar()))
        {
            AdvancePos();
        }

        // Обрабатываем десятичную точку, если она есть
        if (CurrentChar() == '.')
        {
            AdvancePos(); // Пропускаем точку
            if (!IsDigit(CurrentChar()))
            {
                AddError("После точки должна идти хотя бы одна цифра", CurrentChar());
                Recover(new HashSet<char> { ',', ']', ';' });
                return;
            }
            while (IsDigit(CurrentChar()))
            {
                AdvancePos();
            }
        }
    }

    // ------------------ Методы восстановления (нейтрализации ошибок) ------------------

    /// <summary>
    /// Метод восстановления по синхронизации (метод Айронса).
    /// Пропускает входные символы до тех пор, пока не встретит один из символов из syncSet.
    /// Это позволяет выйти из «режима ошибки» и продолжить анализ.
    /// </summary>
    /// <param name="syncSet">Множество синхронизирующих символов</param>
    private void Recover(HashSet<char> syncSet)
    {
        // Если дошли до конца, просто выходим.
        while (!IsEOF() && !syncSet.Contains(CurrentChar()))
        {
            AdvancePos();
        }
    }

    // ------------------ Вспомогательные методы ------------------

    private void SkipSpaces()
    {
        // char.IsWhiteSpace учитывает пробелы, табуляцию, символы перевода строки и т.д.
        while (!IsEOF() && char.IsWhiteSpace(CurrentChar()))
        {
            _pos++;
        }
    }

    private char CurrentChar()
    {
        if (_pos >= _input.Length)
            return EOF;
        return _input[_pos];
    }

    private bool IsEOF()
    {
        return _pos >= _input.Length;
    }

    private void AdvancePos()
    {
        if (_pos < _input.Length)
            _pos++;
    }

    /// <summary>
    /// Пытается прочитать указанный символ. Если текущий символ совпадает, сдвигает позицию и возвращает true.
    /// </summary>
    private bool TryReadChar(char expected)
    {
        if (CurrentChar() == expected)
        {
            AdvancePos();
            return true;
        }
        return false;
    }

    private void AddError(string message, char symbol)
    {
        Errors.Add(new ParseError(message, symbol, _pos));
    }

    private bool IsLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    private bool IsDigit(char c)
    {
        return (c >= '0' && c <= '9');
    }
}
