using System;
using System.Collections.Generic;
using System.Linq;

public class ParseError
{
    public string Message { get; }
    public char Symbol { get; }
    public int Position { get; }

    public ParseError(string message, char symbol, int position)
    {
        Message = message;
        Symbol = symbol == '\0' ? '∅' : symbol;
        Position = position;
    }

    public override string ToString()
        => $"[позиция {Position + 1}] {Message} (символ '{Symbol}')";
}

public class ListParser
{
    private string _input;
    private int _pos;
    public List<ParseError> Errors { get; } = new List<ParseError>();

    private const char EOF = '\0';

    public void Parse(string input)
    {
        _input = input ?? "";
        _pos = 0;
        Errors.Clear();

        SkipSpaces();
        ParseIdentifier();
        SkipSpaces();

        // Ожидаем '='
        ExpectChar('=', "Ожидался символ '='", new[] { '[' });
        SkipSpaces();

        // Ожидаем '['
        ExpectChar('[', "Ожидался символ '['", new[] { ']' });
        SkipSpaces();

        // Элементы списка
        ParseElements();
        SkipSpaces();

        // Ожидаем ']'
        ExpectChar(']', "Ожидался символ ']'", new[] { ';' });
        SkipSpaces();

        // Ожидаем ';'
        ExpectChar(';', "Ожидался символ ';'", new[] { EOF });
        SkipSpaces();

        // Лишние символы после ';'
        if (!IsEOF())
            AddError("Лишние символы после конца списка", CurrentChar(), _pos);
    }

    private void ParseIdentifier()
    {
        // <ID> -> <LETTER> { <LETTER> }
        if (!IsLetter(CurrentChar()))
        {
            AddError("Идентификатор должен начинаться с буквы", CurrentChar(), _pos);
            Recover(new[] { '=', EOF });
            return;
        }

        AdvancePos(); // первая буква

        // оставшиеся буквы — только буквы
        while (!IsEOF())
        {
            char c = CurrentChar();
            if (IsLetter(c))
            {
                AdvancePos();
            }
            else if (char.IsDigit(c))
            {
                AddError("Идентификатор может содержать только английские буквы", c, _pos);
                AdvancePos();
            }
            else break;
        }
    }

    private void ParseElements()
    {
        // <ELEMS> -> ε | <ELEM> { ',' <ELEM> } [',' ]
        SkipSpaces();
        if (CurrentChar() == ']')
            return;  // пустой список допускаем

        ParseElement();

        while (true)
        {
            SkipSpaces();
            if (CurrentChar() == ',')
            {
                AdvancePos();
                SkipSpaces();
                if (CurrentChar() == ']')
                    break; // trailing comma ─ тоже ок
                ParseElement();
            }
            else break;
        }
    }

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
            AddError("Ожидался элемент списка: число или строка в кавычках", c, _pos);
            Recover(new[] { ',', ']', ';' });
        }
    }

    private void ParseString()
    {
        // '"' { любые символы, кроме '"' } '"'
        if (!TryReadChar('"'))
        {
            AddError("Ожидался символ '\"' при начале строки", CurrentChar(), _pos);
            Recover(new[] { ',', ';', ']' });
            return;
        }

        while (!IsEOF() && CurrentChar() != '"')
            AdvancePos();

        if (CurrentChar() == '"')
            AdvancePos();
        else
        {
            AddError("Строка не закрыта кавычкой", EOF, _pos);
            Recover(new[] { ',', ';', ']' });
        }
    }

    private void ParseNumber()
    {
        // [ '+' | '-' ] <DIGITS> [ '.' <DIGITS> ]
        if (CurrentChar() == '+' || CurrentChar() == '-')
            AdvancePos();

        if (!IsDigit(CurrentChar()))
        {
            AddError("После знака числа должна идти цифра", CurrentChar(), _pos);
            Recover(new[] { ',', ';', ']' });
            return;
        }

        // DIGITS
        while (IsDigit(CurrentChar()))
            AdvancePos();

        // дробная часть
        if (CurrentChar() == '.')
        {
            AdvancePos(); // точка
            if (!IsDigit(CurrentChar()))
            {
                AddError("После точки должна идти хотя бы одна цифра", CurrentChar(), _pos);
                Recover(new[] { ',', ';', ']' });
                return;
            }
            while (IsDigit(CurrentChar()))
                AdvancePos();
        }
    }

    // ----------------- вспомогательные методы -----------------

    private void SkipSpaces()
    {
        while (!IsEOF() && char.IsWhiteSpace(CurrentChar()))
            _pos++;
    }

    private char CurrentChar()
        => _pos >= _input.Length ? EOF : _input[_pos];

    private bool IsEOF()
        => _pos >= _input.Length;

    private void AdvancePos()
    {
        if (_pos < _input.Length)
            _pos++;
    }

    private bool TryReadChar(char expected)
    {
        if (CurrentChar() == expected)
        {
            AdvancePos();
            return true;
        }
        return false;
    }

    private void ExpectChar(char expected, string errMsg, IEnumerable<char> syncTokens)
    {
        if (!TryReadChar(expected))
        {
            AddError(errMsg, CurrentChar(), _pos);
            Recover(syncTokens);
        }
    }

    /// <summary>
    /// Метод Айронса: пропускаем всё, пока не встретим один из syncTokens.
    /// После этого считаем, что синхронизировались и парсим дальше.
    /// </summary>
    private void Recover(IEnumerable<char> syncTokens)
    {
        var sync = new HashSet<char>(syncTokens) { EOF };
        while (!IsEOF() && !sync.Contains(CurrentChar()))
            AdvancePos();
    }

    private void AddError(string message, char symbol, int position)
        => Errors.Add(new ParseError(message, symbol, position));

    private bool IsLetter(char c)
        => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');

    private bool IsDigit(char c)
        => (c >= '0' && c <= '9');
}
