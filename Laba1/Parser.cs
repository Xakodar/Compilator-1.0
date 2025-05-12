/*using System;
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
*/

using System;
using System.Collections.Generic;

namespace TetradApp
{
    public class Quad
    {
        public string Op { get; }
        public string Arg1 { get; }
        public string Arg2 { get; }
        public string Result { get; }

        public Quad(string op, string a1, string a2, string res)
        {
            Op = op;
            Arg1 = a1;
            Arg2 = a2;
            Result = res;
        }
    }

    public class SyntaxError
    {
        public string Message { get; }
        public int Position { get; }
        public int Length { get; }

        public SyntaxError(string msg, int pos, int len)
        {
            Message = msg;
            Position = pos;
            Length = len;
        }
    }

    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _pos;
        private int _tempCount;

        public List<Quad> Quads { get; } = new List<Quad>();
        public List<SyntaxError> Errors { get; } = new List<SyntaxError>();
        private Token Peek(int offset) => (_pos + offset) < _tokens.Count ? _tokens[_pos + offset] : new Token(TokenType.End, string.Empty, _pos + offset);

        private Token Current => _tokens[_pos];

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _pos = 0;
            _tempCount = 0;
        }

        private void Advance()
        {
            if (_pos < _tokens.Count - 1)
                _pos++;
        }

        private bool Match(TokenType expected)
        {
            if (Current.Type == expected)
            {
                Advance();
                return true;
            }
            Errors.Add(new SyntaxError(
                $"Ожидался «{expected}», а встретилось «{Current.Lexeme}»",
                Current.Position,
                Current.Lexeme.Length));
            return false;
        }

        private string NewTemp() => "t" + (++_tempCount);

        public void Parse()
        {
            ParseStatement();
            if (Current.Type != TokenType.End)
            {
                Errors.Add(new SyntaxError(
                    $"Лишний токен «{Current.Lexeme}» после конца оператора",
                    Current.Position,
                    Current.Lexeme.Length));
            }
        }

        private void ParseStatement()
        {
            // Assignment: id = expr
            if (Current.Type == TokenType.Identifier && Peek(1).Type == TokenType.Assign)
            {
                var id = Current.Lexeme;
                Advance(); // id
                Advance(); // =
                var exprRes = ParseE();
                if (string.IsNullOrEmpty(exprRes))
                {
                    Errors.Add(new SyntaxError(
                        "Ожидалось выражение после «=»",
                        Current.Position,
                        Current.Lexeme.Length));
                }
                else
                {
                    Quads.Add(new Quad("=", exprRes, string.Empty, id));
                }
            }
            // Unexpected closing parenthesis at top-level
            else if (Current.Type == TokenType.RParen)
            {
                Errors.Add(new SyntaxError(
                    "Неправильная закрывающая скобка без соответствующей открывающей",
                    Current.Position,
                    Current.Lexeme.Length));
                Advance();
            }
            else
            {
                // Regular expression
                _ = ParseE();
            }
        }

        private string ParseE()
        {
            if (Current.Type == TokenType.RParen)
            {
                Errors.Add(new SyntaxError(
                    "Неправильная закрывающая скобка без соответствующей открывающей",
                    Current.Position,
                    Current.Lexeme.Length));
                Advance();
                return string.Empty;
            }

            if (Current.Type == TokenType.Plus || Current.Type == TokenType.Minus ||
                Current.Type == TokenType.Mul ||  Current.Type == TokenType.Div)
            {
                // Operator without left operand
                Errors.Add(new SyntaxError(
                    $"Ожидался идентификатор или «(» перед «{Current.Lexeme}»",
                    Current.Position,
                    Current.Lexeme.Length));
                Advance();
            }

            var t = ParseT();
            return ParseA(t);
        }

        private string ParseA(string inh)
        {
            while (Current.Type == TokenType.Plus || Current.Type == TokenType.Minus)
            {
                var op = Current.Lexeme;
                Advance();
                var t2 = ParseT();
                var res = NewTemp();
                Quads.Add(new Quad(op, inh, t2, res));
                inh = res;
            }
            return inh;
        }

        private string ParseT()
        {
            var o = ParseO();
            return ParseB(o);
        }
        private string ParseB(string inh)
        {
            while (Current.Type == TokenType.Mul || Current.Type == TokenType.Div)
            {
                var op = Current.Lexeme;
                Advance();
                var o2 = ParseO();
                var res = NewTemp();
                Quads.Add(new Quad(op, inh, o2, res));
                inh = res;
            }
            return inh;
        }

        private string ParseO()
        {
            if (Current.Type == TokenType.Identifier)
            {
                var id = Current.Lexeme;
                Advance();
                return id;
            }
            if (Current.Type == TokenType.LParen)
            {
                var pos = Current.Position;
                Advance();
                var e = ParseE();
                if (!Match(TokenType.RParen))
                {
                    Errors.Add(new SyntaxError(
                        "Отсутствует закрывающая скобка «)»",
                        pos,
                        1));
                }
                return e;
            }
            if (Current.Type == TokenType.RParen)
            {
                Errors.Add(new SyntaxError(
                    "Неправильная закрывающая скобка без соответствующей открывающей",
                    Current.Position,
                    Current.Lexeme.Length));
                Advance();
                return string.Empty;
            }

            // Unexpected token
            Errors.Add(new SyntaxError(
                $"Непредвиденный токен «{Current.Lexeme}», ожидался идентификатор или «(»",
                Current.Position,
                Current.Lexeme.Length));
            Advance();
            return string.Empty;
        }
    }
}